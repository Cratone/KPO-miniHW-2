using System;
using System.Collections.Generic;

namespace Domain.Events
{
    /// <summary>
    /// Диспетчер доменных событий, который отвечает за регистрацию обработчиков и отправку событий
    /// </summary>
    public class DomainEventDispatcher
    {
        private static readonly Dictionary<Type, List<object>> _handlers = new Dictionary<Type, List<object>>();
        private static readonly DomainEventDispatcher _instance = new DomainEventDispatcher();

        private DomainEventDispatcher() { }

        /// <summary>
        /// Получает единственный экземпляр диспетчера событий (Singleton)
        /// </summary>
        public static DomainEventDispatcher Instance => _instance;

        /// <summary>
        /// Регистрирует обработчик событий определенного типа
        /// </summary>
        /// <typeparam name="T">Тип доменного события</typeparam>
        /// <param name="handler">Обработчик события</param>
        public void Register<T>(IEventHandler<T> handler) where T : IDomainEvent
        {
            var eventType = typeof(T);
            
            if (!_handlers.ContainsKey(eventType))
            {
                _handlers[eventType] = new List<object>();
            }
            
            _handlers[eventType].Add(handler);
        }

        /// <summary>
        /// Отправляет событие всем зарегистрированным обработчикам этого типа событий
        /// </summary>
        /// <typeparam name="T">Тип доменного события</typeparam>
        /// <param name="event">Событие для отправки</param>
        public void Dispatch<T>(T @event) where T : IDomainEvent
        {
            var eventType = @event.GetType();
            
            if (_handlers.ContainsKey(eventType))
            {
                foreach (var handler in _handlers[eventType])
                {
                    if (handler is IEventHandler<T> typedHandler)
                    {
                        typedHandler.Handle(@event);
                    }
                }
            }
        }
    }
} 