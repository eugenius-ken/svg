using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace svg.Models
{
    public class TreeElement
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        public virtual Tree Tree { get; set; }
        public Guid TreeId { get; set; }

        public bool IsParent { get; set; }

        //[ForeignKey("Parent")]
        //public Guid ParentId { get; set; }
        //public virtual TreeElement Parent { get; set; }

        public virtual ICollection<TreeElement> Children { get; set; }
    }
}