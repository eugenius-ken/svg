using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace svg.Models
{
    public class SvgObject
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public string Value { get; set; }

        public virtual Thumbnail Thumbnail { get; set; }
    }
}