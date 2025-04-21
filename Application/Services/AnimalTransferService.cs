using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Events;
using Domain.Repositories;

namespace Application.Services
{
    /// <summary>
    /// Сервис для перемещения животных между вольерами
    /// </summary>
    public class AnimalTransferService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IEnclosureRepository _enclosureRepository;
        private readonly DomainEventDispatcher _eventDispatcher;

        /// <summary>
        /// Создает новый экземпляр сервиса перемещения животных
        /// </summary>
        /// <param name="animalRepository">Репозиторий животных</param>
        /// <param name="enclosureRepository">Репозиторий вольеров</param>
        /// <param name="eventDispatcher">Диспетчер событий</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если какой-либо из параметров равен null</exception>
        public AnimalTransferService(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository, DomainEventDispatcher eventDispatcher)
        {
            _animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
            _enclosureRepository = enclosureRepository ?? throw new ArgumentNullException(nameof(enclosureRepository));
            _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        }

        /// <summary>
        /// Перемещает животное в другой вольер
        /// </summary>
        /// <param name="animalId">Идентификатор животного</param>
        /// <param name="targetEnclosureId">Идентификатор целевого вольера</param>
        /// <returns>true, если перемещение выполнено успешно; иначе false</returns>
        public async Task<bool> TransferAnimalAsync(Guid animalId, Guid targetEnclosureId)
        {
            try
            {
                var animal = await _animalRepository.GetByIdAsync(animalId);
                if (animal == null)
                    throw new ArgumentException("Animal not found", nameof(animalId));

                var targetEnclosure = await _enclosureRepository.GetByIdAsync(targetEnclosureId);
                if (targetEnclosure == null)
                    throw new ArgumentException("Target enclosure not found", nameof(targetEnclosureId));

                if (!targetEnclosure.CanTransfer(animal))
                    throw new InvalidOperationException($"Animal {animalId} can't be transfered to enclosure {targetEnclosureId}");

                Guid? previousEnclosureId = animal.EnclosureId;

                if (previousEnclosureId.HasValue)
                {
                    var currentEnclosure = await _enclosureRepository.GetByIdAsync(previousEnclosureId.Value);
                    if (currentEnclosure != null)
                    {
                        currentEnclosure.RemoveAnimal(animalId);
                        await _enclosureRepository.UpdateAsync(currentEnclosure);
                    }
                }

                animal.MoveToEnclosure(targetEnclosureId);
                targetEnclosure.AddAnimal(animalId);

                await _animalRepository.UpdateAsync(animal);
                await _enclosureRepository.UpdateAsync(targetEnclosure);

                var transferEvent = animal.DomainEvents[animal.DomainEvents.Count - 1] as AnimalMovedEvent;
                _eventDispatcher.Dispatch(transferEvent);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
} 