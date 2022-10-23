using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyQueue.Data;
using MyQueue.Data.Models;
using MyQueue.DataTansferObject.FoodManipulation;

namespace MyQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly MQDBContext _context;

        public FoodController(MQDBContext context)
        {
            _context = context;
        }

        // GET: api/Food
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            return await _context.Category.ToListAsync();
        }

        // GET: api/Food/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Food/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Food/category
        [HttpPost("category")]
        public async Task<ActionResult<CategoryDTO>> AddCategory(CategoryDTO categoryDto)
        {
            Category category = new Category() { Name = categoryDto.Name };
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, categoryDto);
        }
        
        // POST: api/Food/food
        [HttpPost("food")]
        public async Task<ActionResult<FoodsResultDTO>> AddFood(FoodDTO foodDto)
        {
            Foods foods = new Foods() { Name = foodDto.Name, Price = foodDto.Price, Category = await _context.Category.FindAsync(foodDto.Category) };
            FoodsResultDTO result = new FoodsResultDTO() { Category = foods.Category.Name, Name = foods.Name, Price = foods.Price };
            _context.Foods.Add(foods);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = foods.Id }, result);
        }

        // DELETE: api/Food/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
