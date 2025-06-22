using MediatR;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.CommandHandlers
{
    public class AddProductQuantityCommandHandler : IRequestHandler<AddProductQuantityCommand>
    {
        private readonly IProductQuantityChanger _changer;

        public AddProductQuantityCommandHandler(IProductQuantityChanger changer)
        {
            _changer = changer;
        }

        public async Task Handle(AddProductQuantityCommand command, CancellationToken cancellationToken)
        {
            await _changer.AddProductQuantityAsync(command.cartId, command.productId);
        }
    }
}
