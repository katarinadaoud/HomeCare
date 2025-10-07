let calendar;
let calendarInited = false;
let selectedCell = null;

const PHONE_SOS = "+4712345678"; // ← Replace with the real number

/* --- Helpers ------------------------------------------------------------ */

/** Lazy-load a CSS file once (page-specific CSS) */
function loadCssOnce(href) {
  const exists =
    [...document.querySelectorAll('link[rel="stylesheet"]')].some(l => l.href && l.href.includes(href)) ||
    [...document.styleSheets].some(s => s.href && s.href.includes && s.href.includes(href));

  if (!exists) {
    const link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = href;
    document.head.appendChild(link);
  }
}

/** Quick heuristic for mobile */
function isLikelyMobile() {
  return /Mobi|Android|iPhone|iPad|iPod/i.test(navigator.userAgent);
}

/* --- Routing / Views (pasient) ----------------------------------------- */

function showView(id) {
  document.querySelectorAll('.app-view').forEach(sec => (sec.hidden = true));
  const target = document.getElementById(id);
  if (!target) return;
  target.hidden = false;

  // Lazy-load CSS per view
  if (id === 'view-mine-timeavtaler') {
    loadCssOnce('css/page-calendar.css');
    initCalendarOnce();
    setTimeout(() => calendar && calendar.updateSize(), 0);
  } else if (id === 'view-mine-medisiner') {
    loadCssOnce('css/page-medisiner.css'); // create when you build that page
  } else if (id === 'view-book-time') {
    loadCssOnce('css/page-book.css');      // create when you build that page
  }
}

function route() {
  const hash = window.location.hash || '#mine-timeavtaler';
  switch (hash) {
    case '#mine-timeavtaler': showView('view-mine-timeavtaler'); break;
    case '#book-time':        showView('view-book-time');        break;
    case '#mine-medisiner':   showView('view-mine-medisiner');   break;
    default:                  showView('view-mine-timeavtaler');
  }
}

/* --- Calendar (FullCalendar) ------------------------------------------- */

