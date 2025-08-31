using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using OrderService.Controllers;
using OrderService.Data;
using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Tests
{
    [TestFixture]
    public class OrdersControllerTests
    {
        private Mock<OrderContext> _mockContext;
        private  OrdersController _controller;

        [SetUp]
        public void Setup()
        {

            var options = new DbContextOptionsBuilder<OrderContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
            // Initialize the mock context with the options
            _mockContext = new Mock<OrderContext>(options);

            // Initialize the controller with the mock context
            _controller = new OrdersController(_mockContext.Object);

        }

        [Test]
        public async Task CreateOrder_ValidOrder_ReturnsCreatedAtAction()
        {
            // Arrange
            var order = new Order { Id = 1, UserId = "user1", ProductId = "PROD-001", Quantity = 2, Status = "Pending", OrderDate = DateTime.UtcNow };
            _mockContext.Setup(m => m.Orders.Add(order)).Verifiable();
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateOrder(order);

            // Assert
            // Corrected assertion syntax using Assert.That
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            // For comparing object equality, use Is.EqualTo
            Assert.That(createdResult.Value, Is.EqualTo(order));
        }

        [Test]
        public async Task UpdateOrder_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var order = new Order { Id = 1, UserId = "user1", ProductId = "PROD-001", Quantity = 2, Status = "Pending", OrderDate = DateTime.UtcNow };
            _mockContext.Setup(m => m.Orders.FindAsync(2)).ReturnsAsync((Order)null);

            // Act
            var result = await _controller.UpdateOrder(2, order);

            // Assert
            // Direct assertion on the result object
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeleteOrder_ExistingOrder_ReturnsNoContent()
        {
            // Arrange
            var order = new Order { Id = 1, UserId = "user1", ProductId = "PROD-001", Quantity = 2, Status = "Pending", OrderDate = DateTime.UtcNow };
            _mockContext.Setup(m => m.Orders.FindAsync(1)).ReturnsAsync(order);
            _mockContext.Setup(m => m.Orders.Remove(order)).Verifiable();
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _controller.DeleteOrder(1);

            // Assert
            // Direct assertion on the result object
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }


    }


}
