using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces;
public interface IContextHelper
{
    string GetUserId();
    Task<AppUser> GetUserAsync();
}
