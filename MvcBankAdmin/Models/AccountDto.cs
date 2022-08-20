using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcBankAdmin.Models
{
    //Account model used for Json
    public class AccountDto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Display(Name = "Type")]
        public string AccountType { get; set; }

        public int CustomerID { get; set; }
        public virtual CustomerDto Customer { get; set; }

        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal? Balance { get; set; }

        // Set ambiguous navigation property with InverseProperty annotation or Fluent-API in the McbaContext.cs file.
        [InverseProperty("Account")]
        public virtual List<TransactionDto>? Transactions { get; set; }
    }
}