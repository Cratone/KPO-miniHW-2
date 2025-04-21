using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IFeedingScheduleRepository
    {
        Task<   FeedingSchedule?> GetByIdAsync(Guid id);
        Task<IEnumerable<FeedingSchedule>> GetAllAsync();
        Task<IEnumerable<FeedingSchedule>> GetByAnimalIdAsync(Guid animalId);
        Task<IEnumerable<FeedingSchedule>> GetSchedulesForTimeRangeAsync(TimeOnly start, TimeOnly end);
        Task<IEnumerable<FeedingSchedule>> GetPerformedFeedingSchedulesAsync();
        Task AddAsync(FeedingSchedule schedule);
        Task UpdateAsync(FeedingSchedule schedule);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<FeedingSchedule>> GetFeedingSchedulesByAnimalAsync(Guid animalId);
        Task<IEnumerable<FeedingSchedule>> GetFeedingSchedulesByTimeRangeAsync(TimeOnly startTime, TimeOnly endTime);
    }
} 