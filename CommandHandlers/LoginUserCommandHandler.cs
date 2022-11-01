using Command.UAM;
using Contract;

using MediatR;

namespace CommandHandlers
{
    public class LoginUserCommandHandler : AsyncRequestHandler<LoginUserCommand>
    {
        private IUserManagerServices _userManagerServices;
        public LoginUserCommandHandler(IUserManagerServices userManagerServices)
        {
            _userManagerServices = userManagerServices;
        }
        protected override Task Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            await _userManagerServices.Login(request.UserName, request.Password);
           
            return Task.CompletedTask;
        }
    }
}
