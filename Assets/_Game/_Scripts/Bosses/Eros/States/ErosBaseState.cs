using UnityEngine;

public abstract class ErosBaseState {
    public abstract void EnterState(ErosController context);

    public abstract void UpdateState(ErosController context);

    public virtual void OnTriggerEnter(ErosController context, Collider2D other) {}

    public virtual void OnTriggerExit(ErosController context, Collider2D other) {}
}