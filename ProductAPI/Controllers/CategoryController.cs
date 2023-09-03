using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace ProductAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private IProductRepository repository = new ProductRepository();

        // GET: api/
        [HttpGet]
        public IActionResult GetCategories()
        {
            return Ok(repository.GetCategories());
        }
    }
}
