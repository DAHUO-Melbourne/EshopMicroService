using MediatR;

namespace BuildingBlocks.CQRS
{
    // 定义一个不返回结果的查询接口，继承自 IQuery<Unit>
    public interface IQuery : IQuery<Unit>
    // Unit 是 MediatR 中的一个结构体，用来表示没有返回值的情况，类似于 void 但在泛型中使用。
    {
    }

    // 定义一个通用的查询接口，继承自 IRequest<TResponse>
    public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse: notnull
        //  where TResponse: notnull意思是TResponse要满足notnull的约束，TResponse的值不能为null，也就是返回值不能为空,where相当于filter的作用
    {
    }
}
