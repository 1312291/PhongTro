using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhongTro.Model.DTOs;
using PhongTro.Domain.Infracstucture;
using PhongTro.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PhongTro.Model.Core
{
    /// <summary>
    /// Class implements IRepository
    /// </summary>
    public class PhongTroRepository : IRepository
    {
        private PhongTroUserManager _userManager;
        private PhongTroRoleManager _roleManager;
        private ModelFactory _modelFactory;

        /// <summary>
        /// Default constructor with dependencies which will be inject later
        /// </summary>
        /// <param name="userManager">The application's User Manager</param>
        /// <param name="roleManager">The application's Role Manager</param>
        public PhongTroRepository(PhongTroUserManager userManager, PhongTroRoleManager roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _modelFactory = new ModelFactory(_userManager);
        }

        #region User

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return _userManager.Users.ToList().Select(u => _modelFactory.ConvertFromAppUser(u));
        }

        public async Task<UserDTO> FindUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return null == user ? null : _modelFactory.ConvertFromAppUser(user);
        }

        public async Task<UserDTO> FindUserByUserName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            return null == user ? null : _modelFactory.ConvertFromAppUser(user);
        }

        public async Task<Tuple<IdentityResult, UserDTO>> CreateUser(RegisteringUserDTO registeringUser)
        {
            var user = _modelFactory.ConvertToAppUser(registeringUser);
            
            IdentityResult identityResult = await _userManager.CreateAsync(user, registeringUser.Password);

            var userResult = new UserDTO();

            if (identityResult.Succeeded)
            {
                 userResult = _modelFactory.ConvertFromAppUser(user);
            }
            
            return new Tuple<IdentityResult, UserDTO>(identityResult, userResult);
        }

        public async Task<IdentityResult> ChangePassword(string id, ChangingPasswordDTO model)
        {
            return await _userManager.ChangePasswordAsync(id, model.OldPassword, model.NewPassword);
        }

        public async Task<IdentityResult> DeleteUser(string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);

            return await _userManager.DeleteAsync(appUser);
        }

        public IEnumerable<string> GetRolesNotExist(string[] roles)
        {
            return roles.Except(_roleManager.Roles.Select(x => x.Name)).ToArray();
        }

        public async Task<IdentityResult> RemoveAllRoles(string userId)
        {
            var currentRoles = await _userManager.GetRolesAsync(userId);
            return await _userManager.RemoveFromRolesAsync(userId, currentRoles.ToArray());
        }

        public async Task<IdentityResult> AddRolesToUser(string userId, string[] rolesToAssign)
        {
            return await _userManager.AddToRolesAsync(userId, rolesToAssign);
        }


        #endregion

        #region Role

        public async Task<RoleDTO> FindRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            
            return role == null? null : _modelFactory.ConvertFromIdentityRole(role);
        }

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            var roles = _roleManager.Roles;

            List<RoleDTO> results = new List<RoleDTO>();

            foreach(var role in roles)
            {
                results.Add(_modelFactory.ConvertFromIdentityRole(role));
            }

            return results;
        }

        public async Task<Tuple<IdentityResult, RoleDTO>> CreateRole(CreatingRoleDTO model)
        {
            var role = new IdentityRole { Name = model.Name };
            var result = await _roleManager.CreateAsync(role);

            return new Tuple<IdentityResult, RoleDTO>(result, _modelFactory.ConvertFromIdentityRole(role));
        }

        public async Task<IdentityResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return await _roleManager.DeleteAsync(role);
        }

        #endregion
    }
}
