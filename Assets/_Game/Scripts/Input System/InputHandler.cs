using UnityEngine;
using UnityEngine.Events;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

using Milo.Singleton;

public class InputHandler : MonoBehaviour
{
    public static UnityAction<bool> SetActiveLookAction;
        
    public Vector2 move;
    public Vector2 look;
    public bool isJump;
    public bool isLook = true;

    private void OnEnable()
    {
        SetActiveLookAction += SetActiveLook;
    }

    private void OnDisable()
    {
        SetActiveLookAction -= SetActiveLook;
    }

    private void Start()
    {
        SetActiveLook(isLook);
    }

#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        LookInput(isLook ? value.Get<Vector2>() : Vector2.zero);
    }
#endif

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    private void SetActiveLook(bool isLook)
    {
        this.isLook = isLook;
        Cursor.lockState = isLook ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

