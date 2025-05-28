document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.fdcp-page-header--has-bg[data-bg-src]').forEach(function (el) {
        var src = el.getAttribute('data-bg-src');
        if (src) {
            el.style.backgroundImage = 'url(' + src + ')';
        }
    });
});