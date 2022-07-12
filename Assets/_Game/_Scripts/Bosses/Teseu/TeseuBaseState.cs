using UnityEngine;

public abstract class TeseuBaseState
{
    public abstract void EnterState(TeseuController context);

    public abstract void UpdateState(TeseuController context);

    public abstract void OnTriggerEnter(TeseuController context, Collider2D other);

    public abstract void OnTriggerExit(TeseuController context, Collider2D other);
}