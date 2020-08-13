using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Entities {
    [Table("users")]
    public class EUser {
        [Key]
        public Int64 id { get; set; }
        public int pocId { get; set; }
        [Required][StringLength(200)]
        public string name { get; set; }
        public string avatarUrl { get; set; }
    }
}
