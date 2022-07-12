using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeseuEspecialShootState : TeseuBaseState
{
    public override void EnterState(TeseuController context)
    {
        bool isEnemyVisible = context.GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!isEnemyVisible)
            return;

        context.GenerateBullet(context.EspecialBulletStartingPosLeft, context.EspecialBulletPrefab);
        context.GenerateBullet(context.EspecialBulletStartingPosRight, context.EspecialBulletPrefab);
        context.SwitchState(context.BreakToNormalState);
    }

    public override void UpdateState(TeseuController context)
    {

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