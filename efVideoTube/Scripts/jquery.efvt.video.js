$(document).ready(function () {
    var video = $('video');
    var setVideoClass = function (videoSize) {
        if (videoSize != null && !video.hasClass(videoSize))
            video.removeClass().addClass(videoSize);
    };
    regRadioOptions('videoSize', null, setVideoClass);

    var lineFormat = '<p>{0}:<br /><a href="{1}">{2}</a></p>';
    var innerHtml = String.format(lineFormat, 'Back', parent, getFileName(parent));
    if (prev)
        innerHtml += String.format(lineFormat, 'Previous', prev, getFileName(prev));
    if (next)
        innerHtml += String.format(lineFormat, 'Next', next, getFileName(next));
    var navigation = $('div.navigation');
    navigation.append(innerHtml);

    navigation.regNavigation('div.playerContainer');
});

var getFileName = function (url) {
    var path = getParameter('path', url);
    return path.substring(path.lastIndexOf('\\') + 1);
};
