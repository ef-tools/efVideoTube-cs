$(document).ready(function () {

});

var getUrlParameter = function (paramName) {
    var allParams = document.location.search.substr(1).split('&');
    for (var i = 0; i < allParams.length; i++) {
        var paramPair = allParams[i].split('=');
        if (paramName == decodeURIComponent(paramPair[0])) {
            return decodeURIComponent(paramPair[1]).replace(/\+/g, ' ');
        }
    }
    return '';
};

String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }
    return s;
};
