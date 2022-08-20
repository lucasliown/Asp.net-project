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

namespace BankApplicationForWeb.Tests;

public class HomeControllerTests : IDisposable
{
    private readonly McbaContext _context;
    private readonly ILogger<HomeController> _logger;

    private readonly RealBankService _RealBankService;
    public HomeControllerTests()
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
    public async Task Index_ReturnsAccountResult_WithAListOfOwnerAccount()
    {
        // Use a clean instance of the context to run the test.
        // Arrange.

        var controller = new HomeController(_logger, _RealBankService);

        // Act.
        var result = controller.ShowAccount();

        // Assert.
        var viewResult = Assert.IsType<AccountViewModel>(result);
        var model = Assert.IsAssignableFrom<List<AccountViewModel>>(viewResult);
        Assert.Equal(2, model.Count());
    }


}
