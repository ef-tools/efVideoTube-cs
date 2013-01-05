var setCookie = function (cname, value, domain, path, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    
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
    var allParams = document.location.search.substr(1).split('&');
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
