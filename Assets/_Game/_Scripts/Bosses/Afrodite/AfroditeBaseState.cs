using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AfroditeBaseState
{
    public abstract void EnterState(AfroditeController context);

    public abstract void UpdateState(AfroditeController context);

    public virtual void OnTriggerEnter(AfroditeController context, Collider2D other)
    {
        if (other.CompareTag("PlayerMainShip"))
        {
            context.IsPlayerInRadar = true;
            context.BoxCollider.size = new Vector2(30f, 30f);
        }
    }

    public virtual void OnTriggerExit(AfroditeController context, Collider2D other)
    {
        if (other.CompareTag("PlayerMainShip"))
        {
            context.IsPlayerInRadar = false;
            context.BoxCollider.size = new Vector2(15f, 15f);
        }
    }
}