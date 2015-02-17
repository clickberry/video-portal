using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Portal.BLL.Abstract;
using Portal.Domain.Admin;
using Portal.Domain.PortalContext;
using Portal.Web.Resources;

namespace Portal.Api.Controllers
{
    [Authorize(Roles = DomainRoles.SuperAdministrator)]
    public class AllRolesController : ApiController
    {
        private readonly IService<DomainRoleForAdmin> _roleService;
        private readonly IQueryServiceForAdmin _queryServiceForAdmin;

        public AllRolesController(IService<DomainRoleForAdmin>  roleService, IQueryServiceForAdmin queryServiceForAdmin)
        {
            _roleService = roleService;
            _queryServiceForAdmin = queryServiceForAdmin;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Start()
        {
            try
            {
               var res = await _roleService.GetListAsync(null);
               return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to get roles for admin: {0}", e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ResponseMessages.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> FilterUsers(string startsWith, int take = 0)
        {
            try
            {
                var res = await _queryServiceForAdmin.GetUsersWhoseNameStartsWith(startsWith);
                if (take > 0)
                {
                    res = res.Take(take);
                }
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to get roles for admin: {0}", e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ResponseMessages.InternalServerError);
            }
        }
    }
}
