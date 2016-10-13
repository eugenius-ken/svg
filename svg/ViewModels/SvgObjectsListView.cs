using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svg.ViewModels
{
    public class SvgObjectsListView
    {
        public IEnumerable<SvgObjectVew> Objects { get; set; } = new List<SvgObjectVew>();
    }
}