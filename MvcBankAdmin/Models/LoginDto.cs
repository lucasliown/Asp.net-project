using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBankAdmin.Models
{
    //Login model used for Json and webApi.
    public class LoginDto
    {
        [StringLength(8)]
        public string LoginID { get; set; }

        public int CustomerID { get; set; }

        public CustomerDto Customer { get; set; }

        [Required, StringLength(64)]
        public string PasswordHash { get; set; }

        [StringLength(1)]
        public string? LockState { get; set; }
    }
}