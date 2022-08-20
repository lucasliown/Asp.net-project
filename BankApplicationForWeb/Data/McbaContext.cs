﻿using Microsoft.EntityFrameworkCore;
using BankApplicationForWeb.Models;

namespace BankApplicationForWeb.Data;

public class McbaContext : DbContext
{
    //this is the Entity framwork core database object 
    public McbaContext(DbContextOptions<McbaContext> options) : base(options)
    { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Login> Logins { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<BillPay> BillPay { get; set; }

    public DbSet<Payee> Payee { get; set; }
}
