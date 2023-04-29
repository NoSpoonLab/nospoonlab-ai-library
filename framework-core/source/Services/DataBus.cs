using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FrameworkCore.MVC;
using UnityEngine.Diagnostics;

namespace FrameworkCore.Services
{
    public class DataBus
    {
        #region Properties

        private Dictionary<Type, List<Delegate>> _eventHandlers;

        private static DataBus _instance;
        public static DataBus Instance =>  _instance ?? (_instance = new DataBus());

        #endregion
        
        #region Events Methods

        private static void _addListener(Type type, Delegate listener)
        {
            List<Delegate> handlers;
            if (!Instance._eventHandlers.TryGetValue(type, out handlers))
            {
                handlers = new List<Delegate>();
                Instance._eventHandlers.Add(type, handlers);
            }
            handlers.Add(listener);
        }
        
        private static void _removeListener(Type type, Delegate listener)
        {
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
        
        public static void AddListenerReflection<T>(Func<T> listener) => _addListener(typeof(T), listener);
        public static void AddListener<T>(Func<T> listener) => _addListener(typeof(T), listener);
        public static void AddListener<TResult, T>(Func<T, TResult> listener) => _addListener(typeof(TResult), listener);
        public static void AddListener<TResult, T1, T2>(Func<T1, T2, TResult> listener)  => _addListener(typeof(TResult), listener);
        public static void AddListener<TResult, T1, T2, T3>(Func<T1, T2, T3, TResult> listener)  => _addListener(typeof(TResult), listener);
        
        public static void RemoveListenerReflection<T>(Func<T> listener) => _removeListener(typeof(T), listener);
        public static void RemoveListener<T>(Func<T> listener) => _removeListener(typeof(T), listener);
        public static void RemoveListener<TResult, T>(Func<T, TResult> listener) => _removeListener(typeof(TResult), listener);
        public static void RemoveListener<TResult, T1, T2>(Func<T1, T2, TResult> listener)  => _removeListener(typeof(TResult), listener);
        public static void RemoveListener<TResult, T1, T2, T3>(Func<T1, T2, T3, TResult> listener)  => _removeListener(typeof(TResult), listener);
        
        public static T GetData<T>() where T : class
        {
            Type type = typeof(T);
            List<Delegate> handlers;
            if (Instance._eventHandlers.TryGetValue(type, out handlers))
            {
                foreach (Func<T> handler in handlers)
                {
                    return handler();
                }
            }
            return null;
        }
        public static T GetData<T>(params object[] args) where T : class
        {
            Type type = typeof(T);
            List<Delegate> handlers;
            if (Instance._eventHandlers.TryGetValue(type, out handlers))
            {
                foreach (Delegate handler in handlers)
                {
                    if (handler.Method.ReturnType != type) throw new Exception($"Return type of handler {handler.Method.Name} is not {type}");
                    if (handler.Method.GetParameters().Length != args.Length) throw new Exception($"Handler {handler.Method.Name} has {handler.Method.GetParameters().Length} parameters, expected {args.Length}");
                    try
                    {
                        var paramTypes = handler.Method.GetParameters().Select(p => p.ParameterType).ToArray();
                        var genericType = Expression.GetFuncType(paramTypes.Concat(new[] { type }).ToArray());
                        var target = handler.Target;
                        var customDelegate = Delegate.CreateDelegate(genericType, target, handler.Method, true);
                        var result = (T) customDelegate.DynamicInvoke(args);
                        if (result is GameModel gameModelInstance) return (T) gameModelInstance.Clone();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error executing handler for type {type}: {ex.Message}", ex);
                    }
                }
            }
            return null;
        }
        #endregion
        
        #region Base Service Methods

        private DataBus()
        {
            Initialize();
        }
        
        ~DataBus()
        {
            OnDestroy();
        }

        private void Initialize()
        {
            if (_eventHandlers != null) return;
            _eventHandlers = new Dictionary<Type, List<Delegate>>();
        }

        private void OnDestroy()
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