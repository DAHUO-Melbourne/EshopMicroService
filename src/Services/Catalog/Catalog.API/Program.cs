var builder = WebApplication.CreateBuilder(args);

// Add services to the container,dependency injection, �ȰѲ�ͬ�ķ�������ע�뵽ǰ�벿��

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline���ں�벿�֣��ȵ�����ע������ˣ���ʼconfiguration��ͬ��service

app.MapCarter();

app.Run();
