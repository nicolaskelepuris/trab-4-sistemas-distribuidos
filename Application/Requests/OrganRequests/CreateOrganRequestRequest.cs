using Domain.Enums;

namespace Application.Requests.OrganRequests
{
    public class CreateOrganRequestRequest
    {
        public string? Name { get; set; }
        public string Cpf { get; set; } = null!;
        public RequestType Type { get; set; }
        public string Organ { get; set; } = null!;
    }
}