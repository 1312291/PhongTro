using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PhongTro.Domain.Infracstucture;
using PhongTro.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PhongTro.WebApi.Controllers
{
    /// <summary>
    /// Class allows the administrator to manage roles in membership system
    /// </summary>
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/roles")]
    public class RolesController : BaseApiController
    {

        /// <summary>
        /// Get a role by its identifier
        /// </summary>
        /// <param name="Id">The identifier of role</param>
        /// <returns>A RoleDTO object</returns>
        [Route("{id:guid}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(string Id)
        {
            var role = await _AppRoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                return Ok(_ModelFactory.ConvertFromIdentityRole(role));
            }

            return NotFound();

        }

        /// <summary>
        /// Get all role in membership system
        /// </summary>
        /// <returns>
        /// An IdentityRole list
        /// </returns>
        [Route("", Name = "GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = _AppRoleManager.Roles;

            return Ok(roles);
        }

        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="model">A CreatingRole model</param>
        /// <returns></returns>
        [Route("create")]
        public async Task<IHttpActionResult> Create(CreatingRoleDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = new IdentityRole { Name = model.Name };

            var result = await _AppRoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            Uri locationHeader = new Uri(Url.Link("GetRoleById", new { id = role.Id }));

            return Created(locationHeader, _ModelFactory.ConvertFromIdentityRole(role));

        }

        /// <summary>
        /// Delete a role by supplying its identifier
        /// </summary>
        /// <param name="Id">The role's identifier</param>
        /// <returns>
        /// Success: OkResult
        /// Fail: BadRequestResult comes with Error content
        /// </returns>
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> DeleteRole(string Id)
        {

            var role = await _AppRoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                IdentityResult result = await _AppRoleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }

            return NotFound();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[Route("ManageUsersInRole")]
        //public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleModel model)
        //{
        //    var role = await _AppRoleManager.FindByIdAsync(model.Id);

        //    if (role == null)
        //    {
        //        ModelState.AddModelError("", "Role does not exist");
        //        return BadRequest(ModelState);
        //    }

        //    foreach (string user in model.EnrolledUsers)
        //    {
        //        var appUser = await _AppUserManager.FindByIdAsync(user);

        //        if (appUser == null)
        //        {
        //            ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
        //            continue;
        //        }

        //        if (!_AppUserManager.IsInRole(user, role.Name))
        //        {
        //            IdentityResult result = await _AppUserManager.AddToRoleAsync(user, role.Name);

        //            if (!result.Succeeded)
        //            {
        //                ModelState.AddModelError("", String.Format("User: {0} could not be added to role", user));
        //            }

        //        }
        //    }

        //    foreach (string user in model.RemovedUsers)
        //    {
        //        var appUser = await _AppUserManager.FindByIdAsync(user);

        //        if (appUser == null)
        //        {
        //            ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
        //            continue;
        //        }

        //        IdentityResult result = await _AppUserManager.RemoveFromRoleAsync(user, role.Name);

        //        if (!result.Succeeded)
        //        {
        //            ModelState.AddModelError("", String.Format("User: {0} could not be removed from role", user));
        //        }
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    return Ok();
        //}
    }
}