function initCalendarOnce() {
  if (calendarInited) return;

  const el = document.getElementById('kal');
  if (!el) return;

  const todayISO = new Date().toISOString().slice(0, 10);
  const demoEvents = [
    { title: 'GP',            start: todayISO + 'T10:00:00' },
    { title: 'Physiotherapy', start: todayISO + 'T15:00:00' },
  ];

  const savedView = localStorage.getItem('kalView') || 'dayGridMonth';

  calendar = new FullCalendar.Calendar(el, {
    initialView: savedView,
    height: 'auto',
    firstDay: 1,
    locale: 'en',                 // English month/day names
    headerToolbar: false,
    weekNumbers: true,
    weekNumberCalculation: 'local',
    // Show U instead of W
    weekNumberContent: (arg) => ({ html: `<span class="badge bg-light text-dark fw-bold">U${arg.num}</span>` }),
    events: demoEvents,

    dateClick: (info) => {
      if (selectedCell) selectedCell.classList.remove('is-selected');
      selectedCell = info.dayEl;
      selectedCell.classList.add('is-selected');
    },
  });

  calendar.render();
  calendarInited = true;

  function oppdaterTittel() {
    const d = calendar.view.currentStart;
    const txt = d.toLocaleDateString('en-US', { month: 'long', year: 'numeric' });
    const tittelEl = document.getElementById('kalTittel');
    if (tittelEl) tittelEl.textContent = txt;
    oppdaterTomTilstand();
  }
  oppdaterTittel();
  calendar.on('datesSet', oppdaterTittel);

  /* Navigation */
  document.getElementById('btnPrev')?.addEventListener('click', () => calendar.prev());
  document.getElementById('btnNext')?.addEventListener('click', () => calendar.next());
  document.getElementById('btnToday')?.addEventListener('click', () => calendar.today());

  /* Jump-to-date */
  const jumpDate = document.getElementById('jumpDate');
  if (jumpDate) {
    if (!jumpDate.value) jumpDate.value = todayISO;

    const goTo = () => {
      const v = jumpDate.value;
      if (!v) return;
      const d = new Date(`${v}T00:00:00`);
      calendar.gotoDate(d);
    };

    jumpDate.addEventListener('input', goTo);
    jumpDate.addEventListener('change', goTo);
  }

  /* View toggle + persist */
  const btnMonth = document.getElementById('btnViewMonth');
  const btnList  = document.getElementById('btnViewList');

  function setPressed(monthPressed) {
    btnMonth?.setAttribute('aria-pressed', String(monthPressed));
    btnList ?.setAttribute('aria-pressed', String(!monthPressed));
  }
  function toggleView(){
    if (calendar.view.type === 'dayGridMonth') {
      calendar.changeView('listMonth'); setPressed(false);
    } else {
      calendar.changeView('dayGridMonth'); setPressed(true);
    }
  }
  btnMonth?.addEventListener('click', () => { calendar.changeView('dayGridMonth'); setPressed(true); });
  btnList ?.addEventListener('click', () => { calendar.changeView('listMonth');   setPressed(false); });
  setPressed(calendar.view.type === 'dayGridMonth');
  calendar.on('viewDidMount', (arg) => localStorage.setItem('kalView', arg.view.type));

  /* Keyboard: ←/→, T, L */
  document.addEventListener('keydown', e => {
    // Pasient: kun når "#mine-timeavtaler" vises. Ansatt: alltid når kalender finnes.
    if (window.AppRole === 'patient' && window.location.hash !== '#mine-timeavtaler') return;
    if (!calendarInited) return;

    if (e.key === 'ArrowLeft')         calendar.prev();
    else if (e.key === 'ArrowRight')   calendar.next();
    else if (e.key.toLowerCase() === 't') calendar.today();
    else if (e.key.toLowerCase() === 'l') toggleView();
  });

  /* Dot indicators */
  calendar.on('eventsSet', tegnPrikker);
  tegnPrikker();

  function tegnPrikker(){
    const counts = {};
    calendar.getEvents().forEach(e => {
      const d = e.startStr.slice(0,10);
      counts[d] = (counts[d]||0)+1;
    });

    document.querySelectorAll('.fc-daygrid-day').forEach(cell => {
      cell.querySelector('.dot-wrap')?.remove();
      const date = cell.getAttribute('data-date');
      const n = counts[date]||0;
      if(!n) return;
      const wrap = document.createElement('div');
      wrap.className = 'dot-wrap';
      const dots = Math.min(n,3);
      for(let i=0;i<dots;i++){
        const b = document.createElement('span'); b.className='dot';
        wrap.appendChild(b);
      }
      cell.querySelector('.fc-daygrid-day-frame')?.appendChild(wrap);
    });

    oppdaterTomTilstand();
  }

  /* Empty state */
  function oppdaterTomTilstand(){
    const empty = document.getElementById('emptyState');
    if (!empty) return;
    const rangeStart = calendar.view.currentStart;
    const rangeEnd   = calendar.view.currentEnd;
    const hasEvents = calendar.getEvents().some(e => e.start >= rangeStart && e.start < rangeEnd);
    empty.classList.toggle('show', !hasEvents);
  }

  /* UI: notifications + SOS */
  document.getElementById('notifBtn')?.addEventListener('click', () => {
    const toastEl = document.getElementById('notifToast');
    new bootstrap.Toast(toastEl, { delay: 2500 }).show();
  });

  // SOS: confirm -> tel:, with desktop fallback (toast + copy)
  const sosBtn = document.getElementById('sosBtn');
  const sosConfirmBtn = document.getElementById('sosConfirmCallBtn');
  const sosModalEl = document.getElementById('sosConfirmModal');

  if (sosBtn && sosConfirmBtn && sosModalEl) {
    const sosModal = new bootstrap.Modal(sosModalEl);

    sosBtn.addEventListener('click', () => { sosModal.show(); });

    sosConfirmBtn.addEventListener('click', () => {
      sosModal.hide();

      // Feedback
      new bootstrap.Toast(document.getElementById('alarmToast'), { delay: 1800 }).show();

      const mobile = isLikelyMobile();
      // Attempt call (must be triggered from a click context)
      window.location.href = `tel:${PHONE_SOS}`;

      // Desktop fallback
      if (!mobile) {
        document.getElementById('deskPhoneText').textContent = PHONE_SOS;
        const deskT = new bootstrap.Toast(document.getElementById('deskToast'), { delay: 6000 });
        deskT.show();

        const copyBtn = document.getElementById('copyBtn');
        copyBtn.onclick = async () => {
          try {
            await navigator.clipboard.writeText(PHONE_SOS);
            copyBtn.textContent = "Copied!";
            setTimeout(()=> copyBtn.textContent = "Copy", 1500);
          } catch {
            copyBtn.textContent = "Copy failed";
            setTimeout(()=> copyBtn.textContent = "Copy", 1500);
          }
        };
      }
    });
  }
}

/* --- Bootstrap ---------------------------------------------------------- */
document.addEventListener('DOMContentLoaded', () => {
  if (window.AppRole === 'patient') {
    // Pasient: hash-routing mellom seksjonene
    route();
    window.addEventListener('hashchange', route);
  } else {
    // Ansatt: initier kalender direkte hvis den finnes på siden
    if (document.getElementById('kal')) {
      loadCssOnce('css/page-calendar.css');   // ← lagt til for employee
      initCalendarOnce();
      setTimeout(() => calendar && calendar.updateSize(), 0);
    }
  }
});
