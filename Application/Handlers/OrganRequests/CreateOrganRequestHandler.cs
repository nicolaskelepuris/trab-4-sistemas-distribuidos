using System;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Application.Requests.OrganRequests;
using Application.Responses.OrganRequests;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Handlers.OrganRequests
{
    public class CreateOrganRequestHandler : IHandler<CreateOrganRequestRequest, CreateOrganRequestResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICpfValidator _cpfValidator;
        private readonly IBlockchain _blockchain;
        private readonly IDateTimeProvider _dateTimeProvider;
        public CreateOrganRequestHandler(IUnitOfWork unitOfWork, ICpfValidator cpfValidator, IBlockchain blockchain, IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _blockchain = blockchain;
            _cpfValidator = cpfValidator;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateOrganRequestResponse> HandleAsync(CreateOrganRequestRequest request)
        {
            Validate(request);

            var user = await GetUserByCpfAsync(request.Cpf);
            if (user == null)
            {
                user = CreateUser(request);
            }

            var organRequest = CreateOrganRequest(request, user);
            _unitOfWork.Repository<OrganRequest>().Add(organRequest);

            var matchingOrganRequest = await GetMatchingOrganRequestAsync(request);
            if (matchingOrganRequest != null)
            {
                await RegisterOrganTransactionAsync(organRequest, matchingOrganRequest);
            }

            await _unitOfWork.Complete();
            return new CreateOrganRequestResponse();
        }

        private void Validate(CreateOrganRequestRequest request)
        {
            if (!_cpfValidator.IsValid(request?.Cpf)) throw ApiException.BadRequest(nameof(request.Cpf));
            if (string.IsNullOrWhiteSpace(request?.Organ)) throw ApiException.BadRequest(nameof(request.Organ));
            if (!Enum.IsDefined(typeof(RequestType), request.Type)) throw ApiException.BadRequest(nameof(request.Type));
        }

        private async Task<AppUser?> GetUserByCpfAsync(string cpf)
        {
            return await _unitOfWork.Repository<AppUser>().GetEntityAsyncWithSpec(new Specification<AppUser>(user => user.Cpf == cpf));
        }

        private AppUser CreateUser(CreateOrganRequestRequest request)
        {
            return new AppUser
            {
                Name = request.Name,
                Cpf = request.Cpf,
            };
        }

        private OrganRequest CreateOrganRequest(CreateOrganRequestRequest request, AppUser user)
        {
            return new OrganRequest
            {
                Requester = user,
                Type = request.Type,
                Organ = request.Organ
            };
        }

        private async Task<OrganRequest?> GetMatchingOrganRequestAsync(CreateOrganRequestRequest request)
        {
            var matchingType = request.Type == RequestType.Give ? RequestType.Receive : RequestType.Give;
            var specification = new Specification<OrganRequest>(o => o.Organ == request.Organ && o.CompletedAt == null && o.Type == matchingType);
            specification.AddInclude(x => x.Requester);
            specification.AddOrderBy(x => x.CreatedAt);

            return await _unitOfWork.Repository<OrganRequest>().GetEntityAsyncWithSpec(specification);
        }

        private async Task RegisterOrganTransactionAsync(OrganRequest organRequest, OrganRequest matchingOrganRequest)
        {
            try
            {
                var transaction = await _blockchain.CreateTransaction(organRequest, matchingOrganRequest);
                _unitOfWork.Repository<Transaction>().Add(transaction);
                SetCompleted(organRequest, matchingOrganRequest);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        }

        private void SetCompleted(OrganRequest organRequest, OrganRequest matchingOrganRequest)
        {
            var completedAt = _dateTimeProvider.UtcNow;
            organRequest.CompletedAt = completedAt;
            matchingOrganRequest.CompletedAt = completedAt;
        }
    }
}