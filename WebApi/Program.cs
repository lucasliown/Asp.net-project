using MvcBankAdmin.Data;
using Microsoft.EntityFrameworkCore;
using MvcBankAdmin.Models.DataMananger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<McbaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("McbaContext")));
//Add scoped for all model.
builder.Services.AddScoped<CustomerManager>();
builder.Services.AddScoped<AccountManager>();
builder.Services.AddScoped<TransactionManager>();
builder.Services.AddScoped<LoginManager>();
builder.Services.AddScoped<BillPayManager>();
builder.Services.AddScoped<PayeeManager>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.MapGet("api/UsingMapGet", (string name, int? repeat) =>
{
    if (string.IsNullOrWhiteSpace(name))
        name = "(empty)";
    if (repeat is null or < 1)
        repeat = 1;
    return string.Join(' ', Enumerable.Repeat(name, repeat.Value));
});
app.Run();
