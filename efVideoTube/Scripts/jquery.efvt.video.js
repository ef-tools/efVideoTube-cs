$(document).ready(function () {
    var path = getUrlParameter('path');
    var current = getCookie('current');
    if (path.toUpperCase() === getParameter('path', current).toUpperCase()) {
        var prev = getCookie('prev');
        var next = getCookie('next');
        var lineFormat = '<p>{0}:<br /><a href="{1}">{2}</a></p>';
        var innerHtml = '';
        if (prev)
            innerHtml += String.format(lineFormat, 'Prev', prev, getParameter('path', prev));
        if (next)
            innerHtml += String.format(lineFormat, 'Next', next, getParameter('path', next));
        if (innerHtml) {
            var navigation = $('div.navigation');
            navigation.html(innerHtml);
            $('video').click(function () {
                navigation.fadeIn('fast');
                setTimeout(function () {
                    navigation.fadeOut('fast');
                }, 5000);
            });
        }
    }
});
