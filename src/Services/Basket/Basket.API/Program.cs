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

var app = builder.Build();

// ��building�Ժ����config
app.MapCarter();

app.Run();
