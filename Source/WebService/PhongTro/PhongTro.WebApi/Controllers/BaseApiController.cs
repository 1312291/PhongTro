using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PhongTro.Domain.Infracstucture;
using PhongTro.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PhongTro.WebApi.Controllers
{
    public class BaseApiController : ApiController
    {
        private ModelFactory _modelFactory;
        private PhongTroUserManager _appUserManager = null;
        private PhongTroRoleManager _appRoleManager = null;
        private IRepository _repository;

        protected IRepository _Repository
        {
            get
            {
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }

        protected PhongTroUserManager _AppUserManager
        {
            get
            {
                return _appUserManager ?? Request.GetOwinContext().GetUserManager<PhongTroUserManager>();
            }
        }

        protected ModelFactory _ModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(_AppUserManager);
                }
                return _modelFactory;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="repo">The implement of IRepository interface</param>
        public BaseApiController(IRepository repo)
        {
            _Repository = repo;
        }

        
        protected PhongTroRoleManager _AppRoleManager
        {
            get
            {
                return _appRoleManager ?? Request.GetOwinContext().GetUserManager<PhongTroRoleManager>();
            }
        }
        
        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}