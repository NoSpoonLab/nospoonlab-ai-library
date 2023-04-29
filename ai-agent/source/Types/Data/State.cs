using System.Collections.Generic;

namespace AIAgent.Types.Data
{
    public class State
    {
        #region Variables
        
        private Dictionary<string, int> _states;

        #endregion

        #region Methods

        public State()
        {
            _states = new Dictionary<string, int>();
        }

        public State(string key, int value) : this()
        {
            AddState(key, value);
        }
        
        public State(State state)
        {
            _states = new Dictionary<string, int>(state.GetStates());
        }

        public bool HasState(string key) => _states.ContainsKey(key);
    
        public void AddState(string key, int value) => _states.Add(key, value);
    
        public void ModifyState(string key, int value)
        {
            if (_states.ContainsKey(key))
            {
                _states[key] += value;
                if (_states[key] <= 0)
                {
                    RemoveState(key);
                }
            }
            else
            {
                _states.Add(key, value);
            }
        }
    
        public void RemoveState(string key) => _states.Remove(key);
    
        public void SetState(string key, int value)
        {
            if (_states.ContainsKey(key))
            {
                _states[key] = value;
            }
            else
            {
                _states.Add(key, value);
            }
        }

        #endregion

        #region Getters
        
        public Dictionary<string, int> GetStates() => _states;

        #endregion
    }
}