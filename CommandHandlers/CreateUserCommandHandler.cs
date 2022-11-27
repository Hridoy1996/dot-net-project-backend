﻿using AutoMapper;
using Commands.UAM;
using Contract;
using Domains.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Models;
using System.Net;

namespace CommandHandler
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CommonResponseModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserManagerServices _userManagerServices;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(
            IMapper mapper,
            ILogger<CreateUserCommandHandler> logger,
            IUserManagerServices userManagerServices)
        {
            _logger = logger;
            _mapper = mapper;
            _userManagerServices = userManagerServices;
        }

        public async Task<CommonResponseModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _mapper.Map<TelemedicineAppUser>(request);

                if (user.Roles.Contains("Patinet"))
                {
                    request.Password = "1qazZAQ!";
                }

                var result = await _userManagerServices.RegisterUserAsync(user, request.Password);

                return new CommonResponseModel { IsSucceed = result, StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error in CreateUserCommandHandler \nMessage: {exception.Message} \nStackTrace: {exception.StackTrace}", exception);

                return new CommonResponseModel { IsSucceed = false, StatusCode = (int)HttpStatusCode.InternalServerError, ResponseMessage = "server error" };
            }

        }
    }
}