using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Domain.Events;

namespace Application.Services
{
    /// <summary>
    /// Сервис для организации процесса кормления животных
    /// </summary>
    public class FeedingOrganizationService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IFeedingScheduleRepository _feedingScheduleRepository;
        private readonly DomainEventDispatcher _eventDispatcher;

        /// <summary>
        /// Создает новый экземпляр сервиса организации кормления
        /// </summary>
        /// <param name="animalRepository">Репозиторий животных</param>
        /// <param name="feedingScheduleRepository">Репозиторий расписаний кормления</param>
        /// <param name="eventDispatcher">Диспетчер событий</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если какой-либо из параметров равен null</exception>
        public FeedingOrganizationService(IAnimalRepository animalRepository, IFeedingScheduleRepository feedingScheduleRepository, DomainEventDispatcher eventDispatcher)
        {
            _animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
            _feedingScheduleRepository = feedingScheduleRepository ?? throw new ArgumentNullException(nameof(feedingScheduleRepository));
            _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        }

        /// <summary>
        /// Создает новое расписание кормления для животного
        /// </summary>
        /// <param name="animalId">Идентификатор животного</param>
        /// <param name="feedingTime">Время кормления</param>
        /// <param name="food">Тип пищи</param>
        /// <returns>true, если расписание создано успешно; иначе false</returns>
        public async Task<bool> CreateFeedingScheduleAsync(Guid animalId, TimeValueObject feedingTime, NonEmptyStringValueObject food)
        {
            var animal = await _animalRepository.GetByIdAsync(animalId);
            if (animal == null)
                return false;

            var schedule = new FeedingSchedule(animalId, feedingTime, food);
            await _feedingScheduleRepository.AddAsync(schedule);
            return true;
        }

        /// <summary>
        /// Обновляет время в расписании кормления
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="newFeedingTime">Новое время кормления</param>
        /// <returns>true, если расписание обновлено успешно; иначе false</returns>
        public async Task<bool> UpdateFeedingScheduleAsync(Guid scheduleId, TimeValueObject newFeedingTime)
        {
            var schedule = await _feedingScheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
                return false;

            schedule.ChangeSchedule(newFeedingTime);
            await _feedingScheduleRepository.UpdateAsync(schedule);
            return true;
        }

        /// <summary>
        /// Отменяет выполнение расписания кормления
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <returns>true, если расписание отменено успешно; иначе false</returns>
        public async Task<bool> CancelFeedingScheduleAsync(Guid scheduleId)
        {
            var schedule = await _feedingScheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
                return false;

            schedule.CancelExecution();
            await _feedingScheduleRepository.UpdateAsync(schedule);
            return true;
        }

        /// <summary>
        /// Восстанавливает выполнение отмененного расписания кормления
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <returns>true, если расписание восстановлено успешно; иначе false</returns>
        public async Task<bool> RestoreFeedingScheduleAsync(Guid scheduleId)
        {
            var schedule = await _feedingScheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
                return false;

            schedule.RestoreExecution();
            await _feedingScheduleRepository.UpdateAsync(schedule);
            return true;
        }
        
        /// <summary>
        /// Выполняет кормление по расписанию
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания кормления</param>
        /// <returns>true, если кормление выполнено успешно; иначе false</returns>
        public async Task<bool> ExecuteFeedingAsync(Guid scheduleId)
        {
            try
            {
                var schedule = await _feedingScheduleRepository.GetByIdAsync(scheduleId);
                if (schedule == null)
                    throw new ArgumentException("Feeding schedule not found", nameof(scheduleId));

                var animal = await _animalRepository.GetByIdAsync(schedule.AnimalId);
                if (animal == null)
                    throw new ArgumentException("Animal not found", nameof(scheduleId));

                animal.Feed(schedule.Food);
                
                await _animalRepository.UpdateAsync(animal);
                await _feedingScheduleRepository.UpdateAsync(schedule);
                
                var feedingTimeEvent = new FeedingTimeEvent(animal.Id, schedule.Id, schedule.Food);
                _eventDispatcher.Dispatch(feedingTimeEvent);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
} 