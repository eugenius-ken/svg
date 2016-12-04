using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace svg.Models
{
    public class Thumbnail
    {
        public Guid Id { get; set; }
        public byte[] Content { get; set; }
    }
}