using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Infrastructure.Repositories;
using ShoppingCart.Domain.Database;
using ShoppingCart.Domain.Models;
using System.Linq;

public class ShopCartItemRepositoryTests
{
    private async Task<ApplicationDbContext> GetInMemoryDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    [Fact]
    public async Task AddProductToCartAsync_ShouldAddProduct()
    {
        // Arrange
        var context = await GetInMemoryDbContextAsync();
        var repo = new ShopCartItemRepository(context);
        var item = new ShopCartItem { productId = 1, cartId = 1, quantity = 2 };

        // Act
        await repo.AddProductToCartAsync(item);

        // Assert
        var result = await context.Products.FirstOrDefaultAsync(x => x.productId == 1);
        Assert.NotNull(result);
        Assert.Equal(2, result.quantity);
    }

    [Fact]
    public async Task FindAllProductsByCartIdAsync_ShouldReturnCorrectItems()
    {
        // Arrange
        var context = await GetInMemoryDbContextAsync();
        context.Products.AddRange(
            new ShopCartItem { productId = 1, cartId = 1 },
            new ShopCartItem { productId = 2, cartId = 1 },
            new ShopCartItem { productId = 3, cartId = 2 });
        await context.SaveChangesAsync();

        var repo = new ShopCartItemRepository(context);

        // Act
        var result = await repo.FindAllProductsByCartIdAsync(1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, x => Assert.Equal(1, x.cartId));
    }

    [Fact]
    public async Task FindProductByIdAsync_ShouldReturnItem()
    {
        // Arrange
        var context = await GetInMemoryDbContextAsync();
        var item = new ShopCartItem { productId = 42, cartId = 5 };
        context.Products.Add(item);
        await context.SaveChangesAsync();

        var _itemRepository = new ShopCartItemRepository(context);

        // Act
        var result = await _itemRepository.FindProductByIdAsync(42);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(42, result.productId);
    }

    [Fact]
    public async Task RemoveProductFromCartAsync_ShouldRemoveItem()
    {
        // Arrange
        var context = await GetInMemoryDbContextAsync();
        var item = new ShopCartItem { productId = 77, cartId = 3 };
        context.Products.Add(item);
        await context.SaveChangesAsync();

        var _itemRepository = new ShopCartItemRepository(context);

        // Act
        await _itemRepository.RemoveProductFromCartAsync(77);

        // Assert
        var result = await _itemRepository.FindProductByIdAsync(77);
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldUpdateItem()
    {
        // Arrange
        var context = await GetInMemoryDbContextAsync();
        var item = new ShopCartItem { productId = 11, cartId = 2, quantity = 1 };
        context.Products.Add(item);
        await context.SaveChangesAsync();

        var _itemRepository = new ShopCartItemRepository(context);
        item.quantity = 5;

        // Act
        await _itemRepository.UpdateProductAsync(item);

        // Assert
        var result = await _itemRepository.FindProductByIdAsync(11);
        Assert.Equal(5, result!.quantity);
    }

    [Fact]
    public async Task FindProductByIdAndCartAsync_ShouldReturnCorrectItem()
    {
        // Arrange
        var context = await GetInMemoryDbContextAsync();
        context.Products.Add(new ShopCartItem { productId = 10, cartId = 99 });
        await context.SaveChangesAsync();

        var repo = new ShopCartItemRepository(context);

        // Act
        var result = await repo.FindProductByIdAndCartAsync(99, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.productId);
        Assert.Equal(99, result.cartId);
    }
}
