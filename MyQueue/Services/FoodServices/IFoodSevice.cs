using Microsoft.AspNetCore.Mvc;
using MyQueue.DataTansferObject.FoodManipulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQueue.Services.FoodServices
{
    interface IFoodSevice
    {
        public Task<IEnumerable<CategoryDTO>> GetCategory();
        public Task<IEnumerable<FoodsResultDTO>> GetFood();
        public Task<CategoryDTO> GetCategory(int id);
        public Task<FoodsResultDTO> GetFood(int id);
        public Task<bool> PutCategory(int id, CategoryDTO category);
        public Task<bool> PutFood(int id, FoodDTO foodDTO);
        public Task<CategoryDTO> AddCategory(CategoryDTO categoryDto);
        public Task<FoodsResultDTO> AddFood(FoodDTO foodDto);
        public Task<CategoryDTO> DeleteCategory(int id);
        public Task<FoodsResultDTO> DeleteFood(int id);

    }
}
