﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using PureLib.Common;

namespace efVideoTube.Models {
    public static class Global {
        public const string VideoCacheCategory = "efvtCache";
        public static Dictionary<string, string> CategoryPathMaps { get; private set; }

        static Global() {
            CategoryPathMaps = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                { VideoCacheCategory, ConfigurationManager.AppSettings["videoCacheDir"] },
            };
            foreach (string pair in ConfigurationManager.AppSettings["categories"].Split('|')) {
                string[] map = pair.Split(',');
                CategoryPathMaps.Add(map.First(), map.Last());
            }
        }

        public static string ToJson(this object obj) {
            return new JavaScriptSerializer().Serialize(obj);
        }

        public static object GetRouteValues(this string path) {
            return new { path = path };
        }

        public static object GetRouteValues(this string path, bool isAudioOnly) {
            return new { path = path, isAudioOnly = isAudioOnly };
        }

        public static string GetMediaUrl(this HttpRequestBase request, string path) {
            return new Uri(new Uri("{0}://{1}{2}/".FormatWith(request.Url.Scheme,
                request.Url.Authority, request.ApplicationPath.TrimEnd('/'))), Uri.EscapeDataString(path)).AbsolutePath;
        }

        public static void GetPhysicalPathAndCategory(string path, out string physicalPath, out string category) {
            string[] parts = path.Split(Path.DirectorySeparatorChar);
            category = parts.First();

            if (CategoryPathMaps.ContainsKey(category)) {
                string relativePath = Path.Combine(parts.Skip(1).ToArray());
                physicalPath = Path.Combine(CategoryPathMaps[category], relativePath);
            }
            else
                physicalPath = null;
        }

        public static Player GetPlayer(this HttpRequestBase request, string path) {
            string ext = Path.GetExtension(path);
            if (!Media.SupportedMedia.ContainsKey(ext))
                return Player.None;
            if (path.StartsWith(Global.VideoCacheCategory, StringComparison.OrdinalIgnoreCase))
                return Player.Html5Audio;

            HttpCookie cookie = request.Cookies[ext];
            Player player = Player.None;
            if ((cookie == null) || !Enum.TryParse(cookie.Value, out player) || !Media.SupportedMedia[ext].AvailablePlayers.Contains(player))
                player = Media.SupportedMedia[ext].Player;
            return player;
        }

        public static bool ShouldDisplay(this HttpRequestBase request, string path) {
            return request.GetPlayer(path) != Player.None;
        }

        public static string GetViewName(this Player player) {
            return "{0}Player".FormatWith(player);
        }

        public static MvcHtmlString RadioButtonWithLabel(this HtmlHelper htmlHelper, string name, string value, bool isChecked, object htmlAttributes = null, string displayName = null) {
            string id = name + value;
            IDictionary<string, object> dic = new RouteValueDictionary(htmlAttributes ?? new object());

            TagBuilder raidoBuilder = new TagBuilder("input");
            raidoBuilder.Attributes.Add("id", id);
            raidoBuilder.Attributes.Add("type", HtmlHelper.GetInputTypeString(InputType.Radio));
            raidoBuilder.Attributes.Add("name", name);
            raidoBuilder.Attributes.Add("value", value);
            if (isChecked)
                raidoBuilder.Attributes.Add("checked", "checked");
            raidoBuilder.MergeAttributes(dic);

            TagBuilder labelBuilder = new TagBuilder("label");
            labelBuilder.InnerHtml = HttpUtility.HtmlEncode(displayName ?? value);
            labelBuilder.Attributes.Add("for", id);
            labelBuilder.MergeAttributes(dic);

            return MvcHtmlString.Create(raidoBuilder.ToString(TagRenderMode.SelfClosing) + labelBuilder.ToString(TagRenderMode.Normal));
        }

        public static class ActionName {
            public const string Index = "Index";
            public const string Play = "Play";
            public const string Subtitle = "Subtitle";
            public const string Audio = "Audio";
            public const string Playlist = "Playlist";
        }
    }
}