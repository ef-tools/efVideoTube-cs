# efVideoTube (C#)

[![Build status](https://ci.appveyor.com/api/projects/status/75sknabkg5t3yo3m?svg=true)](https://ci.appveyor.com/project/eriforce/efvideotube-cs)
![License MIT](https://img.shields.io/badge/license-MIT-blue.svg)

efVideoTube (C#) is a website for listing and playing videos and music on the disk.


## Features

- Contains a couple of built-in players for playing different types of media files.
- Supports playing a playlist for audio files in sequence, shuffle or repeat mode.
- Supports playing audio only of specific types of videos. (requires external applications)
- Supports multiple tracks of captions.
- Converts subtitles files to webvtt files which are supported by html5. (requires external applications)


## Supported Media Types

- Video
  - `.mp4`
  - `.webm`
  - `.wmv`
  - `.flv`
- Audio
  - `.m4a`
  - `.mp3`
- Subtitle
  - `.ass`
  - `.ssa`
  - `.srt`
  - `.vtt`


## External Dependencies

The website depends on following external applications to make specific function work.
The pathes of these applications could be configured under `web.config`.

- `mp4box`

    For extracting audio from mp4 videos.
    https://gpac.wp.mines-telecom.fr/mp4box/

- `mkvtoolnix`

    For extracting audio from webm videos.
    https://mkvtoolnix.download/

- `ass2srt`

    For converting `.ass`, `.ssa` and `.srt` subtitles to webvtt.
    *Will be published to github soon.*


## Deployment

All configurations could be found in `appSettings` section of `web.config`.

You have to add media folders as Virtual Directories in the site under IIS to make media hosting work, these media folders should also be set in `categories` setting.
You also have to create another Virtual Directory named `efvtCache` for hosting extracted audio and converted subtitles, the path of this directory should also be set in `videoCacheDir` setting.
You need to remove `issuer` setting if you don't use client certificate to authenticate users.
At last, you may need to set a few MIME types for media and subtitles based on which version of IIS you are using.

File    | MIME
------- | --------------------
.mp4    | `video/mp4`
.webm   | `video/webm`
.wmv    | `video/x-ms-wmv`
.flv    | `video/x-flv`
.m4a    | `audio/mp4`
.weba   | `audio/webm`
.mp3    | `audio/mpeg`
.vtt    | `text/vtt`


## License

MIT