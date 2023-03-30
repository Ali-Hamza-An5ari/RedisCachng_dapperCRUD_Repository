using dapperCRUD.Services.CustomerService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Dependency Injection
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

#endregion

builder.Services.AddSwaggerGen();
#region Redis Cache

builder.Services.AddDistributedRedisCache(
    options =>
    {
        options.Configuration = "localhost:6379";
    });

#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
