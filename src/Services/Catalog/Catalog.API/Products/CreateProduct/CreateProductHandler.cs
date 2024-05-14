using BuildingBlocks.CQRS;
using Catalog.API.Models;
using MediatR;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price): ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // 1. create a product entity from command object
            // 2. save to database
            // 3. return result(which is a CreateProductResult object)

            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
            };
            return new CreateProductResult(Guid.NewGuid());
        }
    }
}


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