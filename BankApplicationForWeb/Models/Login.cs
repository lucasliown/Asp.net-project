using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationForWeb.Models
{
    public class Login
    {
        [Column(TypeName = "char")]
        [StringLength(8)]
        public string LoginID { get; set; }

        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Column(TypeName = "char")]
        [Required, StringLength(64)]
        public string PasswordHash { get; set; }

        [StringLength(1)]
        public string? LockState { get; set; }
    }
}