using BookApplication.Middleware;
using BookApplication.Services;
using BookApplication.Services.Interfaces;
using BookApplication.Utilities;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:4200")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});

// Add services to the container.
Utility.BookAPIBase = builder.Configuration["ServiceUrls:BookApi"];
builder.Services.AddHttpClient("Book", x => x.BaseAddress = new Uri(builder.Configuration["ServiceUrls:BookApi"]));
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBaseService, BaseService>();
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

app.UseCors(MyAllowSpecificOrigins);

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
