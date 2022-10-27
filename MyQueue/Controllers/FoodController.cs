using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        
        // GET: api/Food/category
        [HttpGet("category")]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategory()
        {
            try
            {
                var result = from res in _context.Category.AsNoTracking()
                             select new CategoryDTO
                             {
                                 Id = res.Id,
                                 Name = res.Name
                             };
                return await result.ToListAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            
        }

        // GET: api/Food/food
        [HttpGet("food")]
        public async Task<ActionResult<IEnumerable<FoodsResultDTO>>> GetFood()
        {
            try
            {
                var result = from res in _context.Foods.Include(c => c.Category).AsNoTracking()
                             select new FoodsResultDTO
                             {
                                 Id = res.Id,
                                 Name = res.Name,
                                 Category = res.Category.Name,
                                 Price = res.Price
                             };
                return await result.ToListAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // GET: api/Food/category/5
        [HttpGet("category/{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            try
            {
                var category = await _context.Category.FindAsync(id);   
                if (category == null)
                {
                    return NotFound();
                }
                var result = new CategoryDTO { Id = category.Id, Name = category.Name };
                return result;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            
        }

        // GET: api/Food/food/5
        [HttpGet("food/{id}")]
        public async Task<ActionResult<FoodsResultDTO>> GetFood(int id)
        {
            try
            {
                var food = await _context.Foods.Include(c => c.Category).Where(f => f.Id == id).AsNoTracking().FirstOrDefaultAsync();   
                if (food == null)
                {
                    return NotFound();
                }
                var result = new FoodsResultDTO { Id = food.Id, Name = food.Name, Category = food.Category.Name, Price = food.Price };
                return result;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        
        // PUT: api/Food/category/5
        [HttpPut("category/{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryDTO category)
        {
            if (category.Id != id)
                return BadRequest();

            var result = new Category() { Id = category.Id, Name = category.Name};

            _context.Entry(result).State = EntityState.Modified;
           
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
        
        // PUT: api/Food/food/5
        [HttpPut("food/{id}")]
        public async Task<IActionResult> PutFood(int id, FoodDTO foodDTO)
        {
            if (id != foodDTO.Id)
            {
                return BadRequest();
            }
            var result = new Foods() { Id = foodDTO.Id, Name = foodDTO.Name, Price = foodDTO.Price };
            _context.Entry(result).State = EntityState.Modified;

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

            return CreatedAtAction("GetCategory", 
                new { id = category.Id }, 
                new CategoryDTO { Id = category.Id, Name = category.Name });
        }
        
        // POST: api/Food/food
        [HttpPost("food")]
        public async Task<ActionResult<FoodsResultDTO>> AddFood(FoodDTO foodDto)
        {
            Foods foods = new Foods() { Name = foodDto.Name, Price = foodDto.Price, Category = await _context.Category.FindAsync(foodDto.Category) };
            _context.Foods.Add(foods);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", 
                new { id = foods.Id }, 
                new FoodsResultDTO() { Id = foods.Id, Category = foods.Category.Name, Name = foods.Name, Price = foods.Price });
        }

        // DELETE: api/Food/category/5
        [HttpDelete("category/{id}")]
        public async Task<ActionResult<CategoryDTO>> DeleteCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return new CategoryDTO() { Id = category.Id, Name = category.Name } ;
        }

        // DELETE: api/Food/food/5
        [HttpDelete("food/{id}")]
        public async Task<ActionResult<FoodsResultDTO>> DeleteFood(int id)
        {
            var food = await _context.Foods.Include(c => c.Category).Where(f => f.Id == id).AsNoTracking().FirstOrDefaultAsync();
            if (food == null)
            {
                return NotFound();
            }

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return new FoodsResultDTO() { Id = food.Id, Category = food.Category.Name, Name = food.Name, Price = food.Price };
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
