using BehaviorDesigner.Runtime.Tasks;
using Core.Combat;
using UnityEngine;

public class EnemyAction : Action
{
    protected Rigidbody2D rb2d;
    protected Animator anim;
    protected Destructable destructable;
    protected PlayerMovement player;

    public override void OnAwake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = PlayerMovement.Instance;
        destructable = GetComponent<Destructable>();
        anim = gameObject.GetComponentInChildren<Animator>();
    }
}
