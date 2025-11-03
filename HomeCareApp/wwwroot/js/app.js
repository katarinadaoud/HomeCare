let calendar;
let calendarInited = false;
let selectedCell = null;

const PHONE_SOS = "+4746956500"; // The number to the HomeCareSOS service

/** Load a CSS file only once */
function loadCssOnce(href) {
  // Check if the CSS is already on the page:
  // - existing <link rel="stylesheet"> whose href contains our path
  // - an existing stylesheet in document.styleSheets
  const exists =
    [...document.querySelectorAll('link[rel="stylesheet"]')].some(l => l.href && l.href.includes(href)) ||
    [...document.styleSheets].some(s => s.href && s.href.includes && s.href.includes(href));

  // If not found, create and append a <link> to load it
  if (!exists) {
    const link = document.createElement('link'); // make a new <link> element
    link.rel = 'stylesheet';
    link.href = href;
    document.head.appendChild(link);
  }
}

/** Quick heuristic for mobile */
function isLikelyMobile() {
  return /Mobi|Android|iPhone|iPad|iPod/i.test(navigator.userAgent);
}

/** Find the calendar container (supports both patient/employer/booking) */
function getCalendarElement() {
  return document.getElementById('kal') || document.getElementById('bookingCal');
}

/* 
 * Simple hash-based view router
 */
function showView(id) {
  // Hide all sections first to guarantee exactly one active view
  document.querySelectorAll('.app-view').forEach(sec => (sec.hidden = true));

  const target = document.getElementById(id); // find the target section by its id
  if (!target) return;                        // if not found, stop
  target.hidden = false;                      // show only the requested section 

  // Lazy-load CSS and do per-view initialization (only when that view is shown)
  if (id === 'view-appointment') {
    // load calendar page CSS once, then init calendar
    loadCssOnce('css/page-calendar.css');
    initCalendarOnce();
    setTimeout(() => calendar && calendar.updateSize(), 0);
  } else if (id === 'view-mine-medisiner') {
    loadCssOnce('css/page-medisiner.css'); // the medicine page is not finished yet
  } else if (id === 'view-book-time') {
    loadCssOnce('css/page-book.css');
  }
}

/*function route() {
  const hash = window.location.hash || '#appointment';
  switch (hash) {
    case '#appointment':   showView('view-appointment');    break;
    case '#book-time':     showView('view-book-time');      break;
    case '#mine-medisiner':showView('view-mine-medisiner'); break;
    default:               showView('view-appointment');
  }
}

/*Calendar (FullCalendar)*/

