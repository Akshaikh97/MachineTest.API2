using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using API.Repository;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: api/Category/GetCategories
        [HttpGet("GetCategories")]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // GET: api/Category/GetCategory/5
        [HttpGet("GetCategory/{id}")]
        public async Task<ActionResult<CategoryModel>> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // POST: api/Category/AddCategory
        [HttpPost("AddCategory")]
        public async Task<ActionResult<int>> AddCategory(CategoryModel categoryModel)
        {
            int newCategoryId = await _categoryRepository.AddCategoryAsync(categoryModel);
            return Ok(newCategoryId);
        }

        // PUT: api/Category/UpdateCategory/5
        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryModel categoryModel)
        {
            await _categoryRepository.UpdateCategoryAsync(id, categoryModel);
            return NoContent();
        }

        // DELETE: api/Category/DeleteCategory/5
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryRepository.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
