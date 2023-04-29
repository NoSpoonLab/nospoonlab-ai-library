using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FrameworkCore.Attributes;
using FrameworkCore.Reflection;
using UnityEngine;

namespace FrameworkCore.MVC
{
    public class GameView : MonoBehaviour 
    {
        private MethodStorage _methodStorage;
        private Dictionary<Type, object> _rawModels;
        private Dictionary<Type, object> _rawControllers;
        
        #region Initialization

        internal virtual void Awake()
        {
            InitializeModelAttributes();
            InitializeControllerAttributes();
        }

        #endregion

        #region Getters

        internal T GetModel<T>() where T : GameModel
        {
            var type = typeof(T);
            var model = _rawModels.ContainsKey(type) ? (T) _rawModels[type] : throw new Exception("Model not found");
            return (T) model.Clone();
        }
        
        public T GetController<T>()
        {
            var type = typeof(T);
            var controller = _rawControllers.ContainsKey(type) ? (T) _rawControllers[type] : throw new Exception("Controller not found");
            return (T) controller;
        }

        #endregion

        #region Methods

        public void WakeUp()
        {
            InitializeModelAttributes(true);
            InitializeControllerAttributes(true);
        }

        public void Initialize<T>(params object[] value)
        {
            var model = _rawModels[typeof(T)];
            var method = model.GetType().GetMethod("Initialize");
            method?.Invoke(model, value);
        }

        internal void ReceiveEventFromChild<T>(string functionName, params object[] value)
        {
            var controller = _rawControllers[typeof(T)];
            var method = controller.GetType().GetMethod(functionName);
            method?.Invoke(controller, value);
        }

        private void InitializeModelAttributes(bool force = false)
        {
            if (!Application.isPlaying && !force) return;
            Type modelType = null;
            try
            {
                _rawModels = new Dictionary<Type, object>();
                InitializeModelByType(GetType());
                _rawModels.ToList().ForEach(it =>
                {
                    modelType = it.Key;
                    var currentValue = ((GameModel)it.Value);
                    var callAwake = it.Key.GetMethod("Awake").MakeGenericMethod(it.Key);
                    callAwake?.Invoke(currentValue, null);
                });
            }
            catch (Exception e)
            {
                Debug.Log(e.Message + " | Current Class View: " + GetType().Name + " | Current Type Model: " + modelType?.Name + "\n");
                throw;
            }
        }

        private void InitializeModelByType(Type value)
        {
            var fields = new List<FieldInfo>();
            Type currentType = value;
            while (currentType != null)
            {
                fields.AddRange(currentType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
                currentType = currentType.BaseType;
            }
            
            fields.ForEach(field =>
            {
                var attributes = field.GetCustomAttributes(typeof(ModelFieldAttribute), false);
                if (attributes.Length > 0)
                {
                    var modelType = field.FieldType;
                    var modelValue = field.GetValue(this);
                    if (modelValue == null)
                    {
                        modelValue = Activator.CreateInstance(modelType);
                        field.SetValue(this, modelValue);
                    }
                    _rawModels.Add(modelType, modelValue);
                }
            });
        }

        private async void InitializeControllerAttributes(bool force = false)
        {
            _methodStorage = new MethodStorage();
            _rawControllers = new Dictionary<Type, object>();
            if (!Application.isPlaying && !force) return;

            //Get all the controllers
            var type = GetType();
            var fields = new List<FieldInfo>();
            Type currentType = type;
            while (currentType != null)
            {
                fields.AddRange(currentType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
                currentType = currentType.BaseType;
            }
                
            fields.ForEach(field =>
            {
                var attributes = field.GetCustomAttributes(typeof(ControllerFieldAttribute), false);
                if (attributes.Length > 0)
                {
                    var controllerType = field.FieldType;
                    var controllerInstance = Activator.CreateInstance(controllerType, this);
                    field.SetValue(this, controllerInstance);
                    _rawControllers.Add(controllerType, controllerInstance);
                }
            });
            
            //Setup the controller
            foreach (var it in _rawControllers.Values)
            {
                //Find SetView and call
                var setViewMethod = it.GetType().GetMethod("SetView");
                var setViewParameters = new object[] { this };
                setViewMethod?.Invoke(it, setViewParameters);
                
                //Find SetModel and call
                foreach (var model in _rawModels)
                {
                    var setModelMethod = it.GetType().GetMethod("SetModel").MakeGenericMethod(model.Key);
                    var setModelParameters = new object[] { model.Value };
                    setModelMethod?.Invoke(it, setModelParameters);
                }
            }
            

            //Get all the methods to bind
            var methodsToBindToController =
                Enum.GetValues(typeof(GameControllerEvents)).Cast<GameControllerEvents>().ToList();
            
            //Now bind the events methods
            foreach (var it in _rawControllers.Values)
            {
                methodsToBindToController.ForEach(methodToBind =>
                {
                    var newMethod = it.GetType().GetMethod(methodToBind.ToString());
                    if (newMethod != null)
                        _methodStorage.AddMethod(methodToBind,
                            new Method { MethodInfo = newMethod, AssociatedObject = it });
                });
            }
            
            //PreAwake Controller
            foreach (var it in _rawControllers.Values)
            {
                //Find Awake and call
                var invokeMethod = it.GetType().GetMethod("PreAwake");
                invokeMethod?.Invoke(it, null);
            }
            
            //Awake Controller
            await Task.Yield();
            foreach (var it in _rawControllers.Values)
            {
                //Find Awake and call
                var invokeMethod = it.GetType().GetMethod("Awake");
                invokeMethod?.Invoke(it, null);
            }
        }

        #endregion

        #region Unity Methods

        private async void Start()
        {
            await Task.Yield();
            _methodStorage?.GetList(GameControllerEvents.Start).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, null);
            });
        }
        
