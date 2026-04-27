
namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart ShoppingCart) : ICommand<StoreBasketResult>;

   public record StoreBasketResult(int UserId);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCart).NotNull().WithMessage("Shopping Cart can not be null");
        RuleFor(x => x.ShoppingCart.UserId).NotEmpty().WithMessage("UserName is required");
    }
}

public class StoreBasketHandler
    (IBasketRepository repository,
    DiscountProtoService.DiscountProtoServiceClient discountProto
    ) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await DeductDiscount(command.ShoppingCart, cancellationToken);

        await repository.StoreBasket(command.ShoppingCart, cancellationToken);

        return new StoreBasketResult(command.ShoppingCart.UserId);
    }

private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
{
    // Communicate with Discount.Grpc and calculate lastest prices of products into sc
    // TODO: Add a way to create discount
    foreach (var item in cart.Items)
    {
        var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { GameId = item.GameId }, cancellationToken: cancellationToken);
        item.Price -= item.Price * coupon.Amount;

            if(item.Price < 0)
            {
                item.Price = 0;
            }
    }
}
}