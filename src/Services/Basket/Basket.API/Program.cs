var builder = WebApplication.CreateBuilder(args);

// 在building以前进行依赖注入

var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    // 给pipeline添加验证
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    // 给pipeline添加log
});

var app = builder.Build();

// 在building以后进行config
app.MapCarter();

app.Run();
