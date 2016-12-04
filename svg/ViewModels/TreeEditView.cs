using svg.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace svgProject.ViewModels
{
    public class TreeEditView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        [DisplayName("Choose image for redirect")]
        public Guid ObjectId { get; set; }
        public IEnumerable<SvgObjectView> SvgObjects { get; set; }

        public string CurrentId { get; set; }

        public bool IsMainImageExist { get; set; }
    }
}