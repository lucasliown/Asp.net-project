using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationForWeb.Models
{

    public class BillPay
    {
       
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BillPayId { get; set; }

        
        [ForeignKey("Account")]
        public int AccountNumber { get; set; }

        public virtual Account?  Account { get; set; }

        [ForeignKey("Payee")]
        public int PayeeId { get; set; }

        public virtual Payee?  Payee { get; set; }
       
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime ScheduleTimeUtc { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(1)]
        public string Period { get; set; }
     
        [StringLength(10)]
        public string?  transactionStatus { get; set; }

        [StringLength(1)]
        public string? LockState { get; set; }


    }

}