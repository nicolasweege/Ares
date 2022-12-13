using UnityEngine;

public abstract class Afrodite_State {
    public abstract void EnterState(Afrodite context);

    public abstract void UpdateState(Afrodite context);

    public virtual void OnTriggerEnter(Afrodite context, Collider2D other) {}

    public virtual void OnTriggerExit(Afrodite context, Collider2D other) {}
}