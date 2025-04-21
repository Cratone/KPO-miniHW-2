using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.ValueObjects;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class EnclosureRepository : IEnclosureRepository
    {
        private readonly InMemoryDatabase _database;

        public EnclosureRepository()
        {
            _database = InMemoryDatabase.Instance;
        }

        public Task<Enclosure?> GetByIdAsync(Guid id)
        {
            var enclosure = _database.Enclosures.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(enclosure);
        }

        public Task<IEnumerable<Enclosure>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Enclosure>>(_database.Enclosures.ToList());
        }

        public Task<IEnumerable<Enclosure>> GetByTypeAsync(AnimalTypeValueObject type)
        {
            var enclosures = _database.Enclosures
                .Where(e => e.AnimalType == type)
                .ToList();
            return Task.FromResult<IEnumerable<Enclosure>>(enclosures);
        }

        public Task<IEnumerable<Enclosure>> GetAvailableEnclosuresAsync()
        {
            var enclosures = _database.Enclosures
                .Where(e => e.AnimalIds.Count < e.MaxCapacity)
                .ToList();
            return Task.FromResult<IEnumerable<Enclosure>>(enclosures);
        }

        public Task AddAsync(Enclosure enclosure)
        {
            if (!_database.Enclosures.Any(e => e.Id == enclosure.Id))
            {
                _database.Enclosures.Add(enclosure);
            }
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Enclosure enclosure)
        {
            var existingEnclosure = _database.Enclosures.FirstOrDefault(e => e.Id == enclosure.Id);
            if (existingEnclosure != null)
            {
                _database.Enclosures.Remove(existingEnclosure);
                _database.Enclosures.Add(enclosure);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var enclosure = _database.Enclosures.FirstOrDefault(e => e.Id == id);
            if (enclosure != null)
            {
                _database.Enclosures.Remove(enclosure);
            }
            return Task.CompletedTask;
        }
    }
} 