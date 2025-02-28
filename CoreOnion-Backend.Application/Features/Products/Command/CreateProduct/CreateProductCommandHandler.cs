using CoreOnion_Backend.Application.Interfaces.AutoMapper;
using CoreOnion_Backend.Application.Interfaces.UnitOfWorks;
using CoreOnion_Backend.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Application.Features.Products.Command.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            IList<Product> products = await _unitOfWork.GetReadRepository<Product>().GetAllAsync();

            if (products.Any(p => p.Title == request.Title))
                throw new Exception("Ürün başlığı zaten mevcut.");

            Product product = new(request.Title, request.Description, request.BrandId, request.Price, request.Discount);

            await _unitOfWork.GetWriteRepository<Product>().AddAsync(product);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                foreach (var categoryId in request.CategoryIds)
                    await _unitOfWork.GetWriteRepository<ProductCategory>().AddAsync(new()
                    {
                        ProductId = product.Id,
                        CategoryId = categoryId
                    });

                await _unitOfWork.SaveAsync();
            }

            return Unit.Value;
        }
    }
}
