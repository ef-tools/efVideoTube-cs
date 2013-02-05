$(document).ready(function () {
    var lineFormat = '<p>{0}:<br /><a href="{1}">{2}</a></p>';
    var innerHtml = '';
    if (prev)
        innerHtml += String.format(lineFormat, 'Previous', prev, getParameter('path', prev));
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
});
