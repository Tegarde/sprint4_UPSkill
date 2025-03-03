
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ForumAPI.Controllers
{
    public class Class
    {
        [Key]
        public int id { get; set; }

        
        public string? name { get; set; }
    }
}
