namespace AIAgent.Types.Data
{
    public class AgentGoal
    {
        #region Variables

        private State _agentGoal;
        private bool _remove;

        #endregion

        #region Methods

        public AgentGoal(string key, int value, bool removeAfertComplete)
        {
            _agentGoal = new State();
            _agentGoal.AddState(key, value);
            _remove = removeAfertComplete;
        }

        #endregion

        #region Properties

        public State AgentGoalState => _agentGoal;
        public bool Remove => _remove;

        #endregion
        
        
    }
}