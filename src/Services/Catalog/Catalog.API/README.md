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