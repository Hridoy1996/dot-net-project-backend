using MediatR;
using Shared.Models;

namespace Commands.Test
{
    public class FeatureRoleMapCreationCommand : IRequest<CommonResponseModel>
    {
        public string? Id { get; set; }
        public string? AppType { get; set; }
        public string? AppName { get; set; }
        public string? FeatureId { get; set; }
        public string? FeatureName { get; set; }
        public string? RoleName { get; set; }
    }
}
