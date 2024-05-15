var builder = WebApplication.CreateBuilder(args);

// Add services to the container,dependency injection, 先把不同的服务依赖注入到前半部分

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline，在后半部分：等到依赖注入完毕了，开始configuration不同的service

app.MapCarter();

app.Run();
