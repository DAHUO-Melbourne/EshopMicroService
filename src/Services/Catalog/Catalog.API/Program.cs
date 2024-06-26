using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container,dependency injection, 先把不同的服务依赖注入到前半部分

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    // 这段就是在给每个继承MediatR的handler添加具体的pipeline内容
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(opts => {
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
// 这是marten连接数据库的逻辑。从AppSettings文件里读取Database变量
// 而对于postgres db的使用：需要用到docker, docker-compose

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
// 注入CustomExceptionHandler依赖

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);
// 添加AddHealthChecks来检测postgre数据库以及docker是否被正常连接了

var app = builder.Build();

// Configure the HTTP request pipeline，在后半部分：等到依赖注入完毕了，开始configuration不同的service

app.MapCarter();

app.UseExceptionHandler(option => { });
// 对UseExceptionHandler进行config

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
// 将health check的结果UI可视化掉

app.Run();
