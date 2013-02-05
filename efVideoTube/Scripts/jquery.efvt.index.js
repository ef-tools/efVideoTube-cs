$(document).ready(function () {
    var getHref = function (a) {
        return a.attr('href');
    };

    $('a.item').click(function () {
        var currentRow = $(this).parent().parent();
        setCookie('current', getHref($(this)));
        var prev = currentRow.prev().find('a.item');
        setCookie('prev', prev.length ? getHref(prev) : '');
        var next = currentRow.next().find('a.item');
        setCookie('next', next.length ? getHref(next) : '');
    });
});
