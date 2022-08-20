namespace BankApplicationForWeb.Models.ViewModel
{
    //for display the billpay
    public class BillPayViewModel { 
  
    public int BillPayId { get; set; }

    public int AccountNumber { get; set; }

    public int PayeeId { get; set; }

    public decimal Amount { get; set; }

    public string ScheduleTimeUtc { get; set; }

    public string Period { get; set; }

    public string Status { get; set; }

    public string? LockState { get; set; }
    }
}
