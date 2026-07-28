(function () {
  if (window.self !== window.top) {
    return;
  }

  function mountHeader() {
    if (document.getElementById('edition-docs-header') || !document.body) {
      return;
    }

    var header = document.createElement('header');
    header.id = 'edition-docs-header';
    header.innerHTML = [
      '<a class="edition-docs-brand" href="/">Edition API</a>',
      '<div class="edition-header-end">',
      window.EditionLocale.createSelectHtml(),
      '<nav class="edition-docs-tabs" aria-label="Documentation views">',
      '<a class="edition-docs-tab" href="/docs">Docs</a>',
      '</nav>',
      '</div>'
    ].join('');

    document.body.insertBefore(header, document.body.firstChild);
    document.documentElement.classList.add('edition-docs-with-header');
    window.EditionLocale.initHeader(header);
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', mountHeader);
  } else {
    mountHeader();
  }
})();
