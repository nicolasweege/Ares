using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AfroditeBaseState
{
    public abstract void EnterState(AfroditeController context);

    public abstract void UpdateState(AfroditeController context);

    public virtual void OnTriggerEnter(AfroditeController context, Collider2D other)
    {
        
    }

    public virtual void OnTriggerExit(AfroditeController context, Collider2D other)
    {
        
    }
}