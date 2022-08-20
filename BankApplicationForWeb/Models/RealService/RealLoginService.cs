
namespace BankApplicationForWeb.Models
{
    public class RealLoginService
    {     
        public string LoginID { get; set; }

        public int CustomerID { get; set; }
       
        public string PasswordHash { get; set; }
     
        public string? LockState { get; set; }
    }
}