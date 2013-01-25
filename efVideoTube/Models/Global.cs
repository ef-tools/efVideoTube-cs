﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PureLib.Common;

namespace efVideoTube.Models {
    public static class Global {
        public const string VttExt = ".vtt";
        public static string[] SupportedSubtitles { get; private set; }
        public static Dictionary<string, string> CategoryPathMaps { get; private set; }

        static Global() {
            SupportedSubtitles = new string[] { ".srt", ".ass", ".ssa" };

            CategoryPathMaps = new Dictionary<string, string>();
            foreach (string pair in ConfigurationManager.AppSettings["categories"].Split('|')) {
                string[] map = pair.Split(',');
                CategoryPathMaps.Add(map.First(), map.Last());
            }
        }

        public static object GetRouteValues(this string path) {
            return new { path = path };
        }

        public static string GetMediaUrl(this HttpRequestBase request, string path) {
            return new Uri(new Uri("{0}://{1}{2}/".FormatWith(request.Url.Scheme,
                request.Url.Authority, request.ApplicationPath.TrimEnd('/'))), path).LocalPath;
        }

        public static Player GetVideoPlayer(this HttpRequestBase request, string path) {
            string ext = Path.GetExtension(path);
            HttpCookie cookie = request.Cookies[ext];
            Player player;
            if ((cookie == null) || !Enum.TryParse(cookie.Value, out player)) {
                if (Media.SupportedMedia.ContainsKey(ext))
                    player = Media.SupportedMedia[ext].Player;
                else
                    player = Player.None;
            }
            return player;
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
        }
    }
}