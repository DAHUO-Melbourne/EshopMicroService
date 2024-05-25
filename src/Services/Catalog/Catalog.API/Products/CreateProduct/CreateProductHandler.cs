namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price): ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
    // AbstractValidator的参数需要是被验证的class：CreateProductCommand
    {
        public CreateProductCommandValidator()
        {
            // RuleFor就是规定各种field的格式等
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be more than 0");
        }
    }
    // 1. 先新建一个CommandValidator class继承: AbstractValidator<CreateProductCommand>, 然后我们需要在handler class中传入IValidator<CreateProductCommand>
    internal class CreateProductCommandHandler
        (IDocumentSession session
        // IValidator<CreateProductCommand> validator
        // ILogger<CreateProductCommandHandler> logger
        )
            : ICommandHandler<CreateProductCommand, CreateProductResult>
    // IDocumentSession是Marten的一个包，功能是和postgresql数据库进行交互，是数据库的一种抽象表示
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // var result = await validator.ValidateAsync(command, cancellationToken);
            // var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
            // if (errors.Any())
            // {
            //     throw new ValidationException(errors.FirstOrDefault());
            // }
            // 1. create a product entity from command object
            // 2. save to database
            // 3. return result(which is a CreateProductResult object)
            // logger.LogInformation("CreateProductCommandHandler handler called with {@command}");

            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            // marten会检查数据库，如果数据库里没有schema/table，那么它就会新建一个table/schema
            // session是Marten的一个包，是用来存储/更新的，save the tokens into post db as a documemt object, ???? 关于document db和post db的关系
            // 使用Marten可以将Postgre db转化成类似Document db的机制，就很方便。
            return new CreateProductResult(product.Id);
        }
    }
}

/**
 * Handler文件相当于是Service文件
 */


/**
 * record其实相当于一种简化的定义class的方式。record就等价于class。上面的record CreateProductCommand其实相当于是说：定义了一个名字叫CreateProductCommand的class
 * 该class继承IRequest<CreateProductResult>，其中的参数：Name/Category/Description/ImageFile相当于是这个class的构造函数的参数，因此这段代码就等价于：
 * public class CreateProductCommand : IRequest<CreateProductResult>
{
    public string Name { get; }
    public List<string> Category { get; }
    public string Description { get; }
    public string ImageFile { get; }
    public decimal Price { get; }

    public CreateProductCommand(string name, List<string> category, string description, string imageFile, decimal price)
    {
        Name = name;
        Category = category;
        Description = description;
        ImageFile = imageFile;
        Price = price;
    }
}
 */

/**
 * 在使用CreateProductCommand时，无论这是一个class还是一个record，都要：
 * var command = new CreateProductCommand("ProductName", new List<string> { "Category1", "Category2" }, "ProductDescription", "ImageFilePath", 99.99m);
 * 通过new来传入构造函数的这些参数
 */

/**
 * 而IRequest<TResponse>则是MediatR 库中的一个接口，用于表示一个请求，该请求在被处理后会返回一个类型为 TResponse 的响应。
 * 这里的 CreateProductCommand 实现了 IRequest<CreateProductResult>，这意味着它是一个请求，并且这个请求在被处理后会返回一个 CreateProductResult 类型的响应。
 * 也就是说CreateProductCommand这个class相当于是一个请求（IRequest），这个请求执行处理结束后会返回一个类型为CreateProductResult的class
 */

/**
 * public Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
 * 则是一个MediatR的自带的函数约定，用来Handle request，第一个参数是CreateProductCommand的一个object，返回值则是CreateProductResult
 */

/**
 * IQuery和ICommand都继承了IRequest，而IQueryHandler和ICommandHandler都继承的IRequestHandler
 */

/**
 * 因为Catalog/Basket/Ordering会用到很多相似的架构，因此需要新建一个叫BuildingBlock的类来让他们继承,其中包含了所有的common operations,common packages
 */