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
    public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand>
    {
        public ICartCreater _reader;
        public CreateCartCommandHandler(ICartCreater reader)
        {
            _reader = reader;
        }
        public async Task Handle(CreateCartCommand command, CancellationToken cancellationToken)
        {
            await _reader.CreateCartAsync(command.userId);
        }
    }
}
