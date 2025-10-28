using CoreOnion_Backend.Application.Features.Products.Command.CreateProduct;
using CoreOnion_Backend.Application.Interfaces.Repositories;
using CoreOnion_Backend.Application.Interfaces.UnitOfWorks;
using CoreOnion_Backend.Domain.Entities;
using FluentAssertions;
using MediatR;
using Moq;
using System.Linq.Expressions;


namespace CoreOnion_Backend.Test.Features.Products.Command
{
    public class CreateProductCommandUnitTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IReadRepository<Product>> _productReadRepositoryMock;
        private readonly Mock<IWriteRepository<Product>> _productWriteRepositoryMock;
        private readonly Mock<IWriteRepository<ProductCategory>> _productCategoryWriteRepositoryMock;

        public CreateProductCommandUnitTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productReadRepositoryMock = new Mock<IReadRepository<Product>>();
            _productWriteRepositoryMock = new Mock<IWriteRepository<Product>>();
            _productCategoryWriteRepositoryMock = new Mock<IWriteRepository<ProductCategory>>();

            _unitOfWorkMock.Setup(uow => uow.GetReadRepository<Product>()).Returns(_productReadRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.GetWriteRepository<Product>()).Returns(_productWriteRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.GetWriteRepository<ProductCategory>()).Returns(_productCategoryWriteRepositoryMock.Object);
        }


        [Fact]
        public async Task Handle_ValidRequest_ShouldCreateProductAndReturnUnitValue()
        {

            var brandIdGuid = Guid.NewGuid();
            var categoryId1 = Guid.NewGuid();
            var categoryId2 = Guid.NewGuid();

            var command = new CreateProductCommandRequest
            {
                Title = "Yeni Test Ürünü",
                Description = "Bu bir test ürünüdür.",
                BrandId = brandIdGuid, 
                Price = 150,
                Discount = 10,
                CategoryIds = new List<Guid> { categoryId1, categoryId2 } 
            };

            _productReadRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>>(), null, null, false))
                .ReturnsAsync(new List<Product>());
            _unitOfWorkMock.SetupSequence(uow => uow.SaveAsync())
                .ReturnsAsync(1) 
                .ReturnsAsync(1); 
            var handler = new CreateProductCommandHandler(null, _unitOfWorkMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().Be(Unit.Value);
            _productWriteRepositoryMock.Verify(w => w.AddAsync(It.Is<Product>(p => p.Title == command.Title)), Times.Once);

            _productCategoryWriteRepositoryMock.Verify(w => w.AddAsync(It.IsAny<ProductCategory>()), Times.Exactly(command.CategoryIds.Count));

            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_ExistingProductTitle_ShouldThrowException()
        {
            var brandIdGuid = Guid.NewGuid();
            var categoryId3 = Guid.NewGuid();

            var command = new CreateProductCommandRequest
            {
                Title = "Mevcut Ürün Başlığı",
                Description = "Bu ürün zaten var.",
                BrandId = brandIdGuid, 
                Price = 200,
                Discount = 0,
                CategoryIds = new List<Guid> { categoryId3 } 
            };

            var existingProduct = new Product(command.Title, "Açıklama", brandIdGuid, 200, 0);
            _productReadRepositoryMock
                .Setup(r => r.GetAllAsync(null, null, null, false))
                .ReturnsAsync(new List<Product> { existingProduct });

            var handler = new CreateProductCommandHandler(null, _unitOfWorkMock.Object);

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<Exception>().WithMessage("Ürün başlığı zaten mevcut.");

            _productWriteRepositoryMock.Verify(w => w.AddAsync(It.IsAny<Product>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Never);
        }
    }
}
