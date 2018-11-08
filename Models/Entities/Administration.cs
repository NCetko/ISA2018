using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.Models.Entities
{
    public class Administration
    {
        public Administration() { }

        public string ApplicationUserId { get; set; }
        public int ProviderId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("ProviderId")]
        public Provider Provider { get; set; }
    }

}