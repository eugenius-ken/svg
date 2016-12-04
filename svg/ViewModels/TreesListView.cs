using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svgProject.ViewModels
{
    public class TreesListView
    {
        public IEnumerable<TreeThumbnailView> Trees { get; set; }
    }
}