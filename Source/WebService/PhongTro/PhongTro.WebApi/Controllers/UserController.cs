using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PhongTro.Domain.Entities;
using PhongTro.Domain.Infracstucture;
using PhongTro.Model;
using PhongTro.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PhongTro.WebApi.Controllers
{
    /// <summary>
    /// Controller is responsible for manage App's accounts
    /// </summary>
    [RoutePrefix("api")]
    public class UserController : BaseApiController
    {

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>
        /// Success: OkResult with a list of all users in form of RegisteringUserDTO.
        /// Fail: NotFoundResult.
        /// </returns>
        [Authorize(Roles = "Admin")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(_AppUserManager.Users.ToList().Select(u => _ModelFactory.ConvertFromAppUser(u)));
        }

        /// <summary>
        /// Get User by Identifier filter 
        /// </summary>
        /// <param name="Id">The identifier</param>
        /// <returns>
        /// IHttpActionResult (contains User in form of UserDTO if it is found)
        /// </returns>
        [Authorize(Roles = "Admin")]
        [Route("users/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await _AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(_ModelFactory.ConvertFromAppUser(user));
            }

            return NotFound();

        }

        /// <summary>
        /// Get user data by a username
        /// </summary>
        /// <param name="username">Username of the user to be found</param>
        /// <returns>
        /// IHttpResult (contains User in form of UserDTO if it is found)
        /// </returns>
        [Authorize(Roles= "Admin")]
        [Route("users/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await _AppUserManager.FindByNameAsync(username);
            //var user = await _Repository.GetUserByUserName(username);

            if (user != null)
            {
                return Ok(_ModelFactory.ConvertFromAppUser(user));
            }

            return NotFound();

        }
        
        /// <summary>
        /// Action used to create new user
        /// </summary>
        /// <param name="registeringUser">Param contains user information for registration</param>
        /// <returns>An IHttpActionResult comes with user info in form of UserDTO</returns>
        [Route("users")]
        public async Task<IHttpActionResult> CreateUser(RegisteringUserDTO registeringUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new PhongTroUser()
            {
                UserName = registeringUser.Username,
                Email = registeringUser.Email,
                FirstName = registeringUser.FirstName,
                LastName = registeringUser.LastName,
                DateOfBirth = registeringUser.DateOfBirth,
                PhoneNumber = registeringUser.Phone,
            };

            IdentityResult addUserResult = await _AppUserManager.CreateAsync(user, registeringUser.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return Created(locationHeader, _ModelFactory.ConvertFromAppUser(user));
        }

        /// <summary>
        /// Action used by authentication user to change her password
        /// </summary>
        /// <param name="model">Param contains old password and the new one</param>
        /// <returns>
        /// An IHttpActionResult object
        /// </returns>
        [Authorize]
        [Route("users/changepassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangingPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// Action is used to delete a user
        /// </summary>
        /// <param name="id">Identifier of user</param>
        /// <returns>
        /// IHttpActionResult object
        /// </returns>
        [Authorize(Roles="Admin")]
        [Route("users/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            var appUser = await _AppUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await _AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }
        
        /// <summary>
        /// Action is used to assign roles to a specific user
        /// </summary>
        /// <param name="id">Identifier of the user</param>
        /// <param name="rolesToAssign">Roles to be assigned to the user</param>
        /// <returns>
        /// IHttpActionResult object
        /// </returns>
        [Authorize(Roles = "Admin")]
        [Route("users/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await _AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await _AppUserManager.GetRolesAsync(appUser.Id);

            // Check if roles which will be assigned are exist in membership system.
            // If they does, return BadRequestResult.
            var rolesNotExists = rolesToAssign.Except(_AppRoleManager.Roles.Select(x => x.Name)).ToArray();
            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            // Remove all roles assigned to the user before
            IdentityResult removeResult = await _AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            // Add new roles to the user
            IdentityResult addResult = await _AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
