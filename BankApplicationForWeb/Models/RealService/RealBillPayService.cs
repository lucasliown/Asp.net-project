namespace BankApplicationForWeb.Models
{

    public class RealBillPayService
    {      
        public int BillPayId { get; set; }   
        public int AccountNumber { get; set; }   
        public int PayeeId { get; set; }     
        public decimal Amount { get; set; }    
        public DateTime ScheduleTimeUtc { get; set; }  
        public string Period { get; set; }
        public string? LockState { get; set; }
    }

}
