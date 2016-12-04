using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svgProject.ViewModels
{
    public class TreeThumbnailView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ThumbnailId { get; set; }
    }
}