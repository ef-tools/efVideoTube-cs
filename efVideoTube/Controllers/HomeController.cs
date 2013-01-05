using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using efVideoTube.Models;
using PureLib.Common;
using SrtLib;
using IOFile = System.IO.File;

namespace efVideoTube.Controllers {
    [Authorize, HandleError]
    public class HomeController : Controller {
        public ViewResult Index(string path) {
            if (path.IsNullOrEmpty())
                return View(new ListModel {
                    Folders = Global.CategoryPathMaps.Keys.ToArray(),
                    Files = new string[0]
                });

            string physicalPath;
            string category;
            GetPhysicalPathAndCategory(path, out physicalPath, out category);

            if (!physicalPath.IsNullOrEmpty()) {
                DirectoryInfo dir = new DirectoryInfo(physicalPath);
                if (dir.Exists)
                    return View(new ListModel {
                        Current = path,
                        Folders = dir.GetDirectories().Where(d => !d.Attributes.HasFlag(FileAttributes.Hidden))
                            .Select(d => GetPathForUrl(d.FullName, category)).ToArray(),
                        Files = dir.GetFiles().Where(f => Media.SupportedMedia.ContainsKey(Path.GetExtension(f.FullName)) && !f.Attributes.HasFlag(FileAttributes.Hidden))
                            .Select(f => GetPathForUrl(f.FullName, category)).ToArray()
                    });
            }

            return null;
        }

        public ViewResult Player(string path) {
            if (!path.IsNullOrEmpty()) {
                string physicalPath;
                string category;
                GetPhysicalPathAndCategory(path, out physicalPath, out category);

                if (!physicalPath.IsNullOrEmpty())
                    switch (Request.GetVideoPlayer(path)) {
                        case VideoPlayer.Html5:
                            string[] subs = Directory.GetFiles(Path.GetDirectoryName(physicalPath), "{0}.*".FormatWith(Path.GetFileNameWithoutExtension(physicalPath)))
                                .Where(s => Global.SupportedSubtitles.Contains(Path.GetExtension(s)))
                                .Select(s => Path.ChangeExtension(GetPathForUrl(s, category), Global.VttExt))
                                .Distinct().ToArray();
                            return View("Html5Player", new Html5VideoModel {
                                Title = Path.GetFileName(path),
                                Url = Request.GetMediaUrl(path),
                                SubtitleLanguages = subs.ToDictionary(s => s, s => SubtitleLanguageParser.Parse(s))
                            });
                        case VideoPlayer.Silverlight:
                            return View("SilverlightPlayer", new VideoModel {
                                Title = Path.GetFileName(path),
                                Url = Request.GetMediaUrl(path),
                            });
                        case VideoPlayer.Flash:
                            return View("FlashPlayer", new VideoModel {
                                Title = Path.GetFileName(path),
                                Url = Request.GetMediaUrl(path),
                            });
                    }
            }
            return null;
        }

        public FileContentResult Subtitle(string path) {
            if (!path.IsNullOrEmpty()) {
                string physicalPath;
                string category;
                GetPhysicalPathAndCategory(path, out physicalPath, out category);

                if (!physicalPath.IsNullOrEmpty()) {
                    string subContent = null;
                    if (IOFile.Exists(physicalPath))
                        subContent = physicalPath.ReadText(Encoding.UTF8);
                    else {
                        for (int i = 0; i < Global.SupportedSubtitles.Length; i++) {
                            physicalPath = Path.ChangeExtension(physicalPath, Global.SupportedSubtitles[i]);
                            if (IOFile.Exists(physicalPath))
                                subContent = physicalPath.ReadSubtitle().ToVtt();
                        }
                    }

                    if (!subContent.IsNullOrEmpty())
                        return File(Encoding.UTF8.GetBytes(subContent), "text/vtt");
                }
            }
            return null;
        }

        public ViewResult Settings() {
            return View();
        }

        private void GetPhysicalPathAndCategory(string path, out string physicalPath, out string category) {
            string[] parts = path.Split(Path.DirectorySeparatorChar);
            category = parts.First();

            if (Global.CategoryPathMaps.ContainsKey(category)) {
                string relativePath = Path.Combine(parts.Skip(1).ToArray());
                physicalPath = Path.Combine(Global.CategoryPathMaps[category], relativePath);
            }
            else
                physicalPath = null;
        }

        private string GetPathForUrl(string physicalPath, string category) {
            return "{1}{0}{2}".FormatWith(Path.DirectorySeparatorChar, category,
                physicalPath.Substring(Global.CategoryPathMaps[category].Length).Trim(Path.DirectorySeparatorChar));
        }
    }
}
