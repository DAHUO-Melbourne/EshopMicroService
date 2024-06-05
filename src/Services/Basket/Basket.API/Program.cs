var builder = WebApplication.CreateBuilder(args);

// ��building��ǰ��������ע��

var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    // ��pipeline�����֤
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    // ��pipeline���log
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

var app = builder.Build();

// ��building�Ժ����config
app.MapCarter();

app.Run();
