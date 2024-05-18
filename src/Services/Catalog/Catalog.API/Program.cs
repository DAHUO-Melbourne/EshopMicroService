var builder = WebApplication.CreateBuilder(args);

// Add services to the container,dependency injection, 先把不同的服务依赖注入到前半部分

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddMarten(opts => {
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
// 这是marten连接数据库的逻辑。从AppSettings文件里读取Database变量
// 而对于postgres db的使用：需要用到docker, docker-compose

var app = builder.Build();

// Configure the HTTP request pipeline，在后半部分：等到依赖注入完毕了，开始configuration不同的service

app.MapCarter();

app.Run();
