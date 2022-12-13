using System.Collections.Generic;
using UnityEngine;

public class Afrodite_Member : Singleton<Afrodite_Member> {
    public GameObject DeathAnim;
    public GameObject ShootAnim;
    public GameObject Bullet;
    public List<Transform> ShootDirections = new List<Transform>();

    #region States
    public Afrodite_Member_State CurrentState;
    public Afrodite_Member_State_Idle IdleState = new Afrodite_Member_State_Idle();
    public Afrodite_Member_State_Shoot ShootState = new Afrodite_Member_State_Shoot();
    #endregion

    protected override void Awake() {
        base.Awake();
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
        CurrentState = IdleState;
        CurrentState.EnterState(this);
    }

    private void Update() {
        CurrentState.UpdateState(this);

        if (Afrodite.Instance.CurrentState != Afrodite.Instance.FourthState) {
            Invoke(nameof(HandleDeath), 1f);
        }
    }

    public void HandleDeath() {
        Destroy(gameObject);
        Instantiate(DeathAnim, transform.position, Quaternion.identity);
    }

    public void SwitchState(Afrodite_Member_State state) {
        if (enabled) {
            CurrentState = state;
            state.EnterState(this);
        }
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}