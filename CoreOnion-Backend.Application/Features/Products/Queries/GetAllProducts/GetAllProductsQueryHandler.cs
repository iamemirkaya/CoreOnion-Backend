using CoreOnion_Backend.Application.DTOs;
using CoreOnion_Backend.Application.Interfaces.AutoMapper;
using CoreOnion_Backend.Application.Interfaces.UnitOfWorks;
using CoreOnion_Backend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, IList<GetAllProductsQueryResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<GetAllProductsQueryHandler> logger;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllProductsQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IList<GetAllProductsQueryResponse>> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var products = await unitOfWork.GetReadRepository<Product>().GetAllAsync(include: x => x.Include(b => b.Brand));

            logger.LogInformation("Toplam {Count} ürün getirildi.", products.Count);

            var brand = mapper.Map<BrandDto, Brand>(new Brand());

            var map = mapper.Map<GetAllProductsQueryResponse, Product>(products);
            foreach (var item in map)
                item.Price -= (item.Price * item.Discount / 100);

            return map;
        }
    }
}
