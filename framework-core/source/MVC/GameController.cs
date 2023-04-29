using System;
using System.Collections.Generic;
using DependencyInjectionCore;
using UnityEngine;

namespace FrameworkCore.MVC
{
    public abstract class GameController<TView> : Injectable where TView : GameView
    {
        #region Properties
        
        private TView _view = null;
        private Dictionary<Type, object> _models;

        #endregion

        #region Getters and Setters
        
        public void SetView(TView view)
        {
            if (_view != null) throw new Exception("View already exist, can't set new view");
            _view = view;
        }

        public void SetModel<T>(object model)
        {
            var type = typeof(T);
            _models ??= new Dictionary<Type, object>();
            if (!_models.ContainsKey(type))
            {
                _models.Add(type, model);
                return;
            }
            throw new Exception("Model already exist, can't set new model with this type");
        }

        protected T GetModel<T>()
        {
            var type = typeof(T);
            return _models.ContainsKey(type) ? (T) _models[type] : throw new Exception("Model not found");
        }
        
        protected T GetController<T>() where T : GameController<TView>
        {
            return _view.GetController<T>();
        }
        
        protected TView GetView() => _view;

        protected ControllerType GetModelFromOtherObject<ControllerType>(GameObject value) where ControllerType : GameModel
        {
            var componentExist = value.TryGetComponent(out GameView viewComponent);
            if(!componentExist) throw new Exception("GameView component not found");
            return viewComponent.GetModel<ControllerType>();
        }
        
        protected ControllerType GetControllerFromOtherObject<ControllerType>(GameObject value)
        {
            var componentExist = value.TryGetComponent(out GameView viewComponent);
            if(!componentExist) throw new Exception("GameView component not found");
            return viewComponent.GetController<ControllerType>();
        }

        protected List<ControllerType> GetChildControllers<ControllerType>()
        {
            var controllers = new List<ControllerType>();
            var transform = GetView().transform;
            var childCount = transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                controllers.Add(GetControllerFromOtherObject<ControllerType>(child.gameObject));
            }
            return controllers;
        }

        protected List<ModelType> GetChildModels<ModelType>() where ModelType : GameModel
        {
            var models = new List<ModelType>();
            var transform = GetView().transform;
            var childCount = transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                models.Add(GetModelFromOtherObject<ModelType>(child.gameObject));
            }

            return models;
        }

        protected void SendEventToParent<ControllerType>(string functionName, params object[] data)
        {
            var componentExist = GetView().transform.parent.TryGetComponent(out GameView gameView);
            if(!componentExist) throw new Exception("GameView component not found");
            gameView.ReceiveEventFromChild<ControllerType>(functionName, data);
        }

        #endregion
    }
}