function initCalendarOnce() {
  if (calendarInited) return;

  const el = getCalendarElement();
  if (!el || typeof FullCalendar === 'undefined') return;

  const todayISO = new Date().toISOString().slice(0, 10);
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

    // Event source: JSON from backend. FullCalendar sends start/end automatically.
    events: {
      url: '/Appointment/Events',
      method: 'GET',
      extraParams: () => ({ _: Date.now() }), // prevent dev cache
      failure: (e) => console.error('Events fetch failed', e)
    },

    dateClick: (info) => {
      // Mark selected cell
      if (selectedCell) selectedCell.classList.remove('is-selected');
      selectedCell = info.dayEl;
      selectedCell.classList.add('is-selected');

      // FullCalendar v6 uses dateStr 
      const val = info.dateStr; 

      // Write ONLY to the real date field
      const hiddenByName = document.querySelector('input[name="Appointment.Date"]');
      if (hiddenByName) hiddenByName.value = val;
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

  
  document.addEventListener('keydown', e => {
    // Patient: only when "#appointment" is shown. Employee/booking: always when calendar exists.
    if (window.AppRole === 'patient' && window.location.hash !== '#appointment') return;
    if (!calendarInited) return;

    if (e.key === 'ArrowLeft')         calendar.prev();
    else if (e.key === 'ArrowRight')   calendar.next();
    // else if (e.key.toLowerCase() === 't') calendar.today();
    // else if (e.key.toLowerCase() === 'l') toggleView();
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
/* UI: notifications toggle */
document.getElementById('notifBtn')?.addEventListener('click', async () => {
  const toastEl = document.getElementById('notifToast');
  const bodyEl  = document.getElementById('notifToastBody');
  const Toast   = window.bootstrap?.Toast;
  if (!toastEl || !Toast) return;

  // === TOGGLE: hvis synlig → lukk og tøm, ferdig ===
  const inst = Toast.getInstance(toastEl) ?? new Toast(toastEl);
  if (toastEl.classList.contains('show')) {
    inst.hide();
    if (bodyEl) bodyEl.innerHTML = '';            // fjern varsler fra skjermen
    return;
  }

  // === Ikke synlig → last inn, merk lest, vis ===
  if (bodyEl) bodyEl.innerHTML = '<div class="text-muted">Laster…</div>';

  try {
    const res = await fetch(`/Notifications/Latest?patientId=${window.AppPatientId}&take=5`, { cache: 'no-store' });
    const items = res.ok ? await res.json() : [];
    if (Array.isArray(items) && items.length) {
      const html = `
        <ul class="notif-list">
          ${items.map(n => `
            <li class="notif-item">
              <span class="notif-dot" aria-hidden="true"></span>
              <div class="notif-content">
                <div class="notif-title">${n.message ?? n.Message}</div>
                <div class="notif-time">${new Date(n.createdAt ?? n.CreatedAt).toLocaleString()}</div>
              </div>
            </li>
          `).join('')}
        </ul>`;
      bodyEl.innerHTML = html;
    } else {
      bodyEl.textContent = 'Ingen varsler…';
    }
  } catch {
    if (bodyEl) bodyEl.textContent = 'Klarte ikke å hente varsler';
  }

  // Marker alle lest og skjul badge (så tallet ikke kommer tilbake)
  try {
    const token = document.querySelector('meta[name="request-verification-token"]')?.content;
    await fetch(`/Notifications/MarkAllRead?patientId=${window.AppPatientId}`, {
      method: 'POST',
      headers: token ? { 'RequestVerificationToken': token } : {}
    });
    const badgeEl = document.getElementById('notifBadge');
    if (badgeEl) { badgeEl.style.display = 'none'; badgeEl.textContent = ''; }
  } catch { /* ignore */ }

  // Vis toasten (autolukk etter 4s, men toggle tar uansett)
  toastEl.removeAttribute('hidden');
  const t = new Toast(toastEl, { autohide: true, delay: 4000 });
  t.show();
});

  /*document.getElementById('notifBtn')?.addEventListener('click', async () => {
    const toastEl = document.getElementById('notifToast');
    const bodyEl  = document.getElementById('notifToastBody'); // fra _Toasts.cshtml
    if (!toastEl || !window.bootstrap?.Toast || !window.AppPatientId) return;

   // Marker alle som lest når bjella åpnes
try {
  const token = document.querySelector('meta[name="request-verification-token"]')?.content;
  await fetch(`/Notifications/MarkAllRead?patientId=${window.AppPatientId}`, {
    method: 'POST',
    headers: token ? { 'RequestVerificationToken': token } : {}
  });

  // Skjul badge umiddelbart i UI
  const badgeEl = document.getElementById('notifBadge');
  if (badgeEl) { badgeEl.style.display = 'none'; badgeEl.textContent = ''; }
} catch { /* ignorer evt. feil; toast vises uansett */ 

     


  // SOS: confirm -> tel:, with desktop fallback (toast + copy)
  const sosBtn = document.getElementById('sosBtn');
  const sosConfirmBtn = document.getElementById('sosConfirmCallBtn');
  const sosModalEl = document.getElementById('sosConfirmModal');

  if (sosBtn && sosConfirmBtn && sosModalEl && window.bootstrap?.Modal && window.bootstrap?.Toast) {
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
        const deskPhoneText = document.getElementById('deskPhoneText');
        const deskToastEl   = document.getElementById('deskToast');
        if (deskPhoneText && deskToastEl) {
          deskPhoneText.textContent = PHONE_SOS;
          const deskT = new bootstrap.Toast(deskToastEl, { delay: 6000 });
          deskT.show();

          const copyBtn = document.getElementById('copyBtn');
          if (copyBtn) {
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
        }
      }
    });
  }
}

/* Bootstrap  */
document.addEventListener('DOMContentLoaded', () => {
  // Notlet client-side validation block the form submit
  const bookForm =
    document.querySelector('form[method="post"][action*="/Appointment/Book"]') ||
    document.querySelector('form[method="post"][asp-action="Book"]') ||
    document.querySelector('form[action*="/Appointment/Book"]');

  if (bookForm) {
    bookForm.setAttribute('novalidate', '');

    // If jQuery Unobtrusive is loaded: remove validator instance so it won't intercept submit
    if (window.jQuery && window.jQuery.validator) {
      const $f = window.jQuery(bookForm);
      try {
        $f.removeData('validator');
        $f.removeData('unobtrusiveValidation');
      } catch (_) { /* noop*/ }
    }
  }

  const hasBookingField = !!document.querySelector('input[name="Appointment.Date"]');
  const hasCalendarEl = !!getCalendarElement();

  // Init calendar immediately if it exists (booking/employee/patient)
  if (hasCalendarEl && !calendarInited) {
    loadCssOnce('css/page-calendar.css');
    initCalendarOnce();
    setTimeout(() => calendar && calendar.updateSize(), 0);
  }

  // Patient: keep hash-routing between sections
  if (window.AppRole === 'patient') {
    route();
    window.addEventListener('hashchange', route);
  }
});

/* NOTIFICATIONS CONTROLLER */
(function () {
  const pid = window.AppPatientId;
  const btn = document.getElementById('notifBtn');   // bell button that shows badge
  const badge = document.getElementById('notifBadge'); // the number

  if (!btn || !badge || !pid) return; // if any of these are missing, abort

  // Fetch unread count and update UI
  async function refreshBadge() {
    try {
      const res = await fetch(`/Notifications/UnreadCount?patientId=${pid}`, { cache: 'no-store' });
      if (!res.ok) throw new Error('HTTP ' + res.status);
      const count = await res.json();

      badge.textContent = count;
      badge.style.display = count > 0 ? '' : 'none';
      btn.title = count === 1 ? 'You have 1 notification' : `You have ${count} notifications`;
    } catch (e) {
      // silent in dev
    }
  }

  // First run, then every 5s
  refreshBadge();
  setInterval(refreshBadge, 5000);
})();

/* Booking → refetch hook*/
/* Call `document.dispatchEvent(new Event('booking:saved'));` after backend saved the appointment */
document.addEventListener('booking:saved', () => {
  if (calendarInited && calendar) calendar.refetchEvents();
});


