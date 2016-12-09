using Microsoft.AspNet.Identity;
using PhongTro.Domain.Entities;
using PhongTro.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhongTro.Model.Core
{
    /// <summary>
    /// The interface defines methods to communicate with database
    /// </summary>
    public interface IRepository
    {
        #region User

        /// <summary>
        /// Get all users from database
        /// </summary>
        /// <returns>List of User in form of UserDTO</returns>
        IEnumerable<UserDTO> GetAllUsers();

        /// <summary>
        /// Find a specified user with her identifier
        /// </summary>
        /// <param name="id">Identifier of the user</param>
        /// <returns>A UserDTO object</returns>
        Task<UserDTO> FindUserById(string id);

        /// <summary>
        /// Find a specified user with her username
        /// </summary>
        /// <param name="username">User name of user to be searched</param>
        /// <returns>A UserDTO object</returns>
        Task<UserDTO> FindUserByUserName(string username);

        /// <summary>
        /// Create a user by receiving information from a RegisteringUserDTO object
        /// </summary>
        /// <param name="registeringUser">Object contains user information</param>
        /// <returns>
        /// A IdentityResult object stores result value
        /// A UserDTO object is the newly created user had been converted
        /// </returns>
        Task<Tuple<IdentityResult, UserDTO>> CreateUser(RegisteringUserDTO registeringUser);

        /// <summary>
        /// Method is used to change password of a user
        /// </summary>
        /// <param name="id">The user's identifier</param>
        /// <param name="model">Object contains old password and the new one</param>
        /// <returns>The IdentityResult object indicates the operation result</returns>
        Task<IdentityResult> ChangePassword(string id, ChangingPasswordDTO model);

        /// <summary>
        /// Delete a user by specifying her identifier
        /// </summary>
        /// <param name="id">The identifier of user to be deleted</param>
        /// <returns>The IdentityResult object indicates the operation result</returns>
        Task<IdentityResult> DeleteUser(string id);

        /// <summary>
        /// Get roles given in the param that not exists in Role system
        /// </summary>
        /// <param name="rolesToAssign">Roles to be checked</param>
        /// <returns>IEnumrable roles's name</returns>
        IEnumerable<string> GetRolesNotExist(string[] roles);

        /// <summary>
        /// Remove all roles of a user
        /// </summary>
        /// <param name="userId">The identifier of user</param>
        /// <returns>The IdentityResult object indicates the operation result</returns>
        Task<IdentityResult> RemoveAllRoles(string userId);

        /// <summary>
        /// Assign roles to a user
        /// </summary>
        /// <param name="userId">The identifier of user</param>
        /// <param name="rolesToAssign">Roles to be added</param>
        /// <returns>The IdentityResult object indicates the operation result</returns>
        Task<IdentityResult> AddRolesToUser(string userId, string[] rolesToAssign);

        #endregion

        #region Roles

        /// <summary>
        /// Get a role by its identifier
        /// </summary>
        /// <param name="id">The role's identifier</param>
        /// <returns></returns>
        Task<RoleDTO> FindRole(string id);

        /// <summary>
        /// Get all roles in the membership system
        /// </summary>
        /// <returns>List RoleDTO objects</returns>
        IEnumerable<RoleDTO> GetAllRoles();

        /// <summary>
        /// Create a new role 
        /// </summary>
        /// <param name="model">Object contains new role data</param>
        /// <returns>
        /// An IdentityResult indicates operation's result. 
        /// A RoleDTO object hold a reference to the newly created role
        /// </returns>
        Task<Tuple<IdentityResult, RoleDTO>> CreateRole(CreatingRoleDTO model);

        /// <summary>
        /// Delete a role by specifying its identifier
        /// </summary>
        /// <param name="id">Identifier of the role to be deleted</param>
        /// <returns>An IdentifyResult object indicates operation's result</returns>
        Task<IdentityResult> DeleteRole(string id);

        #endregion
    }
}
