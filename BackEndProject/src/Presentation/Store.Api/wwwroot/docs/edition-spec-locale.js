(function () {
  'use strict';

  function getLanguage() {
    try {
      return window.localStorage.getItem('edition-api-language') || 'fa-IR';
    } catch {
      return 'fa-IR';
    }
  }

  if (typeof window.fetch !== 'function') {
    return;
  }

  var originalFetch = window.fetch.bind(window);

  window.fetch = function (input, init) {
    var url = typeof input === 'string' ? input : input && input.url;
    if (url && url.indexOf('/swagger/') !== -1 && url.indexOf('swagger.json') !== -1) {
      init = init || {};
      var headers = new Headers(init.headers || {});
      if (!headers.has('Accept-Language')) {
        headers.set('Accept-Language', getLanguage());
      }
      init.headers = headers;
    }

    return originalFetch(input, init);
  };
})();
