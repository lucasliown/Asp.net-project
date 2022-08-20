using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MvcBankAdmin.Models
{
    //Customer model used for Json and webApi.
    public class CustomerDto
    {
        public int CustomerID { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string? TFN { get; set; }


        [StringLength(50)]
        public string? Address { get; set; }

        
        [StringLength(40)]
        public string? Suburb { get; set; }

        [StringLength(3)]
        public string? State { get; set; }

        [StringLength(4)]
        public string? PostCode { get; set; }

        [StringLength(10)]
        public string? Mobile { get; set; }


        public virtual List<AccountDto>? Accounts { get; set; }

        [NotMapped]
        public virtual LoginDto? Login { get; set; }
    }
}