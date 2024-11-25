using api.Models;
using System.Threading.Tasks;

public interface ILabService
{
    Task<Lab> GetLabAsync();
}
