using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankApplicationForWeb.Models.ViewModel
{
    //display the checkBox
    public class ViewPayeeInCheckBox
    {
        public string  ServiceName { get; set; }
         
        public SelectList? payeesNameList { get; set; }

        public BillPay billPay { get; set; }
    }
}
