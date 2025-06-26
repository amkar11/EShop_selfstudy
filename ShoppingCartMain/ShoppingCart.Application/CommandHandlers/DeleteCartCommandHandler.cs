using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.CommandHandlers
{
    public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand>
    {
        private readonly ICartRemover _remover;
        public DeleteCartCommandHandler(ICartRemover remover)
        {
            _remover = remover;
        }
        public async Task Handle(DeleteCartCommand command, CancellationToken cancellationToken)
        {
            await _remover.DeleteCartAsync(command.cartId);
        }
    }
}
