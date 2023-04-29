using System;
using System.Linq;
using AIAgent.Types;
using AIAgent.Types.Data;
using UnityEngine;
using UnityEngine.Events;

namespace AIAgent.Components
{
    public  class AgentAction : ICloneable
    {
        #region Variables

        private string _actionName = "Action";
        private float _cost = 1.0f;
        private AgentWorldObjectMemory _target;
        private float _duration = 0f;
        private State _preconditions;
        private State _effects;
        private State _beliefs;
        private bool _running;

        #endregion

        #region Initialization

        public AgentAction(string actionName)
        {
            _preconditions = new State();
            _effects = new State();
            _beliefs = new State();
            _running = false;
            _target = null;
            _actionName = actionName;
        }

        public AgentAction(string actionName, State preconditions, State effects, float duration, float cost) : this(actionName)
        {
            _preconditions = preconditions;
            _effects = effects;
            _duration = duration;
            _cost = cost;
        }
        
        public AgentAction(string actionName, State preconditions, State effects, float duration, float cost, AgentWorldObjectMemory target) : this(actionName, preconditions, effects, duration, cost)
        {
            _target = target;
        }

        #endregion

        #region Methods
        
        public bool IsAchievable() => true;
        
        public bool IsAchievableGiven(State conditions)
        {
            return _preconditions.GetStates().All(p => conditions.HasState(p.Key));
        }

        #endregion

        #region Properties

        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }
        public State Preconditions => _preconditions;
        public State Effects => _effects;
        public float Cost => _cost;
        public bool Running
        {
            get => _running;
            set => _running = value;
        }

        public string ActionName
        {
            get => _actionName;
            set => _actionName = value;
        }
        
        public AgentWorldObjectMemory Target
        {
            get => _target;
            set => _target = value;
        }

        #endregion

        #region Abstract Methods

        public bool PrePerform() => true;
        public bool PostPerform() => true;

        #endregion
        
        public object Clone() => MemberwiseClone();

        public AgentAction CloneInstance() => (AgentAction)MemberwiseClone();
    }
}