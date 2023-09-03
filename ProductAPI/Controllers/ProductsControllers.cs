using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace ProductAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsControllers : Controller
    {

        private IProductRepository repository = new ProductRepository();

        // GET: api/Products
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(repository.GetProducts());
        }

        // GET: api/Products/id
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            return Ok(repository.GetProductById(id));
        }

        // POST: ProductsControllers/Products
        [HttpPost]
        public IActionResult PostProduct(Product p)
        {
            repository.SaveProduct(p);
            return NoContent();
        }

        // DELETE: ProductsControllers/Delete/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {   
            var p = repository.GetProductById(id);
            if (p == null)
                return NotFound();
            repository.DeleteProduct(p);
            return NoContent();
        }

        // PUT: ProductsControllers/Update/5
        [HttpPut]
        public IActionResult UpdateProduct(Product p)
        {
            var pTmp = repository.GetProductById(p.ProductId);
            if (pTmp == null)
                return NotFound();
            repository.UpdateProduct(p);
            return NoContent();
        }
    }
}
