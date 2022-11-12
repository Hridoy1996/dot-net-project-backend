using Commands.SMS;
using Commands.UAM;
using Contract;
using Infrastructure.Core.HelperService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Queries.UAM;
using Shared.Models;
using System.Net;
using System.Security.Claims;

namespace TeleMedicine_WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AppCatalogueController : ControllerBase
    {
        private readonly ILogger<AppCatalogueController> _logger;
        private readonly IMediator _mediator;

        private readonly IUserPermissionManager _userPermissionManager;

        public AppCatalogueController(ILogger<AppCatalogueController> logger,
            IMediator mediator,
            IUserPermissionManager userPermissionManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userPermissionManager = userPermissionManager;
        }


        [HttpGet]
        [Authorize]
        public CommonResponseModel GetApps()
        {
            try
            {
                var rolesString = User.FindFirstValue("Roles");
                var roles = DataConversions.GetRoles(rolesString);
                
                var rolePermissions = _userPermissionManager.GetUserFeatureRolePermissions(roles);

                return new CommonResponseModel { IsSucceed = true, StatusCode = (int)HttpStatusCode.OK, ResponseData = rolePermissions };
            }
            catch(Exception ex)
            {
                return new CommonResponseModel { IsSucceed = true, StatusCode = (int)HttpStatusCode.OK, ResponseMessage = "Server Error" };
            }
        }


    }
}


   
   

