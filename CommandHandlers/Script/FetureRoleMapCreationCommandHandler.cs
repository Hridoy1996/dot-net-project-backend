using Commands.Test;
using Contract;
using Infrastructure.Core.Services.Test;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Models;
using System.Net;

namespace CommandHandlers.Script
{
    public class FetureRoleMapCreationCommandHandler : IRequestHandler<FeatureRoleMapCreationCommand, CommonResponseModel>
    {
        private readonly IBackendScriptService _backendScriptService;
        private readonly ILogger<FetureRoleMapCreationCommandHandler> _logger;

        public FetureRoleMapCreationCommandHandler(
            IBackendScriptService backendScriptService,
            ILogger<FetureRoleMapCreationCommandHandler> logger)
        {
            _logger = logger;
            _backendScriptService = backendScriptService;
        }

        public async Task<CommonResponseModel> Handle(FeatureRoleMapCreationCommand command, CancellationToken cancellationToken)
        {
            try
            {

                var created = await _backendScriptService.SaveFeatureRoleMaps(command);

                return new CommonResponseModel { IsSucceed = created, StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in FetureRoleMapCreationCommandHandler \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, StatusCode = (int)HttpStatusCode.InternalServerError, ResponseMessage = "server error" };
            }

        }
    }
}