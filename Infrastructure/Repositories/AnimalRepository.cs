using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly InMemoryDatabase _database;

        public AnimalRepository()
        {
            _database = InMemoryDatabase.Instance;
        }

        public Task<Animal?> GetByIdAsync(Guid id)
        {
            var animal = _database.Animals.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(animal);
        }

        public Task<IEnumerable<Animal>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Animal>>(_database.Animals.ToList());
        }

        public Task<IEnumerable<Animal>> GetByEnclosureIdAsync(Guid enclosureId)
        {
            var animals = _database.Animals
                .Where(a => a.EnclosureId == enclosureId)
                .ToList();
            return Task.FromResult<IEnumerable<Animal>>(animals);
        }

        public Task AddAsync(Animal animal)
        {
            if (!_database.Animals.Any(a => a.Id == animal.Id))
            {
                _database.Animals.Add(animal);
            }
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Animal animal)
        {
            var existingAnimal = _database.Animals.FirstOrDefault(a => a.Id == animal.Id);
            if (existingAnimal != null)
            {
                _database.Animals.Remove(existingAnimal);
                _database.Animals.Add(animal);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var animal = _database.Animals.FirstOrDefault(a => a.Id == id);
            if (animal != null)
            {
                _database.Animals.Remove(animal);
            }
            return Task.CompletedTask;
        }
    }
} 