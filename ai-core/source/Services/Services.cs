using System;
using System.Collections.Generic;
using AICore.Services.Interfaces;

namespace AICore.Services
{
    public class Services
    {
        #region Properties
        
        private Dictionary<Type,IService> _services;

        #endregion

        #region Initialization & Destruction

        public Services()
        {
            _services = new Dictionary<Type, IService>();
        }
        
        ~Services()
        {
            _services.Clear();
            _services = null;
        }

        #endregion

        #region Methods

        public void Add<T>(T service) where T : IService
        {
            var type = typeof(T);
            if (_services.ContainsKey(type)) return;
            _services.Add(type, service);
        }

        public T Get<T>()
        {
            var type = typeof(T);
            if (!_services.ContainsKey(type)) throw new Exception("Service not found");
            return (T)_services[type];
        }
        
        public bool Exists<T>()
        {
            var type = typeof(T);
            return _services.ContainsKey(type);
        }

        #endregion
    }
}