
using Basket.API.Basket.StoreBasket;

namespace Basket.API.Basket.DeleteBasket
{
    public record DelectBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsSuccess);
    public class DeleteBasketCommandValidator : AbstractValidator<DelectBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotNull().WithMessage("UserName is required");
        }
    }
    public class DeleteBasketCommandHandler : ICommandHandler<DelectBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DelectBasketCommand command, CancellationToken cancellationToken)
        {
            return new DeleteBasketResult(true);
        }
    }
}
