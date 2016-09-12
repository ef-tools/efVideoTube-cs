$.fn.regNavigation = function (navigation) {
    var blockClass = 'block';
    var animationSpeed = 'fast';
    var timeout = 5000;
    var opacity = 0.5;

    var fadeOut = function () {
        navigation.fadeOut(animationSpeed, function () {
            navigation.removeClass(blockClass);
        });
    };
    var delayTask;
    $(this).click(function () {
        if (navigation.hasClass(blockClass))
            fadeOut();
        else {
            navigation.addClass(blockClass);
            navigation.fadeIn(animationSpeed);
            clearTimeout(delayTask);
            delayTask = setTimeout(fadeOut, timeout);
        }
    });
    navigation.mouseenter(function () {
        navigation.animate({ opacity: 1 }, animationSpeed);
        clearTimeout(delayTask);
    }).mouseleave(function () {
        navigation.animate({ opacity: opacity }, animationSpeed);
        delayTask = setTimeout(fadeOut, timeout);
    }).css('opacity', opacity);
};

var regRadioOptions = function (name, defaultValue, optionChanged) {
    var onOptionChanged = function (value) {
        if (optionChanged != null)
            optionChanged(value);
    };
    var value = getCookie(name);
    if (value === null)
        value = defaultValue;
    $('input[type="radio"].' + name).each(function () {
        if (value === null)
            value = $(this).val();
        if ($(this).val() == value) {
            $(this).prop('checked', true);
            onOptionChanged(value);
        }
    }).click(function () {
        onOptionChanged($(this).val());
        setCookie(name, $(this).val());
    });
};

var setCookie = function (cname, value) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + 360);
    
    var c = String.format('{0}={1}; expires={2}; path={3}; domain={4}', cname, escape(value), exdate.toGMTString(), escape(path), escape(domain));
    document.cookie = c;
};

var getCookie = function (cname) {
    var i, x, y, ARRcookies = document.cookie.split(';');
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf('='));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf('=') + 1);
        x = x.replace(/^\s+|\s+$/g, '');
        if (x == cname)
            return unescape(y);
    }
    return null;
};

var getUrlParameter = function (paramName) {
    return getParameter(paramName, window.location.search);
};

var getParameter = function (paramName, url) {
    var allParams = url.substr(url.indexOf('?') + 1).split('&');
    for (var i = 0; i < allParams.length; i++) {
        var paramPair = allParams[i].split('=');
        if (paramName == decodeURIComponent(paramPair[0])) {
            return decodeURIComponent(paramPair[1]).replace(/\+/g, ' ');
        }
    }
    return null;
};

String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }
    return s;
};
