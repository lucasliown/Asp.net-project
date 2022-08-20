namespace MvcBankAdmin.Models
{
    //View model to display information for bill pay.
    public class BillPayViewModel
    {
        public int BillPayId { get; set; }

        public int AccountNumber { get; set; }

        public virtual AccountDto Account { get; set; }

        public int PayeeId { get; set; }

        public virtual PayeeDto Payee { get; set; }

        public decimal Amount { get; set; }

        public string ScheduleTimeUtc { get; set; }

        public string Period { get; set; }

        public string transactionStatus { get; set; }

        public string? LockState { get; set; }
    }
}
