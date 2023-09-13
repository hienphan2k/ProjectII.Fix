using System;

namespace Milo.StateMachine
{
    [Serializable]
    public class Transition
    {
        public Decision decision;
        public State trueState;
        public State falseState;
    }
}
