Products文件夹是我们的feature folder，里面放的是针对product的一系列操作
CreateProductEndpoint 相当于是controller，而CreateProductHandler则相当于是service/repository


Catalog.API文件就相当于是js项目的package.json, 所有的package/依赖，dependencies，reference都会在这里吗管理

MediatR用来管理command/query的service处理逻辑
Carter是用来简化endpoint的
Mapster是用来将object map到MediatR的ICommand上的

backing service是那些不影响micro-service内部运行逻辑，但能够提供帮助，比如说db/cache等
连接post db，需要添加docker-compose文件：到Catalog.API上，右键，add, container orchestrator support来添加docker-compose文件
关于写docker compose yml文件，indentation非常关键，必须是nested的结构

68. 因为我们的app在本地以及docker的port number是不一样的，所以我们在使用postman测试的时候需要更改port number。为了不每次都手动修改port number
	我们使用postman的collections和environment功能。我们可以把port number设置成variable，这样就可以很方便的切换生产环境。environment是用来存储不同环境下的常量/url的，collection则可以存储各种endpoint
	比如说：{{catalog_url}}/products

69. 开发GetProductById 的handler
70. 开发GetProductById 的endpoint
82. 这一节课主要讲的是关于MediatR的另一个用途：关于pipeline的概念。pipeline相当于一个中间件，在将请求转发给handler之前，需要先进行一些process与verification，
	这个时候就可以用到MediatR了，可以添加一些preprocessor behavior，而在handler处理完成之后，可以进行一些post processor behaviour。
	比如说一个request可以经过好几个middle ware中间件，不同middleware通过next function连接，最后到达handler。
	另一个库是fluent，是用来构建validation rules的框架。fluent和MediatR一般配合一起使用，可以将request通过中间件功能，在他们抵达handler之前进行验证。这些中间件的功能包括
	velidation/logging/exception handling.我们最终安装的包是
84. 为createProductHandler添加velidation，规则是验证这些参数不能为null
    先新建一个CommandValidator class继承: AbstractValidator<CreateProductCommand>, 然后我们需要在handler class中传入IValidator<CreateProductCommand>
	然后去program.cs里：注入依赖：builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
85. (1) 因为pipeline velidation是每个微服务都会用到的，因此我们将这些功能（logging/velidation等）统一拿出来放到BuildingBlocks里
	(2) 完成相关逻辑之后，我们需要将`config.AddOpenBehavior(typeof(ValidationBehavior<,>));`注入到program中，意思是以后所有的mediatr验证都需要过一遍ValidatorBehavior的pipeline验证
86. 将CreateProductHandler里的IValidator删除，因为我们已经在Program中注册了config逻辑，它会自动检测Handler里的`AbstractValidator`, 然后添加到监听过程中，我们无需手动在handler里编写validator逻辑了
	(with validator defined(`public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>`), our mediatr pipeline will automatically invoke them)
87. 更新Update和Delete的validation逻辑
88. globalExceptionHandling：因为throw出来的error看着太难看了，所以需要使用`globalExceptionHandling`让error msg看起来更易读: return a structured JSON response
90. 在BuildingBlock里添加一个global的CustomExceptionHandler，在91里可以用来替换Program.cs里面的`globalExceptionHandling`，因为这个看起来更全面，更科学
	通过添加`builder.Services.AddExceptionHandler<CustomExceptionHandler>();`以及`app.UseExceptionHandler(option => { });`
92. 因为每个Handler class里面都有Logger，所以我们也希望把Logger拿出来放进Pipeline里，就像validator一样。这一节的工作是create一个Logger class，将会记录所有的request与response
	首先在bb的behavior folder里添加新的behavior（也是validatorBehavior的地方）,在文件夹中编写完新的逻辑之后：
	添加在`program.cs`中`config.AddOpenBehavior(typeof(LoggingBehavior<,>));`
