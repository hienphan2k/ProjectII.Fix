using UnityEngine;

using Milo.StateMachine;

[CreateAssetMenu(fileName = "ExampleDecision", menuName = "State Machine/Scriptable Object/Decision/ExampleDecision")]
public class ExampleDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return false;
    }
}