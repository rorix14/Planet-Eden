using System.Collections.Generic;

namespace RPG.GOAP
{
    public class Node
    {
        public Node parent;
        public float cost;
        public Dictionary<States, int> state;
        public GAction action;

        public Node(Node parent, float cost, Dictionary<States, int> allStates, GAction action)
        {
            this.parent = parent;
            this.cost = cost;
            state = new Dictionary<States, int>(allStates);
            this.action = action;
        }
    }

    public class GPlanner
    {
        public Queue<GAction> Plan(List<GAction> actions, Dictionary<States, int> goal, WorldStates beliefStates)
        {
            List<GAction> usableActions = new List<GAction>();

            foreach (GAction action in actions) if (action.IsAchivable()) usableActions.Add(action);

            List<Node> leaves = new List<Node>();
            Node start = new Node(null, 0, BuildAllStatesDictonary(GWorld.Instance.GetWorld().States, beliefStates.States), null);

            bool success = BuildGraph(start, leaves, usableActions, goal);

            if (!success)
            {
                //Debug.Log("NO PLAN");
                return null;
            }

            Node cheapest = null;

            foreach (Node leaf in leaves)
            {
                if (cheapest == null) cheapest = leaf;
                else
                {
                    if (leaf.cost < cheapest.cost) cheapest = leaf;
                }
            }

            List<GAction> result = new List<GAction>();
            while (cheapest != null)
            {
                if (cheapest.action) result.Insert(0, cheapest.action);

                cheapest = cheapest.parent;
            }

            // COMENT FOR BETTER PERFORMNACE
            //Debug.Log("the plan is: ");
            //foreach (GAction action in result) Debug.Log("Q: " + action.GetType().Name);

            return new Queue<GAction>(result);
        }

        private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<States, int> goal)
        {
            bool foundPath = false;
            foreach (GAction action in usableActions)
            {
                if (action.IsAchivableGiven(parent.state))
                {
                    Dictionary<States, int> currntState = new Dictionary<States, int>(parent.state);

                    foreach (KeyValuePair<States, int> effect in action.Effects)
                        if (!currntState.ContainsKey(effect.Key)) currntState.Add(effect.Key, effect.Value);

                    Node node = new Node(parent, parent.cost + action.Cost, currntState, action);

                    if (GoalAchived(goal, currntState))
                    {
                        leaves.Add(node);
                        foundPath = true;
                    }
                    else
                    {
                        List<GAction> subSet = ActionSubset(usableActions, action);
                        bool found = BuildGraph(node, leaves, subSet, goal);
                        if (found) foundPath = true;
                    }
                }
            }
            return foundPath;
        }

        private Dictionary<States, int> BuildAllStatesDictonary(Dictionary<States, int> worlStates, Dictionary<States, int> beliefStates)
        {
            Dictionary<States, int> allStates = new Dictionary<States, int>(worlStates);

            foreach (KeyValuePair<States, int> belief in beliefStates)
                if (!allStates.ContainsKey(belief.Key)) allStates.Add(belief.Key, belief.Value);

            return allStates;
        }

        private bool GoalAchived(Dictionary<States, int> goal, Dictionary<States, int> state)
        {
            foreach (KeyValuePair<States, int> g in goal) if (!state.ContainsKey(g.Key)) return false;

            return true;
        }

        private List<GAction> ActionSubset(List<GAction> usableActions, GAction removeAction)
        {
            List<GAction> subset = new List<GAction>();

            foreach (GAction action in usableActions) if (!action.Equals(removeAction)) subset.Add(action);

            return subset;
        }
    }
}