        private void Update()
        {
            _methodStorage?.GetList(GameControllerEvents.Update).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, null);
            });
        }
        
        private void FixedUpdate()
        {
            _methodStorage?.GetList(GameControllerEvents.FixedUpdate).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, null);
            });
        }
        
        private void LateUpdate()
        {
            _methodStorage?.GetList(GameControllerEvents.LateUpdate).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, null);
            });
        }
        
        private void OnEnable()
        {
            _methodStorage?.GetList(GameControllerEvents.OnEnable).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, null);
            });
        }
        
        private void OnDisable()
        {
            _methodStorage?.GetList(GameControllerEvents.OnDisable).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, null);
            });
        }
        
        private void OnDestroy()
        {
            _methodStorage?.GetList(GameControllerEvents.OnDestroy).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, null);
            });
            _rawModels?.ToList().ForEach(it =>
            {
                
                var currentValue = ((GameModel)it.Value);
                var callAwake = it.Key.GetMethod("Destroy").MakeGenericMethod(it.Key);
                callAwake?.Invoke(currentValue, null);
            });
        }
        
        private void OnCollisionEnter(Collision other)
        {
            _methodStorage?.GetList(GameControllerEvents.OnCollisionEnter).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, new object[] {other});
            });
        }
        
        private void OnCollisionExit(Collision other)
        {
            _methodStorage?.GetList(GameControllerEvents.OnCollisionExit).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, new object[] {other});
            });
        }
        
        private void OnCollisionStay(Collision other)
        {
            _methodStorage?.GetList(GameControllerEvents.OnCollisionStay).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, new object[] {other});
            });
        }
        
        private void OnTriggerEnter(Collider other)
        {
            _methodStorage?.GetList(GameControllerEvents.OnTriggerEnter).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, new object[] {other});
            });
        }
        
        private void OnTriggerExit(Collider other)
        {
            _methodStorage?.GetList(GameControllerEvents.OnTriggerExit).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, new object[] {other});
            });
        }
        
        private void OnTriggerStay(Collider other)
        {
            _methodStorage?.GetList(GameControllerEvents.OnTriggerStay).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, new object[] {other});
            });
        }
        
        private void OnMouseDown()
        {
            _methodStorage?.GetList(GameControllerEvents.OnMouseDown).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, null);
            });
        }
        
        private void OnMouseEnter()
        {
            _methodStorage?.GetList(GameControllerEvents.OnMouseEnter).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, null);
            });
        }
        
        private void OnMouseExit()
        {
            _methodStorage?.GetList(GameControllerEvents.OnMouseExit).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, null);
            });
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _methodStorage?.GetList(GameControllerEvents.OnCollisionEnter2D).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, new object[] {other});
            });
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            _methodStorage?.GetList(GameControllerEvents.OnCollisionExit2D).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, new object[] {other});
            });
        }
        
        private void OnCollisionStay2D(Collision2D other)
        {
            _methodStorage?.GetList(GameControllerEvents.OnCollisionStay2D).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, new object[] {other});
            });
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            _methodStorage?.GetList(GameControllerEvents.OnTriggerEnter2D).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, new object[] {other});
            });
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            _methodStorage?.GetList(GameControllerEvents.OnTriggerExit2D).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, new object[] {other});
            });
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            _methodStorage?.GetList(GameControllerEvents.OnTriggerStay2D).ForEach(it =>
            {
                it.MethodInfo.Invoke(it.AssociatedObject, new object[] {other});
            });
        }

        #endregion
    }
}