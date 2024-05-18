var builder = WebApplication.CreateBuilder(args);

// Add services to the container,dependency injection, �ȰѲ�ͬ�ķ�������ע�뵽ǰ�벿��

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddMarten(opts => {
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
// ����marten�������ݿ���߼�����AppSettings�ļ����ȡDatabase����
// ������postgres db��ʹ�ã���Ҫ�õ�docker, docker-compose

var app = builder.Build();

// Configure the HTTP request pipeline���ں�벿�֣��ȵ�����ע������ˣ���ʼconfiguration��ͬ��service

app.MapCarter();

app.Run();
