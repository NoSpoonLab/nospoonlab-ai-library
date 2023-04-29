using System.Collections.Generic;
using System.Linq;
using AIAgent.Components;

namespace AIAgent.Types.Data
{
    public class AgentPlanningNode
    {
        #region Variables

        private AgentPlanningNode _parent;
        private float _cost;
        private AgentAction _action;
        private State _state;

        #endregion

        #region Initialization
        
        public AgentPlanningNode(
            AgentPlanningNode parent, 
            float cost, 
            State worldState,
            AgentAction action)
        {
            _parent = parent;
            _cost = cost;
            _state = new State(worldState);
            _action = action;
        }
        

        public AgentPlanningNode(
            AgentPlanningNode parent, 
            float cost, 
            State worldState, 
            State beliefStates,
            AgentAction action)
        {
            _parent = parent;
            _cost = cost;
            _state = new State(worldState);
            foreach (var b in beliefStates.GetStates().Where(b => !_state.GetStates().ContainsKey(b.Key)))
            {
                _state.AddState(b.Key, b.Value);
            }
            _action = action;
        }

        #endregion

        #region Properties

        public float Cost => _cost;
        public AgentPlanningNode Parent => _parent;
        public State State => _state;
        public AgentAction Action
        {
            get => _action;
            set => _action = value;
        }

        #endregion
    }
}