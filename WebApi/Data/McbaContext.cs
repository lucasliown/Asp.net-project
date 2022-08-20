using Microsoft.EntityFrameworkCore;
using MvcBankAdmin.Models;

namespace MvcBankAdmin.Data;

public class McbaContext : DbContext
{
    //DcbaContext to store all data using EF core.
    public McbaContext(DbContextOptions<McbaContext> options) : base(options)
    { }

    //Store all models to the EF core as context 
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Login> Logins { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<BillPay> BillPay { get; set; }
    public DbSet<Payee> Payee { get; set; }

}
