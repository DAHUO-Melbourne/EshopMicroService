using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<IRequest>
        // 做一下filter：表明只会对ICommand的request进行验证，而不会对IQuery进行验证
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            // request就是接收到的request
            // next是pipeline velidation处理完成后，需要转发到的真正逻辑处理的函数
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResult =
                await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            // 这一行的意思是：task会在：所有的validator都执行完验证以后，将所有validator的规则的验证结果赋值给validationResult
            // 而validators，也就是全部的验证规则，是由new ValidationBehavior的时候传入的

            var failures =
                validationResult
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();
            // 将validationResult中的错误全部导出来，赋值给一个list

            if(failures.Any())
            {
                throw new ValidationException(failures);
            }

            return await next();
            // next也是传进来的，相当于是：如果验证成功，下一步应该做什么。
        }
    }
}
