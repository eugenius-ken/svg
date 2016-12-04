using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svgProject.AjaxModels
{
    public class AjaxData
    {
        public Guid imageId { get; set; }
        public Guid treeId { get; set; }
        public string imageText { get; set; }
    }
}