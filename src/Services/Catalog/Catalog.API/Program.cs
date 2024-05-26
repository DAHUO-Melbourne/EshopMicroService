using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container,dependency injection, �ȰѲ�ͬ�ķ�������ע�뵽ǰ�벿��

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    // ��ξ����ڸ�ÿ���̳�MediatR��handler��Ӿ����pipeline����
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(opts => {
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
// ����marten�������ݿ���߼�����AppSettings�ļ����ȡDatabase����
// ������postgres db��ʹ�ã���Ҫ�õ�docker, docker-compose

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
// ע��CustomExceptionHandler����

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);
// ���AddHealthChecks�����postgre���ݿ��Լ�docker�Ƿ�����������

var app = builder.Build();

// Configure the HTTP request pipeline���ں�벿�֣��ȵ�����ע������ˣ���ʼconfiguration��ͬ��service

app.MapCarter();

app.UseExceptionHandler(option => { });
// ��UseExceptionHandler����config

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
// ��health check�Ľ��UI���ӻ���

app.Run();
