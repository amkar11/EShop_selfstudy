using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.Queries;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CartController> _logger;
        public CartController(IMediator mediator, ILogger<CartController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProductToCart(int cartId, ShopCartItem item)
        {
            var command = new AddProductToCartCommand
            {
                cartId = cartId,
                item = item
            };

            await _mediator.Send(command);
            _logger.LogInformation("Product {ProductId} added to cart {CartId}", nameof(item), cartId);
            return Ok();
        }
        [HttpPost("add-product-quantity")]
        public async Task<IActionResult> AddProductQuantity(int cartId, int productId)
        {
            var command = new AddProductQuantityCommand { productId = productId };
            await _mediator.Send(command);
            _logger.LogInformation("Quantity of item {productId} in cart {cartId} successfully increased by 1", productId, cartId);
            return Ok();
        }

        [HttpPost("substract-product-quantity")]
        public async Task<IActionResult> SubstractProductQuantity(int cartId, int productId)
        {
            var command = new SubstractProductQuantityCommand { productId = productId };
            await _mediator.Send(command);
            _logger.LogInformation("Quantity of item {productId} in cart {cartId} successfully decreased by 1", productId, cartId);
            return Ok();
        }

        [HttpPost("remove-product")]
        public async Task<IActionResult> RemoveProductFromCart(int productId, int cartId)
        {
            var command = new RemoveProductFromCartCommand
            {
                productId = productId,
                cartId = cartId
            };
            await _mediator.Send(command);
            _logger.LogInformation("Product {ProductId} removed from cart {CartId}", productId, cartId);
            return Ok();
        }

        [HttpGet("get-cart")]
        public async Task<IActionResult> GetCart(int userId, int cartId)
        {
            var query = new GetCartQuery
            {
                cartId = cartId
            };

            var cart = await _mediator.Send(query);
            _logger.LogInformation("Retrieved cart {CartId} for user {UserId}", cartId, userId);
            return Ok(cart);
        }

        [HttpGet("get-all-carts")]
        public async Task<IActionResult> GetAllCarts(int userId)
        {
            var query = new GetAllCartsQuery
            {
                userId = userId
            };

            var carts = await _mediator.Send(query);
            _logger.LogInformation("Retrieved all the carts of the user {userId}", userId);
            return Ok(carts);
        }
    }
}
