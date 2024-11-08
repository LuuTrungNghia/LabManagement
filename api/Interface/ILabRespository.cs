using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Helper;
using api.Models;
using api.Dtos.Lab;

namespace api.Interface
{
    public interface ILabRepository
    {
        Task<List<Lab>> GetAllAsync();
        Task<Lab> GetByIdAsync(int id);
        Task CreateAsync(Lab lab);
        Task UpdateAsync(Lab lab);
        Task DeleteAsync(int id);
    }
}
