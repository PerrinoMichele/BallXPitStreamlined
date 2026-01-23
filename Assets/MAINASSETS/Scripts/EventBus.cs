using System;
using System.Collections.Generic;

namespace Core.Events
{
    public interface IGameEvent
    {
        // Interface for In-Game Events
    }

    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Action<IGameEvent>>> _subscribers = new();
        private static readonly Dictionary<Delegate, Action<IGameEvent>> _delegateLookup = new();

        public static void Subscribe<T>(Action<T> callback) where T : IGameEvent
        {
            Type eventType = typeof(T);
            Action<IGameEvent> wrapper = (e) => callback((T)e);

            _delegateLookup[callback] = wrapper;

            if (!_subscribers.ContainsKey(eventType))
                _subscribers[eventType] = new List<Action<IGameEvent>>();

            _subscribers[eventType].Add(wrapper);
        }

        public static void Unsubscribe<T>(Action<T> callback) where T : IGameEvent
        {
            Type eventType = typeof(T);

            if (!_subscribers.ContainsKey(eventType)) return;
            if (!_delegateLookup.TryGetValue(callback, out var wrapper)) return;

            _subscribers[eventType].Remove(wrapper);
            _delegateLookup.Remove(callback);
        }

        public static void Publish<T>(T gameEvent) where T : IGameEvent
        {
            Type eventType = typeof(T);
            if (!_subscribers.ContainsKey(eventType)) return;

            foreach (var action in _subscribers[eventType])
                action.Invoke(gameEvent);
        }
    }
}