$(document).ready(function () {
    var list;
    var margin = 4;
    var scrollToPlayingItem = function (i) {
        window.location.hash = (i < margin) ? 0 : i - margin;
    };
    var play = function () {
        var link = playlist.find('a#' + index);
        link.addClass(playingClassName);
        document.title = link.text();
        audio.src = list[index].Url;
        audio.load();
        audio.play();
    };
    var goNext = function (e) {
        switch (getCookie('audioLoop')) {
            case 'single':
                play();
                break;
            case 'all':
                playlist.find('a#' + index).removeClass(playingClassName);
                if (getCookie('audioOrder') == 'normal')
                    index = (index + 1) % list.length;
                else
                    index = Math.floor(Math.random() * list.length);
                scrollToPlayingItem(index);
                play();
                break;
        }
    };

    regRadioOptions('audioOrder', null, null);
    regRadioOptions('audioLoop', null, null);

    var playlist = $('div.audioPlaylist');
    var index = -1;
    var lineFormat = '<p><a id="{0}" href="#">{1}</a></p>';

    $.ajax({
        type: "GET",
        url: "../Home/Playlist",
        traditional: true,
        dataType: "json",
        data: {
            path: getUrlParameter('path'),
            isAudio: true
        },
        success: function (response) {
            list = response;
        }
    });

    for (i in list) {
        if (list[i].Name == document.title)
            index = i;
        playlist.append(String.format(lineFormat, i, fileName));
    }
    scrollToPlayingItem(index);

    var playingClassName = 'playing';
    var audio = $('audio').get(0);
    audio.addEventListener('ended', goNext);
    $('a.next').click(function (e) {
        e.preventDefault();
        goNext(e);
    });
    playlist.find('a').click(function (e) {
        e.preventDefault();
        playlist.find('a#' + index).removeClass(playingClassName);
        index = $(this).parent().index();
        play();
    });
    playlist.find('a#' + index).addClass(playingClassName);
});
