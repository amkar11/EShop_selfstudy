using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart;
using ShoppingCart.Domain.Dto;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.Database;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.IntegrationTests;

public class CartControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly ApplicationDbContext _dbContext;

    public CartControllerTests(WebApplicationFactory<Program> factory)
    {
        var webAppFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Remove(services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>))!);
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb_Cart");
                });
            });
        });

        _client = webAppFactory.CreateClient();

        var scope = webAppFactory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task CreateCart_ShouldReturnOk_WithCartId()
    {
        // Arrange
        var dto = new CreateCartDto
        {
            userId = 123,
            is_checkout = null
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Cart/create-cart", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var cartId = await response.Content.ReadFromJsonAsync<int>();
        cartId.Should().BeGreaterThan(0);

        var cart = await _dbContext.Carts.FindAsync(cartId);
        cart.Should().NotBeNull();
        cart.userId.Should().Be(123);
        cart.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task AddProductToCart_ShouldReturnOk()
    {
        // Arrange
        var cart = new Cart { userId = 123, IsActive = true };
        _dbContext.Carts.Add(cart);
        await _dbContext.SaveChangesAsync();

        var item = new ShopCartItemDto
        {
            cartId = cart.cartId,
            productId = 555
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Cart/add-product", item);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var savedItem = _dbContext.Products.FirstOrDefault(x => x.cartId == cart.cartId && x.productId == 555);
        savedItem.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllItemsFromCart_ShouldReturnProducts()
    {
        // Arrange
        var cart = new Cart { userId = 999, IsActive = true };
        _dbContext.Carts.Add(cart);
        await _dbContext.SaveChangesAsync();

        _dbContext.Products.AddRange(new[]
        {
            new ShopCartItem { cartId = cart.cartId, productId = 1, quantity = 2 },
            new ShopCartItem { cartId = cart.cartId, productId = 2, quantity = 3 },
        });
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/Cart/get-products?cartId={cart.cartId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<ShopCartDto>>();
        items.Should().HaveCount(2);
    }

    [Fact]
    public async Task DeleteCart_ShouldRemoveCart()
    {
        // Arrange
        var cart = new Cart { userId = 1, IsActive = true };
        _dbContext.Carts.Add(cart);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"/api/Cart/delete-cart?cartId={cart.cartId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var deleted = await _dbContext.Carts.FindAsync(cart.cartId);
        deleted.Should().BeNull();
    }
}
