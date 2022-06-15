using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request);
}

public class Handler<TRequest, TResponse> : IHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request)
    {
        throw new System.NotImplementedException();
    }
}
