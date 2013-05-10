$(document).ready(function () {
    var video = $('video');
    var setVideoClass = function (videoSize) {
        if (videoSize != null && !video.hasClass(videoSize))
            video.removeClass().addClass(videoSize);
    };
    setVideoClass(getCookie('videoSize'));
    $('input[type="radio"].videoSize').click(function () {
        setVideoClass($(this).val());
        setCookie('videoSize', $(this).val());
    }).each(function () {
        if ($(this).val() == video.get(0).className)
            $(this).prop('checked', true);
    });

    var lineFormat = '<p>{0}:<br /><a href="{1}">{2}</a></p>';
    var innerHtml = String.format(lineFormat, 'Back', parent, getFileName(parent));
    if (prev)
        innerHtml += String.format(lineFormat, 'Previous', prev, getFileName(prev));
    if (next)
        innerHtml += String.format(lineFormat, 'Next', next, getFileName(next));
    var navigation = $('div.navigation');
    navigation.append(innerHtml);

    var blockClass = 'block';
    var animationSpeed = 'fast';
    var timeout = 5000;
    var fadeOut = function () {
        navigation.fadeOut(animationSpeed, function () {
            navigation.removeClass(blockClass);
        });
    };
    var delayTask;
    $('div.playerContainer').click(function () {
        if (navigation.hasClass(blockClass))
            fadeOut();
        else {
            navigation.addClass(blockClass);
            navigation.fadeIn(animationSpeed);
            clearTimeout(delayTask);
            delayTask = setTimeout(fadeOut, timeout);
        }
    });

    var opacity = 0.5;
    navigation.mouseenter(function () {
        navigation.animate({ opacity: 1 }, animationSpeed);
        clearTimeout(delayTask);
    }).mouseleave(function () {
        navigation.animate({ opacity: opacity }, animationSpeed);
        delayTask = setTimeout(fadeOut, timeout);
    }).css('opacity', opacity);
});

var getFileName = function (url) {
    var path = getParameter('path', url);
    return path.substring(path.lastIndexOf('\\') + 1);
};