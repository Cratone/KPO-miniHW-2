using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IAnimalRepository
    {
        Task<Animal?> GetByIdAsync(Guid id);
        Task<IEnumerable<Animal>> GetAllAsync();
        Task<IEnumerable<Animal>> GetByEnclosureIdAsync(Guid enclosureId);
        Task AddAsync(Animal animal);
        Task UpdateAsync(Animal animal);
        Task DeleteAsync(Guid id);
    }
} 