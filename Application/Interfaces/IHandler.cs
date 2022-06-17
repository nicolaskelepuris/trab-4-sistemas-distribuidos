using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IHandler<TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request);
}
