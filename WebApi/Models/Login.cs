using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBankAdmin.Models
{
    //Login model used for get and update data with EF core.
    public class Login
    {
        
        [StringLength(8)]
        public string LoginID { get; set; }

        public int CustomerID { get; set; }
        public virtual Customer? Customer { get; set; }

        
        [StringLength(64)]
        public string PasswordHash { get; set; }

        [StringLength(1)]
        public string? LockState { get; set; }
    }
}