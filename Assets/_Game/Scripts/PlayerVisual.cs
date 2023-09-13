using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Material transparent;
    [SerializeField] private SkinnedMeshRenderer[] renderers;

    private Animator animator;
    private PlayerAnimation currentAnimation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimation(PlayerAnimation nextAnimation)
    {
        if (nextAnimation == currentAnimation) return;
        animator.ResetTrigger(currentAnimation.ToString());
        animator.SetTrigger(nextAnimation.ToString());
        currentAnimation = nextAnimation;
    }

    public void EnableTransparent()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = transparent;
        }
    }

    public void OnMouseOverPlayer()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.EnableKeyword(Constants.EMISSION);
        }
    }

    public void OnMouseExitPlayer()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.DisableKeyword(Constants.EMISSION);
        }
    }
}

public enum PlayerAnimation
{
    Idle,
    Walk,
    Run,
    Jump,
}
