using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AIAgent.Components;
using AIAgent.Types.Data;
using UnityEngine;
using UnityEngine.AI;

namespace AIAgent
{
    public class AgentPlanner
    {
        #region Variables
        
        private List<AgentAction> _actions;
        private Dictionary<AgentGoal, int> _goals;
        private Queue<AgentAction> _actionQueue;
        private AgentAction _currentAction;
        private AgentGoal _currentGoal;
        private bool _invoked;
        private State _beliefs;
        private GameObject _agentInstance;
        private NavMeshAgent _navMeshAgent;
        private bool _plannerRunning;

        #endregion

        #region Initialization

        public AgentPlanner(GameObject agentInstance)
        {
            _agentInstance = agentInstance;
            _navMeshAgent = _agentInstance.GetComponent<NavMeshAgent>();
            _actions = new List<AgentAction>();
            _goals = new Dictionary<AgentGoal, int>();
            _beliefs = new State();
            Reset();
        }

        private void Reset()
        {
            _invoked = false;
            _plannerRunning = false;
        }

        #endregion

        #region Lifecycle

        public async void LateUpdate()
        {
            if(_currentAction != null && _currentAction.Running)
            {
                if(_navMeshAgent.hasPath && _navMeshAgent.remainingDistance < 1f)
                {
                    if (!_invoked)
                    {
                        _invoked = true;
                        await Task.Delay((int)(_currentAction.Duration * 1000));
                        //Debug.Log("Action Completed");
                        CompleteAction();
                    }
                }
                return;
            }
            if(_plannerRunning == false || _actionQueue == null)
            {
                _plannerRunning = true;
                var sortedGoals = from entry in _goals orderby entry.Value descending select entry;
                foreach(var sg in sortedGoals)
                {
                    _actionQueue = Plan(_actions, sg.Key, _beliefs);
                    if(_actionQueue != null)
                    {
                        _currentGoal = sg.Key;
                        break;
                    }
                }
            }

            if (_actionQueue != null && _actionQueue.Count == 0)
            {
                if(_currentGoal.Remove)
                {
                    _goals.Remove(_currentGoal);
                }

                _plannerRunning = false;
                Reset();
            }

            if (_actionQueue != null && _actionQueue.Count > 0)
            {
                _currentAction = _actionQueue.Dequeue();
                if(_currentAction.PrePerform())
                {
                    if(_currentAction.Target != null)
                    {
                        var targetObject = WorldObject.FindObject(_currentAction.Target);
                        _currentAction.Running = true;
                        _navMeshAgent.SetDestination(targetObject.transform.position);
                    }
                }
                else
                {
                    _actionQueue = null;
                }
            }
        }

        #endregion
        
        #region Methods
        
        public void AddGoal(AgentGoal goal, int priority)
        {
            _goals.Add(goal, priority);
        }
        
        public void AddAction(AgentAction action)
        {
            _actions.Add(action);
        }

        public Queue<AgentAction> Plan(List<AgentAction> actions, AgentGoal goal, State beliefStates)
        {
            var usableActions = new List<AgentAction>();
            actions.ForEach(it => usableActions.Add(it.CloneInstance()));
            
            var leaves = new List<AgentPlanningNode>();
            var startNode = new AgentPlanningNode(null, 0f, World.Instance.GetWorld(), beliefStates, null);
            
            var success = BuildGraph(startNode, leaves, usableActions, goal.AgentGoalState);
            if (!success)
            {
                //Debug.Log("NO PLAN");
                return null;
            }
            
            AgentPlanningNode cheapest = null;
            foreach (var leaf in leaves)
            {
                if (cheapest == null)
                {
                    cheapest = leaf;
                }
                else
                {
                    if (leaf.Cost < cheapest.Cost)
                    {
                        cheapest = leaf;
                    }
                }
            }
            
            var result = new List<AgentAction>();
            var n = cheapest;
            while (n != null)
            {
                if (n.Action != null)
                {
                    result.Insert(0, n.Action);
                }
                n = n.Parent;
            }
        
            var queue = new Queue<AgentAction>();
            result.ForEach(it => queue.Enqueue(it));
        
            //Debug.Log("The Plan is: ");
            foreach (var a in queue)
            {
                //Debug.Log("Q: " + a.ActionName);
            }
        
            return queue;
        }

        private bool BuildGraph(
            AgentPlanningNode parent, 
            List<AgentPlanningNode> leaves, 
            List<AgentAction> usableActions,
            State goal)
        {
            var foundPath = false;
            foreach (var action in usableActions)
            {
                if (action.IsAchievableGiven(parent.State))
                {
                    var currentState = new State(parent.State);
                    foreach (KeyValuePair<string, int> eff in action.Effects.GetStates())
                    {
                        if (!currentState.HasState(eff.Key))
                        {
                            currentState.AddState(eff.Key, eff.Value);
                        }
                    }
                
                    var node = new AgentPlanningNode(parent, parent.Cost + action.Cost, currentState, action);
                    if (GoalAchieved(goal.GetStates(), currentState.GetStates()))
                    {
                        leaves.Add(node);
                        foundPath = true;
                    }
                    else
                    {
                        var subset = ActionSubset(usableActions, action);
                        var found = BuildGraph(node, leaves, subset, goal);
                        if (found)
                        {
                            foundPath = true;
                        }
                    }
                }
            }
            return foundPath;
        }
        
        private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
        {
            return goal.All(g => state.ContainsKey(g.Key));
        }
    
        private List<AgentAction> ActionSubset(List<AgentAction> actions, AgentAction removeMe)
        {
            return actions.Where(a => !a.Equals(removeMe)).ToList();
        }
        
        
        private void CompleteAction()
        {
            _currentAction.Running = false;
            _currentAction.PostPerform();
            _invoked = false;
        }

        #endregion
    }
}