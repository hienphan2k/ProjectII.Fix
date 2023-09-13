using UnityEngine;

namespace Milo.StateMachine
{
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(StateController controller);
    }
}
