using MyQueue.DataTansferObject.Admin;
using MyQueue.DataTansferObject.FoodManipulation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyQueue.Services.AdminServices
{
    public class AdminServices : IAdminServices
    {
        public Task<RoleDTO> AddRole(RoleDTO role)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserRoleDTO> AddUserRole(UsersRole add)
        {
            throw new System.NotImplementedException();
        }

        public Task<RoleDTO> DeleteRole(RoleDTO role)
        {
            throw new System.NotImplementedException();
        }

        public Task<UsersDTO> DeleteUser(IDToDelete delete)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserRoleDTO> DeleteUserRole(UsersRole delete)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<RoleDTO>> GetRole()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<UsersDTO>> GetUsers()
        {
            throw new System.NotImplementedException();
        }
    }
}
