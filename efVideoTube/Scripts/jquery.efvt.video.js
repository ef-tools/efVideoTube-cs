$(document).ready(function () {
    var blockClass = 'block';
    var lineFormat = '<p>{0}:<br /><a href="{1}">{2}</a></p>';
    var innerHtml = '';
    if (prev)
        innerHtml += String.format(lineFormat, 'Previous', prev, getParameter('path', prev));
    if (next)
        innerHtml += String.format(lineFormat, 'Next', next, getParameter('path', next));
    if (innerHtml) {
        var navigation = $('div.navigation');
        navigation.html(innerHtml);
        var fadeOut = function () {
            navigation.fadeOut('fast', function () {
                navigation.removeClass(blockClass);
            });
        };
        var delayTask;
        $('video').click(function () {
            if (navigation.hasClass(blockClass))
                fadeOut();
            else {
                navigation.addClass(blockClass);
                navigation.fadeIn('fast');
                clearTimeout(delayTask);
                delayTask = setTimeout(fadeOut, 5000);
            }
        });
    }
});
