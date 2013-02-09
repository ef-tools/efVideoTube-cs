$(document).ready(function () {
    var blockClass = 'block';
    var lineFormat = '<p>{0}:<br /><a href="{1}">{2}</a></p>';
    var innerHtml = '';
    if (prev)
        innerHtml += String.format(lineFormat, 'Previous', prev, getFileName(prev));
    if (next)
        innerHtml += String.format(lineFormat, 'Next', next, getFileName(next));
    if (innerHtml) {
        var navigation = $('div.navigation');
        navigation.html(innerHtml);
        var fadeOut = function () {
            navigation.fadeOut('fast', function () {
                navigation.removeClass(blockClass);
            });
        };
        var delayTask;
        $('div.videoContainer').click(function () {
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

var getFileName = function (url) {
    var path = getParameter('path', url);
    return path.substring(path.lastIndexOf('\\') + 1);
};