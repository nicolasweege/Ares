using System;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeMemberController : Singleton<AfroditeMemberController> {
    public GameObject DeathAnim;
    public GameObject ShootAnim;
    public GameObject Bullet;
    public List<Transform> ShootDirections = new List<Transform>();

    #region States
    [NonSerialized] public BaseState_Afrodite_M CurrentState;
    [NonSerialized] public IdleState_Afrodite_M IdleState = new IdleState_Afrodite_M();
    [NonSerialized] public ShootingState_Afrodite_M ShootingState = new ShootingState_Afrodite_M();
    #endregion

    protected override void Awake() {
        base.Awake();
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
        CurrentState = IdleState;
        CurrentState.EnterState(this);
    }

    private void Update() {
        CurrentState.UpdateState(this);

        if (AfroditeController.Instance.CurrentState != AfroditeController.Instance.FourthStageState) {
            Invoke(nameof(HandleDeath), 1f);
        }
    }

    public void HandleDeath() {
        Destroy(gameObject);
        Instantiate(DeathAnim, transform.position, Quaternion.identity);
    }

    public void SwitchState(BaseState_Afrodite_M state) {
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