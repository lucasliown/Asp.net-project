using BankApplicationForWeb.Controllers;
using BankApplicationForWeb.Data;
using BankApplicationForWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.Extensions.Logging;
using BankApplicationForWeb.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using McbaExample.Controllers;

namespace BankApplicationForWeb.Tests;

public class BilPayControllerTests : IDisposable
{
    private readonly McbaContext _context;

    private readonly RealBankService _RealBankService;
    public BilPayControllerTests()
    {
        // Seed data into the database using an in-memory instance of the context.
        _context = new McbaContext(new DbContextOptionsBuilder<McbaContext>().
            UseInMemoryDatabase(nameof(McbaContext)).Options);
        // OR
        //UseInMemoryDatabase("MagicInventoryContext").Options);
        SeedDataForTest.Initialize(_context);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Index_ReturnsCustomerResult_WithAListOfAccount()
    {
       // Use a clean instance of the context to run the test.
       // Arrange.

       var controller = new BillPayController(_context, _RealBankService);

       // Act.
       var result = controller.Index();

        //Assert.
       var viewResult = Assert.IsType<ViewAccountTypeModel>(result);
        var model = Assert.IsAssignableFrom<ViewAccountTypeModel>(viewResult);
        Assert.Equal(1, model.AccountNumber);
    }


}
