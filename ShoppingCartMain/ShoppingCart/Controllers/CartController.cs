using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.Database;
using ShoppingCart.Domain.Dto;
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
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CartController(IMediator mediator, ILogger<CartController> logger, ApplicationDbContext context, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProductToCart(ShopCartItemDto itemDto)
        {
            var item = new ShopCartItem
            {
                cartId = itemDto.cartId,
                productId = itemDto.productId
            };
            var command = new AddProductToCartCommand
            {
                item = item
            };

            await _mediator.Send(command);
            _logger.LogInformation("Product {ProductId} added to cart {CartId}", nameof(item), item.cartId);
            return Ok();
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetAllItemsFromCart([FromQuery] int cartId)
        {
            var query = new GetAllItemsFromCartQuery { cartId = cartId };
            var items = await _mediator.Send(query);
            List<ShopCartDto> response = new List<ShopCartDto>();
            for (int i = 0; i < items.Count; i++)
            {
                response.Add(_mapper.Map<ShopCartDto>(items[i]));
                
            }
            _logger.LogInformation("All items from cart {cartId} successfuly retrieved", cartId);
            return Ok(response);
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

        [HttpPost("create-cart")]
        public async Task<IActionResult> CreateCart([FromBody] CreateCartDto createCartDto)
        {
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                var userId = createCartDto.userId;
                if (createCartDto.is_checkout is not null) {
                    var get_cart_inactive = new GetAllCartsQuery { userId = userId };
                    var carts_inactive = await _mediator.Send(get_cart_inactive);
                    var cart_inactive = carts_inactive.FirstOrDefault(x => x.IsActive == true);
                    ArgumentNullException.ThrowIfNull(cart_inactive);
                    cart_inactive.IsActive = false;
                    var updated_cart = new UpdateCartCommand { cart = cart_inactive };
                    await _mediator.Send(updated_cart);
                    _logger.LogInformation($"Successfully changed status of cart to inactive: {cart_inactive.cartId}");
                }
                var command = new CreateCartCommand { userId = userId };
                await _mediator.Send(command);
                var get_cart = new GetAllCartsQuery { userId = userId };
                var carts = await _mediator.Send(get_cart);
                var cart = carts.FirstOrDefault(x => x.IsActive == true);
                ArgumentNullException.ThrowIfNull(cart);
                _logger.LogInformation("New cart {cartId} was added to user with following id {userId}", cart.cartId, cart.userId);
                await transaction.CommitAsync();
                return Ok(cart.cartId);
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException(ex.Message);
            }
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

        [HttpDelete("delete-cart")]
        public async Task<IActionResult> DeleteCart([FromQuery] int cartId)
        {
            var command = new DeleteCartCommand { cartId = cartId };
            await _mediator.Send(command);
            var query = new GetCartQuery { cartId = cartId };
            var deleted_cart = _mediator.Send(query);
            _logger.LogInformation("Cart successfully deleted: {deletedCart}", deleted_cart is null);
            return Ok();
        }
    }
}
