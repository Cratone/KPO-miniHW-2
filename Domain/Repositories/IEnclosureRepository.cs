using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Repositories
{
    public interface IEnclosureRepository
    {
        Task<Enclosure?> GetByIdAsync(Guid id);
        Task<IEnumerable<Enclosure>> GetAllAsync();
        Task<IEnumerable<Enclosure>> GetByTypeAsync(AnimalTypeValueObject type);
        Task<IEnumerable<Enclosure>> GetAvailableEnclosuresAsync();
        Task AddAsync(Enclosure enclosure);
        Task UpdateAsync(Enclosure enclosure);
        Task DeleteAsync(Guid id);
    }
} 