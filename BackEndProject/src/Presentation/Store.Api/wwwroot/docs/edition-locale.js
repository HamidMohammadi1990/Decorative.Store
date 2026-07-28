window.EditionLocale = (function () {
  var STORAGE_KEY = 'edition-api-language';
  var DEFAULT = 'fa-IR';

  function get() {
    try {
      return localStorage.getItem(STORAGE_KEY) || DEFAULT;
    } catch {
      return DEFAULT;
    }
  }

  function set(value) {
    try {
      localStorage.setItem(STORAGE_KEY, value);
    } catch {
      /* ignore */
    }

    window.dispatchEvent(new CustomEvent('edition-locale-change', { detail: value }));
  }

  function createSelectHtml() {
    var current = get();
    return [
      '<label class="edition-locale" for="edition-locale-select">',
      '<span class="edition-locale-field">',
      '<span class="edition-locale-icon" aria-hidden="true">',
      '<svg viewBox="0 0 24 24" width="15" height="15" fill="none" stroke="currentColor" stroke-width="2">',
      '<circle cx="12" cy="12" r="10"></circle>',
      '<path d="M2 12h20M12 2a15.3 15.3 0 0 1 4 10 15.3 15.3 0 0 1-4 10 15.3 15.3 0 0 1-4-10 15.3 15.3 0 0 1 4-10z"></path>',
      '</svg>',
      '</span>',
      '<select id="edition-locale-select" class="edition-locale-select" aria-label="Language">',
      '<option value="fa-IR"' + (current === 'fa-IR' ? ' selected' : '') + '>فارسی</option>',
      '<option value="en-US"' + (current === 'en-US' ? ' selected' : '') + '>English</option>',
      '</select>',
      '</span>',
      '</label>'
    ].join('');
  }

  function bindSelect(root) {
    var scope = root || document;
    var select = scope.querySelector('#edition-locale-select');
    if (!select) {
      return;
    }

    select.value = get();
    select.addEventListener('change', function () {
      set(select.value);
    });
  }

  function applyToRequest(request) {
    if (!request.headers) {
      request.headers = {};
    }

    request.headers['Accept-Language'] = get();
    return request;
  }

  function initHeader(root) {
    bindSelect(root);

    window.addEventListener('storage', function (event) {
      if (event.key !== STORAGE_KEY) {
        return;
      }

      var select = document.querySelector('#edition-locale-select');
      if (select && event.newValue) {
        select.value = event.newValue;
      }
    });
  }

  return {
    get: get,
    set: set,
    createSelectHtml: createSelectHtml,
    initHeader: initHeader,
    applyToRequest: applyToRequest,
    STORAGE_KEY: STORAGE_KEY
  };
})();
