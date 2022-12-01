using RoleBasedBasicAuthenticationWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace RoleBasedBasicAuthenticationWebAPI.Controllers
{
    public class EmployeeController : ApiController
    {
        [BasicAuthentication]
        [MyAuthorize(Roles="Admin")]
        [Route("api/AllMaleEmployees")]
        public HttpResponseMessage GetAllMaleEmployees()
        {
            var identity=(ClaimsIdentity)User.Identity;
            var ID = identity.Claims.FirstOrDefault(c => c.Type == "ID").Value;
            var Name=identity.Name;
            var Email=identity.Claims.FirstOrDefault(c=>c.Type== "Email").Value;
            //Getting the Roles only if you set the roles in the claims
            //var Roles = identity.Claims
            //            .Where(c => c.Type == ClaimTypes.Role)
            //            .Select(c => c.Value).ToArray();
            var EmpList = new EmployeeBL().GetEmployees().Where(c => c.Gender.ToLower() == "male").ToList();
            return Request.CreateResponse(HttpStatusCode.OK,EmpList);
        }
        [BasicAuthentication]
        [MyAuthorize(Roles ="Superadmin")]
        [Route("api/AllFemaleEmployees")]
        public HttpResponseMessage GetAllFemaleEmployees()
        {
            return Request.CreateResponse(HttpStatusCode.OK,new EmployeeBL().GetEmployees().Where(
                c=>c.Gender.ToLower()=="female").ToList());
        }
        [BasicAuthentication]
        [MyAuthorize(Roles ="Admin,Superadmin")]
        [Route("api/AllEmployees")]
        public HttpResponseMessage GetAllEmployees()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new EmployeeBL().GetEmployees());
        }
    }
}
