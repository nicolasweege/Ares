using UnityEngine;

public class TeseuIdleState : TeseuBaseState
{
    public override void EnterState(TeseuController context)
    {

    }

    public override void UpdateState(TeseuController context)
    {

    }

    public override void OnTriggerEnter(TeseuController context, Collider2D other)
    {
        if (other.CompareTag("PlayerMainShip") || other.CompareTag("PlayerSubAttackShip"))
        {
            context.BoxCollider.size = new Vector2(17f, 17f);
            context.SwitchState(context.FollowingPlayerState);
        }
    }

    public override void OnTriggerExit(TeseuController context, Collider2D other)
    {

    }
}