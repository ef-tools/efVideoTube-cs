$(document).ready(function () {
    var video = $('video');
    var getFileName = function (url) {
        var path = getParameter('path', url);
        var index = path.lastIndexOf('\\');
        if (index < path.length - 1)
            index++;
        return path.substring(index);
    };

    var lineFormat = '<p>{0}:<br /><a href="{1}">{2}</a></p>';
    var innerHtml = String.format(lineFormat, 'Back', parent, getFileName(parent));
    if (prev)
        innerHtml += String.format(lineFormat, 'Previous', prev, getFileName(prev));
    if (next)
        innerHtml += String.format(lineFormat, 'Next', next, getFileName(next));
    var navigation = $('div.navigation');
    navigation.append(innerHtml);

    $('div.playerContainer').regNavigation(navigation);
    regRadioOptions('videoSize', null, function (videoSize) {
        if (videoSize != null && !video.hasClass(videoSize))
            video.removeClass().addClass(videoSize);
    });
});
