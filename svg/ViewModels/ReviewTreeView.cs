using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svgProject.ViewModels
{
    public class ReviewTreeView
    {
        public string Name { get; set; }
        public Guid TreeId { get; set; }
        public Guid ParentId { get; set; }
        public string ImageText { get; set; }
    }
}