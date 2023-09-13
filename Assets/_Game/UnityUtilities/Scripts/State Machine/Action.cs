using UnityEngine;

namespace Milo.StateMachine
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(StateController controller);
    }
}
