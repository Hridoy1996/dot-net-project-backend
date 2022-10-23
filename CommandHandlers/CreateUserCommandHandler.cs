

using AutoMapper;
using Commands.UAM;
using Contract;
using Domains.DBModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;


namespace CommandHandler
{
    public class CreateUserCommandHandler : AsyncRequestHandler<CreateUserCommand>
    {
        private readonly IMapper _mapper;
        private readonly IUserManagerServices _userManagerServices;

        public CreateUserCommandHandler(
            IMapper mapper,
            IUserManagerServices userManagerServices)
        {
            _mapper = mapper;
           _userManagerServices = userManagerServices;
        }


        protected override async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            var user = _mapper.Map<TelemedicineAppUser>(request);
            var result = await _userManagerServices.RegisterUserAsync(user, request.Password);
            
            return ;

        }
    }
}