using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyQueue.Data;
using MyQueue.Data.Models;
using MyQueue.DataTansferObject.FoodManipulation;
using MyQueue.Services.FoodServices;

namespace MyQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FoodController : ControllerBase
    {
        private readonly FoodServices _foodServices;
        public FoodController(FoodServices foodServices)
        {
            _foodServices = foodServices;
        }
        
        // GET: api/Food/Category
        [HttpGet("Category")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategory()
        {
            var result = await _foodServices.GetCategory();
            return result.ToList();     
        }

        // GET: api/Food/Food
        [HttpGet("Food")]
        public async Task<ActionResult<IEnumerable<FoodsResultDTO>>> GetFood()
        {
            var result = await _foodServices.GetFood();
            return result.ToList();
        }

        // GET: api/Food/ActiveFood
        [HttpGet("ActiveFood")]
        public async Task<ActionResult<IEnumerable<FoodsResultDTO>>> GetActiveFood()
        {
            var result = await _foodServices.GetActiveFood();
            return result.ToList();
        }

        // GET: api/Food/Category/5
        [HttpGet("Category/{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var result = await _foodServices.GetCategory(id);
            if (result == null)
                return NotFound();
            return result;
        }

        // GET: api/Food/Food/5
        [HttpGet("Food/{id}")]
        public async Task<ActionResult<FoodsResultDTO>> GetFood(int id)
        {
            var result = await _foodServices.GetFood(id);   
            if (result == null)
                return NotFound();
            return result;  
        }
        
        // PUT: api/Food/Category/5
        [HttpPut("Category/{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryDTO category)
        {
            if (category.Id != id)
                return BadRequest();

            var result = await _foodServices.PutCategory(id, category);

            if (result == false)
                return NotFound();

            return Ok("Изменено");
        }
        
        // PUT: api/Food/Food/5
        [HttpPut("Food/{id}")]
        public async Task<IActionResult> PutFood(int id, FoodDTO foodDTO)
        {
            if (foodDTO.Id != id)
                return BadRequest();

            var result = await _foodServices.PutFood(id, foodDTO);

            if (result == false)
                return NotFound();

            return Ok("Изменено");
        }

        // PUT: api/Food/Food/visibility/5
        [HttpPut("Food/activity/{id}/{activity}")]
        public async Task<IActionResult> ChangeFoodActivity(int id, bool activity)
        {
            var result = await _foodServices.ChangeFoodActivity(id, activity);
            
            if (result == false)
                return NotFound();

            return Ok("Изменено");
        }

        // POST: api/Food/Category
        [HttpPost("Category")]
        public async Task<ActionResult<CategoryDTO>> AddCategory(CategoryDTO categoryDto)
        {
            var result = await _foodServices.AddCategory(categoryDto);
            
            return CreatedAtAction("GetCategory",
                new { id = result.Id },
                new CategoryDTO { Id = result.Id, Name = result.Name });
        }
        
        // POST: api/Food/Food
        [HttpPost("Food")]
        public async Task<ActionResult<FoodsResultDTO>> AddFood(FoodDTO foodDto)
        {
            var result = await _foodServices.AddFood(foodDto);
            return CreatedAtAction("GetCategory", 
                new { id = result.Id }, 
                new FoodsResultDTO() { Id = result.Id, Category = result.Category, Name = result.Name, Price = result.Price, Active = result.Active });
        }

        // DELETE: api/Food/Category/5
        [HttpDelete("Category/{id}")]
        public async Task<ActionResult<CategoryDTO>> DeleteCategory(int id)
        {
            var result = await _foodServices.DeleteCategory(id);
            if (result == null)
                return NotFound();
            return result;
        }

        // DELETE: api/Food/food/5
        [HttpDelete("Food/{id}")]
        public async Task<ActionResult<FoodsResultDTO>> DeleteFood(int id)
        {
            var result = await _foodServices.DeleteFood(id);
            if (result == null)
                return NotFound();
            return result;
        }
        
    }
}
