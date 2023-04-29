using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjectionCore
{
    /// <summary>
    /// Dependency Injection Container that acts as a service provider for registering, binding, 
    /// releasing and retrieving services in a system.
    /// </summary>
    public class DIContainer
    {
        #region Properties

        /// <summary>
        /// Maintains the dictionary of type-object pairs for registered services.
        /// </summary>
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        #endregion

        #region Methods

        /// <summary>
        /// Registers a service with its implementation.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface type to be registered.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <exception cref="System.Exception">Throws when the service is already registered.</exception>
        public static void Register<TInterface, TImplementation>() where TImplementation : new()
        {
            var interfaceType = typeof(TInterface);
            if (!_services.ContainsKey(interfaceType))
            {
                _services.Add(interfaceType, new TImplementation());
            }
            else
            {
                throw new Exception("Service is already registered");
            }
        }

        /// <summary>
        /// Checks whether a specific service exists or not.
        /// </summary>
        /// <typeparam name="TInterface">The type of the service interface.</typeparam>
        /// <returns>Boolean indicating the existence of the service.</returns>
        public static bool Exists<TInterface>() => _services.ContainsKey(typeof(TInterface));

        /// <summary>
        /// Binds a specific data object to a service.
        /// </summary>
        /// <typeparam name="TInterface">The type of the service interface to bind to.</typeparam>
        /// <param name="data">The data to be bind to the service.</param>
        public static void Bind<TInterface>(object data)
        {
            var interfaceType = typeof(TInterface);
            if (!_services.ContainsKey(interfaceType))
            {
                _services.Add(interfaceType, data);
            }
            else
            {
                _services[interfaceType] = data;
            }
        }

        /// <summary>
        /// Releases a service from the DI Container.
        /// </summary>
        /// <typeparam name="TInterface">The type of the service to be released.</typeparam>
        /// <exception cref="System.Exception">Throws when the service to be released is not registered.</exception>
        public static void Release<TInterface>()
        {
            var type = typeof(TInterface);
            if (_services.ContainsKey(type))
            {
                _services.Remove(type);
            }
            else
            {
                throw new Exception($"Service {type.Name} is not registered");
            }
        }

        /// <summary>
        /// Retrieves a service from the DI Container.
        /// </summary>
        /// <typeparam name="TInterface">The type of the service to retrieve.</typeparam>
        /// <returns>The service of the specified type.</returns>
        /// <exception cref="System.Exception">Throws when the service to retrieve is not registered.</exception>
        public static TInterface Get<TInterface>()
        {
            var type = typeof(TInterface);
            if (_services.ContainsKey(type))
            {
                return (TInterface)_services[type];
            }
            else
            {
                throw new Exception($"Service {type.Name} is not registered");
            }
        }

        /// <summary>
        /// Retrieves a service from the DI Container by its type.
        /// </summary>
        /// <param name="type">The type of the service to retrieve.</param>
        /// <returns>The service of the specified type.</returns>
        /// <exception cref="System.Exception">Throws when the service to retrieve is not registered.</exception>
        public static object Get(Type type)
        {
            if (_services.ContainsKey(type))
            {
                return _services[type];
            }
            else
            {
                throw new Exception($"Service {type.Name} is not registered");
            }
        }

        /// <summary>
        /// Retrieves all the services registered in the DI Container.
        /// </summary>
        /// <returns>A list of all registered services.</returns>
        public static List<object> GetAll()
        {
            return _services.Select(service => service.Value).ToList();
        }

        #endregion
    }
}