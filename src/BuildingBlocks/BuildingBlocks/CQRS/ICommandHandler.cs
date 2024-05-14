using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.CQRS
{
    public interface ICommandHandler<in TCommand>: ICommandHandler<TCommand, Unit> where TCommand: ICommand<Unit>;
    public interface ICommandHandler<in TCommand, TResponse>: IRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse> where TResponse : notnull
        // in和out是用来定义<>泛型中的类型是用于输入的参数还是return value的类型的。in代表的是输入的参数类型，out是return的数据的类型
    {
    }
}
