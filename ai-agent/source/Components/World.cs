using AIAgent.Types.Data;

namespace AIAgent.Components
{
    public class World
    {
        #region Variables

        private static readonly World _instance = new World();
        private static State _worldState;

        #endregion

        #region Initialization

        private World()
        {
            _worldState = new State();
        }

        #endregion

        #region Properties
        
        public static World Instance => _instance;

        #endregion

        #region Getters

        public State GetWorld() => _worldState;

        #endregion
    }
}