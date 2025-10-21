(function () {
  const pid = window.AppPatientId;
  const btn = document.getElementById('notifBtn');
  const badge = document.getElementById('notifBadge');
  const list = document.getElementById('notifList');
  const toastEl = document.getElementById('notifToast');
  const Toast = window.bootstrap?.Toast;

  if (!btn || !badge || !pid) return;

  function fmt(dtStr) {
    try { return new Date(dtStr).toLocaleString(); } catch { return dtStr; }
  }
  async function refreshBadge() {
  // if no patient id, do nothing
  if (!window.AppPatientId) return;

  try {
    const res = await fetch(`/Notifications/UnreadCount?patientId=${window.AppPatientId}`, { cache: 'no-store' });
    if (!res.ok) throw new Error('HTTP ' + res.status);
    const count = await res.json();

    // 1) dont show number, just a dot
    badge.textContent = ''; 
    // only show badge if count > 0
    badge.style.display = count > 0 ? '' : 'none';

    // 2) tooltip only if there are notifications
    if (count > 0) {
      const text = count === 1
        ? 'You have 1 new notification'
        : `You have ${count} new notifications`;
      // use title attribute for tooltip text
      btn.setAttribute('title', text);

      // this is to re-initialize tooltip with new text)
      if (window.bootstrap?.Tooltip) {
        const existing = window.bootstrap.Tooltip.getInstance(btn);
        if (existing) existing.dispose();
        new window.bootstrap.Tooltip(btn);
      }
    } else {
      // if no notifications, remove tooltip
      btn.removeAttribute('title');
      if (window.bootstrap?.Tooltip) {
        const existing = window.bootstrap.Tooltip.getInstance(btn);
        if (existing) existing.dispose();
      }
    }
  } catch {
    // if error, hide badge and tooltip
    badge.style.display = 'none';
    btn.removeAttribute('title');
    if (window.bootstrap?.Tooltip) {
      const existing = window.bootstrap.Tooltip.getInstance(btn);
      if (existing) existing.dispose();
    }
  }
}



  async function loadLatest() {
    if (!list) return;
    list.innerHTML = `<li class="text-muted">Laster…</li>`;
    try {
      const res = await fetch(`/Notifications/Latest?patientId=${pid}&take=10`, { cache: 'no-store' });
      if (!res.ok) throw new Error('HTTP ' + res.status);
      const items = await res.json();
      if (!Array.isArray(items) || items.length === 0) {
        list.innerHTML = `<li class="text-muted">Ingen varsler…</li>`;
        return;
      }
      list.innerHTML = items.map(n => `
        <li class="d-flex align-items-start gap-2 py-1">
          <i class="bi bi-dot fs-4 ${n.isRead ? 'text-secondary' : 'text-primary'}"></i>
          <div class="flex-grow-1">
            <div>${n.message}</div>
            <div class="text-muted">${fmt(n.createdAt)}</div>
          </div>
          ${n.isRead ? '' : `<button class="btn btn-sm btn-link mark-read" data-id="${n.notificationId}">Marker som lest</button>`}
        </li>
      `).join('');
    } catch {
      list.innerHTML = `<li class="text-danger">Klarte ikke å hente varsler</li>`;
    }
  }

  async function markRead(id) {
    const token = document.querySelector('meta[name="request-verification-token"]')?.content;
    const res = await fetch(`/Notifications/MarkRead?id=${id}`, {
      method: 'POST',
      headers: token ? { 'RequestVerificationToken': token } : {}
    });
    if (res.ok) { await refreshBadge(); await loadLatest(); }
  }

  btn.addEventListener('click', async () => {
  await loadLatest(); // keep your existing list loading

  // only show toast if there are real notifications
  try {
    if (!window.AppPatientId || !toastEl || !Toast) return;

    const r = await fetch(`/Notifications/UnreadCount?patientId=${window.AppPatientId}`, { cache: 'no-store' });
    if (!r.ok) return;
    const count = await r.json();

    if (count > 0) {
      const body = document.getElementById('notifToastBody');
      if (body) {
        body.textContent = count === 1
          ? 'You have 1 new notification.'
          : `You have ${count} new notifications.`;
      }
      toastEl.removeAttribute('hidden');
      new Toast(toastEl, { delay: 4000 }).show();
    }
    // else: do nothing, no toast shown
  } catch { /* ignore */ }
})});