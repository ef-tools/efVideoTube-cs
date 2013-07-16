$(document).ready(function () {
    var play = function () {
        var link = playlist.find('a#' + index);
        link.addClass(playingClassName);
        document.title = link.text();
        audio.src = list[index];
        audio.load();
        audio.play();
    };

    regRadioOptions('audioOrder', null, null);
    regRadioOptions('audioLoop', null, null);

    var playlist = $('div.audioPlaylist');
    var index = -1;
    var lineFormat = '<p><a id="{0}" href="#">{1}</a></p>';
    for (i in list) {
        var path = list[i];
        var fileName = decodeURIComponent(path.substring(path.lastIndexOf('/') + 1, path.lastIndexOf('.')));
        if (fileName == document.title)
            index = i;
        playlist.append(String.format(lineFormat, i, fileName));
    }

    var playingClassName = 'playing';
    var audio = $('audio').get(0);
    audio.addEventListener('ended', function (e) {
        switch (getCookie('audioLoop')) {
            case 'single':
                play();
                break;
            case 'all':
                playlist.find('a#' + index).removeClass(playingClassName);
                if (getCookie('audioOrder') == 'normal')
                    index++;
                else
                    index = Math.floor(Math.random() * list.length);
                window.location.hash = index;
                play();
                break;
        }
    });
    playlist.find('a').click(function (e) {
        e.preventDefault();
        playlist.find('a#' + index).removeClass(playingClassName);
        index = $(this).parent().index();
        play();
    });
    playlist.find('a#' + index).addClass(playingClassName);
});
