using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svgProject.ViewModels
{
    public class SvgObjectFullView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public SvgObjectFullView(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}