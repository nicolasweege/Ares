using UnityEngine;

public abstract class Eros_State {
    public abstract void EnterState(Eros context);

    public abstract void UpdateState(Eros context);

    public virtual void OnTriggerEnter(Eros context, Collider2D other) {}

    public virtual void OnTriggerExit(Eros context, Collider2D other) {}
}