94. initialize seeding db: 初始化数据库并且添加一些必要的初始数据，就叫seeding DB。Marten提供了一个叫`IInitialData`的工具函数来完成这一目标.
	具体逻辑是`session.Store(_initialData); await session.SaveChangesAsync()`
	首先新建文件夹：Data，里面新建一个class叫CatalogInitialData
	在完成相关逻辑之后, 在`Program.cs`中添加：
	```
	if (builder.Environment.IsDevelopment())
	{
		builder.Services.InitializeMartenWith<CatalogInitialData>();
	}
	```
	值得注意的是: 这个seeding function必须需要database启动：但是如果db没有正常启动呢？这个时候就需要我们使用其他的库来retry start db/docker中的db了
96. 开发pagination：给GetProducts query加pagination，使用ToPagedListAsync方法，Marten只需要PageNumber和PageSize这两个参数：
	```
	theSession.Query<Target>().ToPagedList(pageNumber, pageSize)
	```
	注意：`Marten`是用来操作数据库的库/MediatR是用来处理网络请求的库
98. 添加health check
99. 为了微服务的部署：我们需要将所有的微服务后端，和数据库一起部署到docker里。本地开发的话是本地电脑跑后端，docker包括了数据库。但是微服务部署的话需要将后端和数据库一起放到docker里
	因此这一小节我们需要编写docker文件：
	```
	FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
	USER app
	WORKDIR /app
	EXPOSE 8080
	EXPOSE 8081
	```
	这一段是设置docker运行环境：需要是.net8的环境
	```
	FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
	ARG BUILD_CONFIGURATION=Release
	WORKDIR /src
	COPY ["Services/Catalog/Catalog.API/Catalog.API.csproj", "Services/Catalog/Catalog.API/"]
	COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
	RUN dotnet restore "./Services/Catalog/Catalog.API/./Catalog.API.csproj"
	COPY . .
	WORKDIR "/src/Services/Catalog/Catalog.API"
	RUN dotnet build "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build
	```
	这一段是设置需要构建的项目：catalog+buildingBlocks
	```
	FROM build AS publish
	ARG BUILD_CONFIGURATION=Release
	RUN dotnet publish "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
	```
	这一段是设置publish
	```
	FROM base AS final
	WORKDIR /app
	COPY --from=publish /app/publish .
	ENTRYPOINT ["dotnet", "Catalog.API.dll"]
	```
	set up the endpoint in order to run our application
100	. 我们将会使用docker-compose文件来orchestrate（编排）所有的microservices+对应的db的communication。之前我们只用docker来运行数据库，现在我们要用docker来连接所有的微服务了
	add container orchestrator support -> docker-compose.yml文件里会多出
	```
	catalog.api:
    image: ${DOCKER_REGISTRY-}catalogapi
    build:
      context: .
      dockerfile: Services/Catalog/Catalog.API/Dockerfile
	```
	override文件里：
	```
	ports:
    - "6000:8080"
    - "6060:8081"
	```
	这一部分是用于规定catalog在docker中用于收发网络请求的端口号
	```
    - ConnectionStrings__Database=Server=catalogdb;Port=5432;Database=CatalogDb;User Id=postgres;Password=postgres;Include Error Detail=true
	```
	这一部分用于设置在docker中与数据库连接的connection string，和appSettings里的一样
	然后添加
	```
	depends_on:
      - catalogdb
	```
	意思是catalog.api依赖于catalogdb
	docker containers on the same network can communicate with each other using the service names
	此外：
	`ConnectionStrings__Database`双下划线意味着overwrite connectionStrings in the appSettings
	总的来说：每个微服务都有自己的docker文件：用于设置自己这个微服务在docker里的配置信息。
	而最外面的docker-compose文件则用来对所有的docker文件进行统筹规划，确定不同docker文件之间的关系
101. 如何跑docker-compose文件：在docker-compose右键 -> Open in Terminal：
	`docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d`