var builder = WebApplication.CreateBuilder(args);

// 在building以前进行依赖注入

var app = builder.Build();

// 在building以后进行config

app.Run();
