using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class FeedingScheduleRepository : IFeedingScheduleRepository
    {
        private readonly InMemoryDatabase _database;

        public FeedingScheduleRepository()
        {
            _database = InMemoryDatabase.Instance;
        }

        public Task<FeedingSchedule?> GetByIdAsync(Guid id)
        {
            var schedule = _database.FeedingSchedules.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(schedule);
        }

        public Task<IEnumerable<FeedingSchedule>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<FeedingSchedule>>(_database.FeedingSchedules.ToList());
        }

        public Task<IEnumerable<FeedingSchedule>> GetByAnimalIdAsync(Guid animalId)
        {
            var schedules = _database.FeedingSchedules
                .Where(s => s.AnimalId == animalId)
                .ToList();
            return Task.FromResult<IEnumerable<FeedingSchedule>>(schedules);
        }

        public Task<IEnumerable<FeedingSchedule>> GetSchedulesForTimeRangeAsync(TimeOnly start, TimeOnly end)
        {
            var schedules = _database.FeedingSchedules
                .Where(s => s.Time >= start && s.Time <= end)
                .ToList();
            return Task.FromResult<IEnumerable<FeedingSchedule>>(schedules);
        }

        public Task<IEnumerable<FeedingSchedule>> GetPerformedFeedingSchedulesAsync()
        {
            var schedules = _database.FeedingSchedules
                .Where(s => s.Performed)
                .ToList();
            return Task.FromResult<IEnumerable<FeedingSchedule>>(schedules);
        }

        public Task AddAsync(FeedingSchedule schedule)
        {
            if (!_database.FeedingSchedules.Any(s => s.Id == schedule.Id))
            {
                _database.FeedingSchedules.Add(schedule);
            }
            return Task.CompletedTask;
        }

        public Task UpdateAsync(FeedingSchedule schedule)
        {
            var existingSchedule = _database.FeedingSchedules.FirstOrDefault(s => s.Id == schedule.Id);
            if (existingSchedule != null)
            {
                _database.FeedingSchedules.Remove(existingSchedule);
                _database.FeedingSchedules.Add(schedule);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var schedule = _database.FeedingSchedules.FirstOrDefault(s => s.Id == id);
            if (schedule != null)
            {
                _database.FeedingSchedules.Remove(schedule);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<FeedingSchedule>> GetFeedingSchedulesByAnimalAsync(Guid animalId)
        {
            return Task.FromResult<IEnumerable<FeedingSchedule>>(_database.FeedingSchedules.Where(s => s.AnimalId == animalId).ToList());
        }
        
        public Task<IEnumerable<FeedingSchedule>> GetFeedingSchedulesByTimeRangeAsync(TimeOnly startTime, TimeOnly endTime)
        {
            return Task.FromResult<IEnumerable<FeedingSchedule>>(_database.FeedingSchedules.Where(s => s.Time >= startTime && s.Time <= endTime).ToList());
        }
    }
} 