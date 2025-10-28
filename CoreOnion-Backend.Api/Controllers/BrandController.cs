using CoreOnion_Backend.Application.Consts;
using CoreOnion_Backend.Application.CustomAttributes;
using CoreOnion_Backend.Application.Features.Brands.Command.CreateBrand;
using CoreOnion_Backend.Application.Features.Brands.Queries.GetAllBrands;
using CoreOnion_Backend.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreOnion_Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "User")]
    public class BrandController : ControllerBase
    {
        private readonly IMediator mediator;

        public BrandController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Brands, ActionType = ActionType.Writing, Definition = "Create Brands")]
        public async Task<IActionResult> CreateBrand(CreateBrandCommandRequest request)
        {
            await mediator.Send(request);
            return Ok();
        }

        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Brands, ActionType = ActionType.Reading, Definition = "Get All Brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var response = await mediator.Send(new GetAllBrandsQueryRequest());
            return Ok(response);
        }
    }
}
