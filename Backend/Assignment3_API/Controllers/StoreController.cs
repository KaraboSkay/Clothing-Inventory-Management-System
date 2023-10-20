using Assignment3_Backend.Models;
using Assignment3_Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StoreController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            product.ProductType = null;
            product.Brand = null;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpGet("ProductListing")]
        public async Task<IActionResult> ProductListing()
        {
            var products = await _context.Products
            .Include(p => p.ProductType)
            .Include(p => p.Brand)
            .Select(p => new ProductViewModel
            {
                image = p.Image,
                price = p.Price,
                productType = p.ProductType.Name, 
                brand = p.Brand.Name,             
                description = p.Description,
                name = p.Name
            })
            .ToListAsync();

            return Ok(products);
        }

     
        [HttpGet("GetBrands")]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _context.Brands.ToListAsync();
            return Ok(brands);
        }

   
        [HttpGet("GetProductTypes")]
        public async Task<IActionResult> GetProductTypes()
        {
            var productTypes = await _context.ProductTypes.ToListAsync();
            return Ok(productTypes);
        }
    }
}
