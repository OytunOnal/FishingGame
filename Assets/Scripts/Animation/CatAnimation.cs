
using UnityEngine;

public class CatAnimation : Singleton<CatAnimation>
{
    private Animator animator;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
    }
    public void Idle()
    {
        animator.SetTrigger("Idle");
    }

    public void CastFishingLine()
    {
        animator.SetTrigger("Cast");
    }

    public void CatchFish()
    {
        animator.SetTrigger("Catch");
    }
    public void Happy()
    {
        animator.SetTrigger("Happy");
    }
}
