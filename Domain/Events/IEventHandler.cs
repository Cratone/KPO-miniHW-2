namespace Domain.Events
{
    /// <summary>
    /// Интерфейс для обработчика доменных событий
    /// </summary>
    /// <typeparam name="T">Тип доменного события, с которым работает обработчик</typeparam>
    public interface IEventHandler<T> where T : IDomainEvent
    {
        /// <summary>
        /// Обрабатывает доменное событие
        /// </summary>
        /// <param name="event">Событие для обработки</param>
        void Handle(T @event);
    }
} 