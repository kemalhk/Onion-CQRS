using Api.Application.Dtos;
using Api.Application.Interfaces.AutoMapper;
using Api.Application.Interfaces.UnitOfWorks;
using Api.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Futures.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, IList<GetAllProductsQueryResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IList<GetAllProductsQueryResponse>> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var products = await unitOfWork.GetReadRepository<Product>().GetAllAsync(include: x=> x.Include(b=> b.Brand));

            mapper.Map<BrandDto,Brand>(new Brand());
            var response = mapper.Map<GetAllProductsQueryResponse, Product>(products);
            foreach (var item in response)
            {
                item.Price -= (item.Price * item.Discount / 100);
            }

            return response;
        }
    }
}
