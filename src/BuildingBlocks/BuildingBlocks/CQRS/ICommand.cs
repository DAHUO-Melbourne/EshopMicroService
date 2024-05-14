using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface ICommand: ICommand<Unit>
    {
    }

    // <>里的类型是泛型generic，相当于该interface返回的数据类型，也就是说ICommand返回一个TResponse类型。他继承IRequest接口，会包含该接口里的成员
    public interface ICommand<out TResponse>: IRequest<TResponse>
    {
    }
}

/**
 * 
 */
