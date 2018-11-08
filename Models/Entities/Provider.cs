using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.Entities
{
    public class Provider
    {
        public Provider() { }

        [Key]
        public int ProviderId { get; set; }
    }

}