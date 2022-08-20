using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBankAdmin.Models
{
    //Payee model used for Json and webApi.
    public class PayeeDto
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PayeeId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
        [StringLength(3)]
        public string State { get; set; }
        [StringLength(4)]
        public string Postcode { get; set; }
        [StringLength(10)]
        public string Phone { get; set; }
    }

}