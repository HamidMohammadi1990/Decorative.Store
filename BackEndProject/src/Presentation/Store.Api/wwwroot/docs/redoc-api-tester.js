(function () {
  var TOKEN_KEY = 'edition-api-token';
  var SPEC_URL = '/swagger/v1/swagger.json';
  var spec = null;
  var operationMap = {};

  var HTTP_COLORS = {
    get: '#2F8132',
    post: '#186FAF',
    put: '#95507c',
    patch: '#bf581d',
    delete: '#cc3333',
    head: '#A23DAD',
    options: '#947014'
  };

  function getStorage(key, fallback) {
    try {
      return window.parent.localStorage.getItem(key) || fallback;
    } catch (e) {
      try {
        return localStorage.getItem(key) || fallback;
      } catch (e2) {
        return fallback;
      }
    }
  }

  function setStorage(key, value) {
    try {
      window.parent.localStorage.setItem(key, value);
    } catch (e) {
      localStorage.setItem(key, value);
    }
  }

  function removeStorage(key) {
    try {
      window.parent.localStorage.removeItem(key);
    } catch (e) {
      localStorage.removeItem(key);
    }
  }

  function loadSpec() {
    if (spec) {
      return Promise.resolve(spec);
    }
    return fetch(SPEC_URL)
      .then(function (response) {
        return response.json();
      })
      .then(function (data) {
        spec = data;
        buildOperationMap();
        return spec;
      });
  }

  function buildOperationMap() {
    operationMap = {};
    if (!spec || !spec.paths) {
      return;
    }
    Object.keys(spec.paths).forEach(function (path) {
      var pathItem = spec.paths[path];
      ['get', 'post', 'put', 'patch', 'delete', 'head', 'options'].forEach(function (method) {
        var operation = pathItem[method];
        if (operation && operation.operationId) {
          operationMap[operation.operationId] = {
            path: path,
            method: method,
            operation: operation
          };
        }
      });
    });
  }

  function resolveRef(ref) {
    if (!spec || !ref || ref.indexOf('#/') !== 0) {
      return null;
    }
    var parts = ref.slice(2).split('/');
    var node = spec;
    for (var i = 0; i < parts.length; i++) {
      node = node && node[parts[i]];
    }
    return node;
  }

  function schemaToSample(schema, depth) {
    depth = depth || 0;
    if (!schema || depth > 10) {
      return undefined;
    }
    if (schema.example !== undefined) {
      return schema.example;
    }
    if (schema.default !== undefined) {
      return schema.default;
    }
    if (schema.$ref) {
      return schemaToSample(resolveRef(schema.$ref), depth + 1);
    }
    if (schema.allOf) {
      var merged = {};
      schema.allOf.forEach(function (part) {
        var sample = schemaToSample(part, depth + 1);
        if (sample && typeof sample === 'object' && !Array.isArray(sample)) {
          Object.assign(merged, sample);
        }
      });
      return merged;
    }
    if (schema.oneOf || schema.anyOf) {
      var list = schema.oneOf || schema.anyOf;
      return list.length ? schemaToSample(list[0], depth + 1) : undefined;
    }
    if (schema.type === 'string' || (!schema.type && schema.enum)) {
      return schema.enum ? schema.enum[0] : '';
    }
    if (schema.type === 'integer' || schema.type === 'number') {
      return 0;
    }
    if (schema.type === 'boolean') {
      return false;
    }
    if (schema.type === 'array') {
      return [schemaToSample(schema.items, depth + 1)];
    }
    var obj = {};
    if (schema.properties) {
      Object.keys(schema.properties).forEach(function (key) {
        var required = (schema.required || []).indexOf(key) >= 0;
        if (required || depth < 2) {
          obj[key] = schemaToSample(schema.properties[key], depth + 1);
        }
      });
    }
    return obj;
  }

  function getRequestBodyExample(operation) {
    var requestBody = operation.requestBody;
    if (!requestBody || !requestBody.content) {
      return '';
    }
    var json = requestBody.content['application/json'];
    if (!json) {
      return '';
    }
    if (json.example !== undefined) {
      return JSON.stringify(json.example, null, 2);
    }
    if (json.examples) {
      var keys = Object.keys(json.examples);
      if (keys.length && json.examples[keys[0]].value !== undefined) {
        return JSON.stringify(json.examples[keys[0]].value, null, 2);
      }
    }
    if (json.schema) {
      var sample = schemaToSample(json.schema);
      if (sample !== undefined) {
        return JSON.stringify(sample, null, 2);
      }
    }
    return '{}';
  }

  function escapeHtml(value) {
    return String(value)
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;');
  }

  function extractOperationId(sectionId) {
    if (!sectionId) {
      return null;
    }
    if (sectionId.indexOf('operation/') === 0) {
      return sectionId.slice('operation/'.length);
    }
    if (sectionId.indexOf('operation-') === 0) {
      return sectionId.slice('operation-'.length);
    }
    return null;
  }

  function findMiddlePanel(operationElement) {
    if (!operationElement) {
      return null;
    }
    var row = operationElement.firstElementChild;
    if (row && row.firstElementChild) {
      return row.firstElementChild;
    }
    return operationElement;
  }

  function buildUrl(pathTemplate, pathParams, queryParams) {
    var url = pathTemplate;
    Object.keys(pathParams).forEach(function (key) {
      url = url.replace('{' + key + '}', encodeURIComponent(pathParams[key] || ''));
    });
    var search = new URLSearchParams();
    Object.keys(queryParams).forEach(function (key) {
      var value = queryParams[key];
      if (value !== '' && value !== undefined && value !== null) {
        search.append(key, value);
      }
    });
    var query = search.toString();
    return window.location.origin + url + (query ? '?' + query : '');
  }

  function collectParams(panel, paramIn) {
    var result = {};
    panel.querySelectorAll('[data-param-in="' + paramIn + '"]').forEach(function (input) {
      result[input.dataset.paramName] = input.value;
    });
    return result;
  }

  function updateUrlPreview(panel, meta) {
    var pathParams = collectParams(panel, 'path');
    var queryParams = collectParams(panel, 'query');
    var url = buildUrl(meta.path, pathParams, queryParams);
    var element = panel.querySelector('.edition-redoc-path');
    if (element) {
      element.textContent = url.replace(window.location.origin, '');
    }
    updateCurlPreview(panel, meta);
  }

  function shellQuote(value) {
    return "'" + String(value).replace(/'/g, "'\"'\"'") + "'";
  }

  function buildRequestConfig(panel, meta) {
    var pathParams = collectParams(panel, 'path');
    var queryParams = collectParams(panel, 'query');
    var url = buildUrl(meta.path, pathParams, queryParams);
    var headers = {
      'Accept-Language': getStorage('edition-api-language', 'fa-IR'),
      Accept: 'application/json'
    };
    var token = getStorage(TOKEN_KEY, '');
    if (token) {
      headers.Authorization = token.indexOf('Bearer ') === 0 ? token : 'Bearer ' + token;
    }
    var method = meta.method.toUpperCase();
    var body = null;
    var bodyEl = panel.querySelector('.edition-redoc-code-input');
    if (bodyEl && ['POST', 'PUT', 'PATCH'].indexOf(method) >= 0) {
      headers['Content-Type'] = 'application/json';
      var rawBody = bodyEl.value.trim();
      if (rawBody) {
        body = rawBody;
      }
    }
    return { method: method, url: url, headers: headers, body: body };
  }

  function buildCurl(config) {
    var lines = ['curl -X ' + config.method, '  ' + shellQuote(config.url)];
    Object.keys(config.headers).forEach(function (name) {
      lines.push('  -H ' + shellQuote(name + ': ' + config.headers[name]));
    });
    if (config.body) {
      lines.push('  -d ' + shellQuote(config.body));
    }
    return lines.join(' \\\n');
  }

  function updateCurlPreview(panel, meta) {
    var curlEl = panel.querySelector('.edition-redoc-curl');
    if (!curlEl) {
      return;
    }
    curlEl.textContent = buildCurl(buildRequestConfig(panel, meta));
  }

  function renderParamTable(title, parameters, paramIn) {
    if (!parameters.length) {
      return '';
    }

    var rows = parameters
      .map(function (parameter) {
        var example = parameter.example || (parameter.schema && parameter.schema.example) || '';
        var typeLabel = (parameter.schema && parameter.schema.type) || 'string';
        var required = parameter.required
          ? '<span class="edition-redoc-required">required</span>'
          : '<span class="edition-redoc-optional">optional</span>';

        return (
          '<tr>' +
          '<td class="edition-redoc-name">' +
          '<span class="property-name">' +
          escapeHtml(parameter.name) +
          '</span> ' +
          required +
          '<div class="edition-redoc-type">' +
          escapeHtml(typeLabel) +
          '</div>' +
          '</td>' +
          '<td class="edition-redoc-value">' +
          '<input type="text" class="edition-redoc-input" data-param-in="' +
          paramIn +
          '" data-param-name="' +
          escapeHtml(parameter.name) +
          '" value="' +
          escapeHtml(String(example)) +
          '" spellcheck="false" />' +
          '</td>' +
          '</tr>'
        );
      })
      .join('');

    return (
      '<h5 class="edition-redoc-subheader">' +
      escapeHtml(title) +
      '</h5>' +
      '<table class="edition-redoc-table"><tbody>' +
      rows +
      '</tbody></table>'
    );
  }

  function setExecuteLoading(sendBtn, loading) {
    var spinner = sendBtn.querySelector('.edition-redoc-spinner');
    var text = sendBtn.querySelector('.edition-redoc-submit-text');
    sendBtn.disabled = loading;
    sendBtn.classList.toggle('is-loading', loading);
    sendBtn.setAttribute('aria-busy', loading ? 'true' : 'false');
    if (spinner) {
      spinner.hidden = !loading;
    }
    if (text) {
      text.textContent = loading ? 'Loading...' : 'Execute';
    }
  }

  function showAlert(panel, message, type) {
    var alertEl = panel.querySelector('.edition-redoc-alert');
    if (!alertEl) {
      return;
    }
    if (!message) {
      alertEl.hidden = true;
      alertEl.textContent = '';
      alertEl.className = 'edition-redoc-alert';
      return;
    }
    alertEl.textContent = message;
    alertEl.className = 'edition-redoc-alert is-' + (type || 'error');
    alertEl.hidden = false;
  }

  function formatDuration(startTime) {
    var ms = Date.now() - startTime;
    if (ms < 1000) {
      return ms + ' ms';
    }
    return (ms / 1000).toFixed(2) + ' s';
  }

  function sendRequest(panel, meta) {
    var responseWrap = panel.querySelector('.edition-redoc-response');
    var responseCode = panel.querySelector('.edition-redoc-response-code-value');
    var responseDetails = panel.querySelector('.edition-redoc-response-details');
    var responseDuration = panel.querySelector('.edition-redoc-response-duration');
    var responseBody = panel.querySelector('.edition-redoc-response-code code');
    var sendBtn = panel.querySelector('.edition-redoc-submit');
    var startedAt = Date.now();

    showAlert(panel, null);

    var config = buildRequestConfig(panel, meta);
    var url = config.url;
    var headers = config.headers;
    var options = {
      method: config.method,
      headers: headers
    };

    if (config.body) {
      try {
        JSON.parse(config.body);
        options.body = config.body;
      } catch (e) {
        showAlert(panel, 'Request body contains invalid JSON.', 'error');
        return;
      }
    }

    updateCurlPreview(panel, meta);

    setExecuteLoading(sendBtn, true);
    responseWrap.hidden = true;

    fetch(url, options)
      .then(function (response) {
        return response.text().then(function (text) {
          var formatted = text;
          try {
            formatted = JSON.stringify(JSON.parse(text), null, 2);
          } catch (e) {}

          var isSuccess = response.ok;
          responseCode.textContent = String(response.status);
          responseCode.className =
            'edition-redoc-response-code-value ' + (isSuccess ? 'is-success' : 'is-error');
          responseDetails.textContent = response.statusText || (isSuccess ? 'OK' : 'Error');
          responseDuration.textContent = formatDuration(startedAt);
          responseBody.textContent = formatted || '(empty response)';
          responseWrap.hidden = false;
          responseWrap.classList.toggle('is-error', !isSuccess);
          responseWrap.classList.toggle('is-success', isSuccess);
        });
      })
      .catch(function (error) {
        responseCode.textContent = '—';
        responseCode.className = 'edition-redoc-response-code-value is-error';
        responseDetails.textContent = 'Network error';
        responseDuration.textContent = formatDuration(startedAt);
        responseBody.textContent = String(error);
        responseWrap.hidden = false;
        responseWrap.classList.add('is-error');
        responseWrap.classList.remove('is-success');
      })
      .finally(function () {
        setExecuteLoading(sendBtn, false);
      });
  }

  function createTesterPanel(operationId, meta) {
    var panel = document.createElement('div');
    panel.className = 'edition-api-tester';
    panel.dataset.operationId = operationId;

    var operation = meta.operation;
    var pathParams = [];
    var queryParams = [];
    (operation.parameters || []).forEach(function (parameter) {
      if (parameter.in === 'path') {
        pathParams.push(parameter);
      } else if (parameter.in === 'query') {
        queryParams.push(parameter);
      }
    });

    var hasBody = ['post', 'put', 'patch'].indexOf(meta.method) >= 0 && operation.requestBody;
    var needsAuth = operation.security && operation.security.length > 0;
    var verbColor = HTTP_COLORS[meta.method] || '#707070';

    var html =
      '<h5 class="edition-redoc-section">' +
      '<button type="button" class="edition-redoc-section-btn edition-api-tester-toggle" aria-expanded="false">' +
      '<span class="edition-redoc-caret" aria-hidden="true"></span>' +
      '<span>Try it out</span>' +
      '</button>' +
      '</h5>' +
      '<div class="edition-api-tester-panel" hidden>';

    html +=
      '<div class="edition-redoc-endpoint">' +
      '<span class="edition-redoc-verb" style="background-color:' +
      verbColor +
      '">' +
      meta.method +
      '</span>' +
      '<span class="edition-redoc-path"></span>' +
      '</div>';

    html += renderParamTable('پارامترهای مسیر', pathParams, 'path');
    html += renderParamTable('پارامترهای Query', queryParams, 'query');

    if (hasBody) {
      html +=
        '<h5 class="edition-redoc-subheader">Request body</h5>' +
        '<div class="edition-redoc-code-wrap">' +
        '<textarea class="edition-redoc-code-input" rows="12" spellcheck="false">' +
        escapeHtml(getRequestBodyExample(operation)) +
        '</textarea></div>';
    }

    if (needsAuth) {
      html +=
        '<p class="edition-redoc-note">نیاز به احراز هویت — توکن Bearer را در بخش Authorization بالای مستندات وارد کنید.</p>';
    }

    html +=
      '<h5 class="edition-redoc-subheader">Curl</h5>' +
      '<pre class="edition-redoc-curl-block"><code class="edition-redoc-curl"></code></pre>' +
      '<div class="edition-redoc-actions">' +
      '<button type="button" class="edition-redoc-submit" aria-busy="false">' +
      '<span class="edition-redoc-spinner" hidden aria-hidden="true"></span>' +
      '<span class="edition-redoc-submit-text">Execute</span>' +
      '</button>' +
      '</div>' +
      '<div class="edition-redoc-alert" role="alert" hidden></div>' +
      '<div class="edition-redoc-response" hidden>' +
      '<h5 class="edition-redoc-subheader">Responses</h5>' +
      '<div class="edition-redoc-response-meta">' +
      '<div class="edition-redoc-response-row">' +
      '<span class="edition-redoc-response-label">Code</span>' +
      '<span class="edition-redoc-response-code-value"></span>' +
      '</div>' +
      '<div class="edition-redoc-response-row">' +
      '<span class="edition-redoc-response-label">Details</span>' +
      '<span class="edition-redoc-response-details"></span>' +
      '</div>' +
      '<div class="edition-redoc-response-row">' +
      '<span class="edition-redoc-response-label">Duration</span>' +
      '<span class="edition-redoc-response-duration"></span>' +
      '</div>' +
      '</div>' +
      '<div class="edition-redoc-response-body-label">Response body</div>' +
      '<pre class="edition-redoc-response-code"><code></code></pre>' +
      '</div></div>';

    panel.innerHTML = html;

    var toggle = panel.querySelector('.edition-api-tester-toggle');
    var innerPanel = panel.querySelector('.edition-api-tester-panel');

    toggle.addEventListener('click', function (event) {
      event.preventDefault();
      event.stopPropagation();
      var open = innerPanel.hidden;
      innerPanel.hidden = !open;
      toggle.setAttribute('aria-expanded', open ? 'true' : 'false');
      toggle.classList.toggle('is-open', open);
      if (open) {
        updateUrlPreview(panel, meta);
      }
    });

    panel.querySelector('.edition-redoc-submit').addEventListener('click', function () {
      sendRequest(panel, meta);
    });

    panel.querySelectorAll('[data-param-in]').forEach(function (input) {
      input.addEventListener('input', function () {
        updateUrlPreview(panel, meta);
      });
    });

    var bodyInput = panel.querySelector('.edition-redoc-code-input');
    if (bodyInput) {
      bodyInput.addEventListener('input', function () {
        updateCurlPreview(panel, meta);
      });
    }

    updateUrlPreview(panel, meta);
    return panel;
  }

  function refreshAllCurlPreviews() {
    document.querySelectorAll('.edition-api-tester').forEach(function (panel) {
      var operationId = panel.dataset.operationId;
      if (operationId && operationMap[operationId]) {
        updateCurlPreview(panel, operationMap[operationId]);
      }
    });
  }

  function showAuthStatus(bar, message, type) {
    var status = bar.querySelector('.edition-auth-status');
    if (!status) {
      return;
    }
    status.textContent = message;
    status.className = 'edition-auth-status is-' + (type || 'success');
    status.hidden = false;
    if (bar.__authStatusTimer) {
      clearTimeout(bar.__authStatusTimer);
    }
    bar.__authStatusTimer = setTimeout(function () {
      status.hidden = true;
    }, 2200);
  }

  function injectGlobalAuthBar() {
    var apiContent = document.querySelector('.api-content');
    if (!apiContent || apiContent.querySelector('.edition-api-auth')) {
      return;
    }

    var bar = document.createElement('div');
    bar.className = 'edition-api-auth';
    bar.innerHTML =
      '<h5 class="edition-redoc-subheader">Authorization</h5>' +
      '<div class="edition-auth-card">' +
      '<div class="edition-auth-row">' +
      '<div class="edition-auth-scheme-wrap">' +
      '<span class="edition-auth-scheme">Bearer <span class="edition-auth-type">JWT</span></span>' +
      '</div>' +
      '<div class="edition-auth-input-wrap">' +
      '<input type="password" class="edition-auth-token" placeholder="Enter access token" autocomplete="off" spellcheck="false" />' +
      '</div>' +
      '<div class="edition-auth-buttons">' +
      '<button type="button" class="edition-redoc-submit edition-auth-save">Save</button>' +
      '<button type="button" class="edition-redoc-btn-secondary edition-auth-clear">Clear</button>' +
      '</div>' +
      '</div>' +
      '<span class="edition-auth-status" hidden aria-live="polite"></span>' +
      '<p class="edition-auth-hint">Saved token is used automatically for authenticated Try it out requests.</p>' +
      '</div>';

    var firstSection = apiContent.querySelector('h1, [data-section-id]');
    if (firstSection && firstSection.parentElement === apiContent) {
      apiContent.insertBefore(bar, firstSection.nextSibling);
    } else {
      apiContent.insertBefore(bar, apiContent.firstChild);
    }

    var input = bar.querySelector('.edition-auth-token');
    var saved = getStorage(TOKEN_KEY, '');
    if (saved) {
      input.value = saved.replace(/^Bearer\s+/i, '');
    }

    if (bar.dataset.editionAuthBound !== 'true') {
      bar.dataset.editionAuthBound = 'true';
      bar.addEventListener('click', function (event) {
        var saveBtn = event.target.closest('.edition-auth-save');
        var clearBtn = event.target.closest('.edition-auth-clear');
        var tokenInput = bar.querySelector('.edition-auth-token');

        if (saveBtn) {
          event.preventDefault();
          var value = tokenInput.value.trim();
          if (!value) {
            showAuthStatus(bar, 'Enter a token first', 'error');
            return;
          }
          setStorage(TOKEN_KEY, value);
          showAuthStatus(bar, 'Saved', 'success');
          refreshAllCurlPreviews();
          return;
        }

        if (clearBtn) {
          event.preventDefault();
          tokenInput.value = '';
          removeStorage(TOKEN_KEY);
          showAuthStatus(bar, 'Cleared', 'success');
          refreshAllCurlPreviews();
        }
      });
    }
  }

  function attachTester(operationElement) {
    if (!operationElement || operationElement.querySelector('.edition-api-tester')) {
      return;
    }

    var sectionId = operationElement.id || operationElement.dataset.sectionId;
    var operationId = extractOperationId(sectionId);
    if (!operationId || !operationMap[operationId]) {
      return;
    }

    var middlePanel = findMiddlePanel(operationElement);
    if (!middlePanel) {
      return;
    }

    middlePanel.appendChild(createTesterPanel(operationId, operationMap[operationId]));
  }

  function fixSearchInput() {
    var input = document.querySelector('.menu-content input.search-input');
    if (!input) {
      return;
    }

    var wrap = input.closest('.edition-search-wrap');
    if (!wrap) {
      wrap = document.createElement('div');
      wrap.className = 'edition-search-wrap';

      var parent = input.parentElement;
      if (!parent) {
        return;
      }

      Array.from(parent.children).forEach(function (child) {
        if (child !== input && child.tagName && child.tagName.toLowerCase() === 'svg') {
          child.remove();
        }
      });

      parent.insertBefore(wrap, input);
      wrap.appendChild(input);

      var icon = document.createElement('span');
      icon.className = 'edition-search-icon';
      icon.setAttribute('aria-hidden', 'true');
      wrap.insertBefore(icon, input);
    } else {
      wrap.querySelectorAll('svg').forEach(function (svg) {
        svg.remove();
      });
    }

    input.style.setProperty('background-image', 'none', 'important');
    input.style.setProperty('background', 'transparent', 'important');
    input.style.setProperty('margin', '0', 'important');
    input.style.setProperty('width', '100%', 'important');
    input.dataset.editionSearchFixed = 'true';
  }

  function scanOperations() {
    ['[id^="operation/"]', '[data-section-id^="operation/"]'].forEach(function (selector) {
      document.querySelectorAll(selector).forEach(attachTester);
    });
    injectGlobalAuthBar();
    fixSearchInput();
  }

  function start() {
    loadSpec()
      .then(scanOperations)
      .catch(function () {
        console.warn('Edition API tester: could not load OpenAPI spec.');
      });

    var timer = null;
    var observer = new MutationObserver(function () {
      if (timer) {
        clearTimeout(timer);
      }
      timer = setTimeout(scanOperations, 150);
    });

    observer.observe(document.documentElement, { childList: true, subtree: true });
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', start);
  } else {
    start();
  }
})();
