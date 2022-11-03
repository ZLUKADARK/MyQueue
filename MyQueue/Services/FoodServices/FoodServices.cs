using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyQueue.Data;
using MyQueue.Data.Models;
using MyQueue.DataTansferObject.FoodManipulation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQueue.Services.FoodServices
{
    public class FoodServices : IFoodSevice
    {
        private readonly MQDBContext _context;
        public FoodServices(MQDBContext context)
        {
            _context = context;
        }

        public async Task<CategoryDTO> AddCategory(CategoryDTO categoryDto)
        {
            Category category = new Category() { Name = categoryDto.Name };
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDTO { Id = category.Id, Name = category.Name };
        }

        public async Task<FoodsResultDTO> AddFood(FoodDTO foodDto)
        {
            Foods foods = new Foods() { Name = foodDto.Name, Price = foodDto.Price, Category = await _context.Category.FindAsync(foodDto.Category), Active = foodDto.Active };
            _context.Foods.Add(foods);
            await _context.SaveChangesAsync();

            return new FoodsResultDTO() { Id = foods.Id, Category = foods.Category.Name, Name = foods.Name, Price = foods.Price, Active = foods.Active.Value };
        }

        public async Task<CategoryDTO> DeleteCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
                return null;

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return new CategoryDTO() { Id = category.Id, Name = category.Name };
        }

        public async Task<FoodsResultDTO> DeleteFood(int id)
        {
            var food = await _context.Foods.Include(c => c.Category).Where(f => f.Id == id).AsNoTracking().FirstOrDefaultAsync();
            if (food == null)
                return null;

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return new FoodsResultDTO() { Id = food.Id, Category = food.Category.Name, Name = food.Name, Price = food.Price };
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategory()
        {
            var result = from res in _context.Category.AsNoTracking()
                         select new CategoryDTO
                         {
                             Id = res.Id,
                             Name = res.Name
                         };
            return await result.ToListAsync();
        }

        public async Task<CategoryDTO> GetCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
                return null;
            return new CategoryDTO { Id = category.Id, Name = category.Name };
        }

        public async Task<IEnumerable<FoodsResultDTO>> GetFood()
        {
            var result = from res in _context.Foods.Include(c => c.Category).AsNoTracking()
                         select new FoodsResultDTO
                         {
                             Id = res.Id,
                             Name = res.Name,
                             Category = res.Category.Name,
                             Price = res.Price,
                             Active = res.Active
                         };
            return await result.ToListAsync();
        }

        public async Task<FoodsResultDTO> GetFood(int id)
        {
            var food = await _context.Foods.Include(c => c.Category).Where(f => f.Id == id).AsNoTracking().FirstOrDefaultAsync();
            if (food == null)
                return null;
            return new FoodsResultDTO { Id = food.Id, Name = food.Name, Category = food.Category.Name, Price = food.Price, Active = food.Active };
        }

        public async Task<bool> PutCategory(int id, CategoryDTO category)
        {
            if (category.Id != id)
                return false;

            var result = new Category() { Id = category.Id, Name = category.Name };

            _context.Entry(result).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                    return false;
                else
                    throw;
            }
        }

        public async Task<bool> PutFood(int id, FoodDTO foodDTO)
        {
            if (id != foodDTO.Id)
                return false;
            var result = new Foods() { Id = foodDTO.Id, Name = foodDTO.Name, Price = foodDTO.Price, CategoryId = foodDTO.Category, Active = foodDTO.Active };
            _context.Entry(result).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
                    return false;
                else
                    throw;
            }
        }

        public async Task<bool> ChangeFoodActivity(int id, bool activity)
        {
            var result = await _context.Foods.FindAsync(id);
            result.Active = activity;
            _context.Entry(result).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
                    return false;
                else
                    throw;
            }
        }
        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }

        private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.Id == id);
        }
    }
}
