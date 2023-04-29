using System;
using DependencyInjectionCore;

namespace FrameworkCore.MVC
{
    public abstract class GameModel : Injectable, ICloneable
    {
        #region Initialization

        public void Awake<T>() where T: GameModel
        {
        }
        
        public void Destroy<T>()  where T: GameModel
        {
        }

        internal T GetType<T>() where T: GameModel
        {
            return (T) this;
        }

        #endregion

        #region Destruction

        

        #endregion

        #region Methods

        public object Clone() => MemberwiseClone();

        #endregion

    }
}