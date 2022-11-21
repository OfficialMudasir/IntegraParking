using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.OxxoWS.WS.Data
{
    public class ZoneTreeViewItem : TreeViewItemModel
    {
        public int NumDesc;
    }

    public class TreeViewItemModel
    {
        

        public bool Checked { get; set; }
        public bool Enabled { get; set; }
        public bool Encoded { get; set; }
        public bool Expanded { get; set; }
        public bool HasChildren { get; set; }
        public IDictionary<string, string> HtmlAttributes { get; set; }
        public string Id { get; set; }
        public IDictionary<string, string> ImageHtmlAttributes { get; set; }
        public string ImageUrl { get; set; }
        public List<TreeViewItemModel> Items { get; set; }
        public IDictionary<string, string> LinkHtmlAttributes { get; set; }
        public bool Selected { get; set; }
        public string SpriteCssClass { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
    }
}