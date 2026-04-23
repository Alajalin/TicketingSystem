using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Interfaces;

namespace TicketingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
	private readonly IProductRepository _productRepo;

	public ProductsController(IProductRepository productRepo) =>
		_productRepo = productRepo;

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var products = await _productRepo.GetAllActiveAsync();
		return Ok(products);
	}
}