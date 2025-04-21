using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;

namespace Application.Services
{
    public class ZooStatisticsService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IEnclosureRepository _enclosureRepository;
        private readonly IFeedingScheduleRepository _feedingScheduleRepository;

        public ZooStatisticsService(
            IAnimalRepository animalRepository,
            IEnclosureRepository enclosureRepository,
            IFeedingScheduleRepository feedingScheduleRepository)
        {
            _animalRepository = animalRepository;
            _enclosureRepository = enclosureRepository;
            _feedingScheduleRepository = feedingScheduleRepository;
        }

        public async Task<int> GetTotalAnimalCountAsync()
        {
            var animals = await _animalRepository.GetAllAsync();
            return animals.Count();
        }

        public async Task<Dictionary<string, int>> GetCountOfAnimalsByTypesAsync()
        {
            var animals = await _animalRepository.GetAllAsync();
            return animals
                .GroupBy(a => a.Species.AnimalType.ToString())
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<string, int>> GetCountOfAnimalsByHealthStatusAsync()
        {
            var animals = await _animalRepository.GetAllAsync();
            return animals
                .GroupBy(a => a.HealthStatus.ToString())
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<int> GetTotalEnclosuresCountAsync()
        {
            var enclosures = await _enclosureRepository.GetAllAsync();
            return enclosures.Count();
        }

        public async Task<int> GetTotalFeedingSchedulesCountAsync()
        {
            var schedules = await _feedingScheduleRepository.GetAllAsync();
            return schedules.Count();
        }
    }
} 