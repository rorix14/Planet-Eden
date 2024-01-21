using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.GOAP
{
    public class IntractWithPlayer : GAction
    {
        public override bool PrePerform()
        {
            return true;
        }

        public override bool PostPerform()
        {
          //  agentBeliefs.RemoveState(States.isInteracted);
            return true;
        }
    }
}