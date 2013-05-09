$(document).ready(function () {
    var videoSize = getCookie('videoSize');
    if (videoSize)
        $('input[type="radio"].videoSize').each(function () {
            if (this.id == videoSize)
                $(this).prop('checked', true);
        });
    $('fieldset.videoType').each(function () {
        var firstRadio = $(this).find('input[type="radio"].player').get(0);
        var name = $(firstRadio).attr('name');
        var value = getCookie(name);
        if(value != null)
            $(this).find(String.format('input[value="{0}"].player', value)).prop('checked', true);
    });

    $('input[type="radio"].player').click(function () {
        setCookie($(this).attr('name'), $(this).val());
    });

    $('input[type="radio"].videoSize').click(function () {
        setCookie('videoSize', this.id);
    });
});
