$(document).ready(function () {
    $('fieldset').each(function () {
        var firstRadio = $(this).find('input[type="radio"].player').get(0);
        var name = $(firstRadio).attr('name');
        var value = getCookie(name);
        if(value != null)
            $(this).find(String.format('input[value="{0}"].player', value)).prop('checked', true);
    });

    $('input[type="radio"].player').click(function () {
        setCookie($(this).attr('name'), $(this).val(), domain, path, 360);
    });
});
