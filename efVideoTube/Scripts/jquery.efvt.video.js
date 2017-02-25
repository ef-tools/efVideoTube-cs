$(document).ready(function () {
    var video = $('video');
    var getFileName = function (url) {
        var path = getParameter('path', url);
        return path.endsWith('\\') ? path.substring(0, path.length - 1) :
            path.substring(path.lastIndexOf('\\') + 1);
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
