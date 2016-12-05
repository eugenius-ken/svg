using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svg.ViewModels
{
    public class SvgObjectsListView
    {
        public IEnumerable<SvgObjectView> Objects { get; set; } = new List<SvgObjectView>();
        public string Keyword { get; set; }
    }
}