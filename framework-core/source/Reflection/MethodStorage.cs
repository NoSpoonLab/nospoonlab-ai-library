using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkCore.MVC;

namespace FrameworkCore.Reflection
{
    public class MethodStorage
    {
        #region Properties
        
        private Dictionary<GameControllerEvents, List<Method>> _methods;

        #endregion

        #region Initialization

        public MethodStorage()
        {
            _methods = new Dictionary<GameControllerEvents, List<Method>>();
            var methods =
                Enum.GetValues(typeof(GameControllerEvents)).Cast<GameControllerEvents>().ToList();
            
            methods.ForEach(it => _methods.Add(it, new List<Method>()));
        }

        #endregion

        #region Methods

        public List<Method> GetList(GameControllerEvents value)
        {
            return _methods[value];
        }

        public void AddMethod(GameControllerEvents key, Method value)
        {
            _methods[key].Add(value);
        }

        #endregion
    }
}