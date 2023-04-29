using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrameworkCore.Services
{
    public class EventBus
    {
        #region Properties

        private Dictionary<Type, List<Delegate>> _eventHandlers;

        private static EventBus _instance;
        public static EventBus Instance =>  _instance ??= new EventBus();

        #endregion

        #region Events Methods

        private static void _addListener(Type type, Delegate listener)
        {
            _forceAddListener(type, listener);
        }
        
        private static void _removeListener(Type type, Delegate listener)
        {
            _forceRemoveListener(type, listener);
        }

        internal static void _forceAddListener(Type type, Delegate listener)
        {
            List<Delegate> handlers;
            if (!Instance._eventHandlers.TryGetValue(type, out handlers))
            {
                handlers = new List<Delegate>();
                Instance._eventHandlers.Add(type, handlers);
            }
            handlers.Add(listener);
        }
        
        internal static async void _forceRemoveListener(Type type, Delegate listener)
        {
            await Task.Yield();
            List<Delegate> handlers;
            if (Instance._eventHandlers.TryGetValue(type, out handlers))
            {
                handlers.Remove(listener);
                if (handlers.Count == 0)
                {
                    Instance._eventHandlers.Remove(type);
                }
            }
        }
        
        public static void AddListener<T>(Action<T> listener) => _addListener(typeof(T), listener);
        
        public static void AddListener<T>(Action listener) => _addListener(typeof(T), listener);
        
        public static void RemoveListener<T>(Action<T> listener) => _removeListener(typeof(T), listener);
        
        public static void RemoveListener<T>(Action listener) => _removeListener(typeof(T), listener);
        
        public static void RaiseEvent<T>(T args = null) where T : class
        {
            Type type = typeof(T);
            List<Delegate> handlers;
            if (Instance._eventHandlers.TryGetValue(type, out handlers))
            {
                foreach (Delegate handler in handlers)
                {
                    if (handler is Action)
                    {
                        ((Action)handler).Invoke();
                    }
                    else if (handler is Action<T>)
                    {
                        ((Action<T>)handler).Invoke(args);
                    }
                }
            }
        }
        

        #endregion
        
        #region Base Service Methods

        private EventBus()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (_eventHandlers != null) return;
            _eventHandlers = new Dictionary<Type, List<Delegate>>();
        }

        public void Destroy()
        {
            foreach (var keyValuePair in _eventHandlers)
            {
                keyValuePair.Value.Clear();
            }
            _eventHandlers.Clear();
            _eventHandlers = null;
            _instance = null;
        }

        #endregion
       
    }
}