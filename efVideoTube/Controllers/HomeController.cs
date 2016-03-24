using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using efVideoTube.Models;
using PureLib.Common;

namespace efVideoTube.Controllers {
    [Authorize]
    public class HomeController : Controller {
        private const string playerMasterPageName = "_PlayerLayout";

        public ViewResult Index(string path) {
            if (path.IsNullOrEmpty())
                return View(new ListModel {
                    Folders = Global.CategoryPathMaps.Keys.Where(
                        n => !n.Equals(Global.VideoCacheCategory, StringComparison.OrdinalIgnoreCase)).ToArray(),
                    Files = new FileModel[0]
                });

            string physicalPath;
            string category;
            Global.GetPhysicalPathAndCategory(path, out physicalPath, out category);

            if (!physicalPath.IsNullOrEmpty()) {
                DirectoryInfo dir = new DirectoryInfo(physicalPath);
                if (dir.Exists)
                    return View(new ListModel {
                        Current = path,
                        Folders = GetFolders(dir, category),
                        Files = GetFiles(dir, category),
                    });
            }

            return null;
        }

        public ActionResult Play(string path, bool isAudioOnly = false) {
            if (!path.IsNullOrEmpty()) {
                string physicalPath;
                string category;
                Global.GetPhysicalPathAndCategory(path, out physicalPath, out category);

                if (!physicalPath.IsNullOrEmpty()) {
                    if (isAudioOnly && !AudioExtractor.Extract(ref path, physicalPath))
                        return null;

                    Player player = Request.GetPlayer(path);
                    string parent = Path.GetDirectoryName(physicalPath);
                    switch (player) {
                        case Player.Html5Video:
                            string[] subs = Directory.EnumerateFiles(parent, "{0}.*".FormatWith(Path.GetFileNameWithoutExtension(physicalPath)))
                                .Where(s => SubtitleConverter.CanExtract(s))
                                .Select(s => GetPathForUrl(s, category)).ToArray();
                            Html5VideoModel model = new Html5VideoModel {
                                Title = Path.GetFileNameWithoutExtension(path),
                                Url = Request.GetMediaUrl(path),
                                SubtitleLanguages = subs.ToSubtitleLanguages(),
                                Parent = Url.Action(Global.ActionName.Index, GetPathForUrl(parent, category).GetRouteValues()),
                            };
                            FileModel[] files = GetFiles(new DirectoryInfo(parent), category);
                            int index = Array.FindIndex(files, f => f.PathForUrl.Equals(path, StringComparison.OrdinalIgnoreCase));
                            if (index > 0)
                                model.Previous = Url.Action(Global.ActionName.Play, files[index - 1].PathForUrl.GetRouteValues());
                            if (index < (files.Length - 1))
                                model.Next = Url.Action(Global.ActionName.Play, files[index + 1].PathForUrl.GetRouteValues());
                            return View(player.GetViewName(), playerMasterPageName, model);
                        case Player.Html5Audio:
                            return View(player.GetViewName(), playerMasterPageName, new Html5AudioModel {
                                Title = Path.GetFileNameWithoutExtension(path),
                                Url = Request.GetMediaUrl(path),
                                Parent = Url.Action(Global.ActionName.Index, GetPathForUrl(parent, category).GetRouteValues()),
                            });
                        case Player.Silverlight:
                        case Player.Flash:
                            return View(player.GetViewName(), playerMasterPageName, new MediaModel {
                                Title = Path.GetFileNameWithoutExtension(path),
                                Url = Request.GetMediaUrl(path),
                            });
                        case Player.None:
                            return Redirect(Request.GetMediaUrl(path));
                    }
                }
            }
            return HttpNotFound();
        }

        public ActionResult Subtitle(string path) {
            if (!path.IsNullOrEmpty()) {
                string physicalPath;
                string category;
                Global.GetPhysicalPathAndCategory(path, out physicalPath, out category);

                if (SubtitleConverter.Extract(ref path, physicalPath))
                    return Redirect(Request.GetMediaUrl(path));
            }
            return HttpNotFound();
        }

        public ActionResult Audio(string path) {
            if (!path.IsNullOrEmpty()) {
                string physicalPath;
                string category;
                Global.GetPhysicalPathAndCategory(path, out physicalPath, out category);

                if (AudioExtractor.Extract(ref path, physicalPath))
                    return Redirect(Request.GetMediaUrl(path));
            }
            return HttpNotFound();
        }

        public ActionResult Playlist(string path) {
            if (!path.IsNullOrEmpty()) {
                string physicalPath;
                string category;
                Global.GetPhysicalPathAndCategory(path, out physicalPath, out category);

                string parent = Path.GetDirectoryName(physicalPath);
                return Json(from m in GetFiles(new DirectoryInfo(parent), category)
                            let canExtract = AudioExtractor.CanExtract(m.PathForUrl)
                            where canExtract || (Request.GetPlayer(m.PathForUrl) == Player.Html5Audio)
                            select new {
                                Name = Path.GetFileNameWithoutExtension(m.PathForUrl),
                                Url = canExtract ?
                                    Url.Action(Global.ActionName.Audio, m.PathForUrl.GetRouteValues()) :
                                    Request.GetMediaUrl(m.PathForUrl)
                            });
            }
            return HttpNotFound();
        }

        public ViewResult Settings() {
            return View();
        }

        private string[] GetFolders(DirectoryInfo dir, string category) {
            return dir.EnumerateDirectories().Where(d => !d.Attributes.HasFlag(FileAttributes.Hidden))
                .OrderBy(d => d.Name).Select(d => GetPathForUrl(d.FullName, category)).ToArray();
        }

        private FileModel[] GetFiles(DirectoryInfo dir, string category) {
            return dir.EnumerateFiles().Where(f => Request.ShouldDisplay(f.Name) && !f.Attributes.HasFlag(FileAttributes.Hidden))
                .OrderBy(f => f.Name).Select(f => new FileModel() { PathForUrl = GetPathForUrl(f.FullName, category), Size = f.Length }).ToArray();
        }

        private string GetPathForUrl(string physicalPath, string category) {
            return "{1}{0}{2}".FormatWith(Path.DirectorySeparatorChar, category,
                physicalPath.Substring(Global.CategoryPathMaps[category].Length).Trim(Path.DirectorySeparatorChar));
        }
    }
}
