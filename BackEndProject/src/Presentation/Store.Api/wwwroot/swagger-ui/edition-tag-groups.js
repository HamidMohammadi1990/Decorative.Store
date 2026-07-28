(function () {
  'use strict';

  function getSpec(ui) {
    try {
      var state = ui.getSystem().getState();
      var json = state && state.getIn(['spec', 'json']);
      if (json && json.toJS) {
        return json.toJS();
      }
    } catch {
      /* ignore */
    }

    return null;
  }

  function applyTagGroups() {
    if (document.documentElement.dataset.editionTagGroupsApplied === 'true') {
      return true;
    }

    var ui = window.ui;
    if (!ui || typeof ui.getSystem !== 'function') {
      return false;
    }

    var spec = getSpec(ui);
    var tagGroups = spec && spec['x-tagGroups'];
    if (!Array.isArray(tagGroups) || tagGroups.length === 0) {
      return false;
    }

    var sections = Array.prototype.slice.call(
      document.querySelectorAll('.swagger-ui .opblock-tag-section[data-tag]')
    );
    if (sections.length === 0) {
      return false;
    }

    var sectionByTag = {};
    sections.forEach(function (section) {
      var tag = section.getAttribute('data-tag');
      if (tag) {
        sectionByTag[tag] = section;
      }
    });

    var firstSection = sections[0];
    var parent = firstSection.parentNode;
    if (!parent) {
      return false;
    }

    var anchor = firstSection;
    tagGroups.forEach(function (group) {
      if (!group || !Array.isArray(group.tags) || group.tags.length === 0) {
        return;
      }

      var groupEl = document.createElement('div');
      groupEl.className = 'edition-swagger-tag-group';

      var heading = document.createElement('button');
      heading.type = 'button';
      heading.className = 'edition-swagger-tag-group-title';
      heading.setAttribute('aria-expanded', 'true');
      heading.textContent = group.name || 'Group';
      groupEl.appendChild(heading);

      var body = document.createElement('div');
      body.className = 'edition-swagger-tag-group-body';
      groupEl.appendChild(body);

      group.tags.forEach(function (tagName) {
        var section = sectionByTag[tagName];
        if (section) {
          body.appendChild(section);
          delete sectionByTag[tagName];
        }
      });

      if (body.children.length === 0) {
        return;
      }

      heading.addEventListener('click', function () {
        groupEl.classList.toggle('is-open');
        var isOpen = groupEl.classList.contains('is-open');
        heading.setAttribute('aria-expanded', isOpen ? 'true' : 'false');
      });

      groupEl.classList.add('is-open');
      parent.insertBefore(groupEl, anchor);
    });

    Object.keys(sectionByTag).forEach(function (tagName) {
      var section = sectionByTag[tagName];
      if (section && section.parentNode === parent) {
        parent.removeChild(section);
      }
    });

    document.documentElement.dataset.editionTagGroupsApplied = 'true';
    return true;
  }

  function watch() {
    var attempts = 0;
    var timer = window.setInterval(function () {
      if (applyTagGroups() || ++attempts > 80) {
        window.clearInterval(timer);
      }
    }, 150);
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', watch);
  } else {
    watch();
  }
})();
