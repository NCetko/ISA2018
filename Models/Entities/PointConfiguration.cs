using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.Entities
{
    public class PointConfiguration
    {
        public PointConfiguration() {
        }

        [Key]
        public string Key { get; set; }
        public int Value { get; set; }

    }

}