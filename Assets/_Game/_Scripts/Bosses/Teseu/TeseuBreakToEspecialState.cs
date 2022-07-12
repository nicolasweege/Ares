using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeseuBreakToEspecialState : TeseuBaseState
{
    public override void EnterState(TeseuController context)
    {

    }

    public override void UpdateState(TeseuController context)
    {
        context.BreakTimer -= Time.deltaTime;
        if (context.BreakTimer <= 0f)
        {
            context.SwitchState(context.EspecialShootState);
            context.BreakTimer = 0.5f;
        }
    }

    public override void OnTriggerEnter(TeseuController context, Collider2D other)
    {

    }

    public override void OnTriggerExit(TeseuController context, Collider2D other)
    {
        if (other.CompareTag("PlayerMainShip") || other.CompareTag("PlayerSubAttackShip"))
        {
            context.EspecialShootTimer = context.TimeToEspecial;
            context.BoxCollider.size = new Vector2(10f, 10f);
            context.SwitchState(context.IdleState);
        }
    }
}