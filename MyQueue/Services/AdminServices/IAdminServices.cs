using MyQueue.DataTansferObject.Admin;
using MyQueue.DataTansferObject.FoodManipulation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyQueue.Services.AdminServices
{
    interface IAdminServices
    {
        public Task<RoleDTO> AddRole(RoleDTO role);
        public Task<RoleDTO> DeleteRole(RoleDTO role);
        public Task<IEnumerable<RoleDTO>> GetRole();
        public Task<IEnumerable<UsersDTO>> GetUsers();
        public Task<UsersDTO> DeleteUser(IDToDelete delete);
        public Task<UserRoleDTO> DeleteUserRole(UsersRole delete);
        public Task<UserRoleDTO> AddUserRole(UsersRole add);
    }
}
