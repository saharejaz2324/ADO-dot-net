using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ADO_dot_net.Models
{
    public class AlterTable
    {
        [Required]
        public string columnName { get; set; }
        [Required]
        public string columnType { get; set; }
    }
}
