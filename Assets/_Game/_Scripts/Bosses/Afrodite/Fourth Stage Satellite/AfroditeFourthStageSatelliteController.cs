using System;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeFourthStageSatelliteController : Singleton<AfroditeFourthStageSatelliteController>
{
    public GameObject DeathAnim;
    public GameObject ShootAnim;
    public GameObject Projectile;
    public Rotate RotateComponent;
    public List<Transform> ShootDirections = new List<Transform>();

    #region States
    [NonSerialized] public AfroditeFourthStageStateSatelliteBaseState CurrentState;
    [NonSerialized] public AfroditeFourthStageStateSatelliteIdleState IdleState = new AfroditeFourthStageStateSatelliteIdleState();
    [NonSerialized] public AfroditeFourthStageStateSatelliteShootingState ShootingState = new AfroditeFourthStageStateSatelliteShootingState();
    #endregion

    protected override void Awake()
    {
        base.Awake();
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
        RotateComponent = GetComponent<Rotate>();
        CurrentState = IdleState;
        CurrentState.EnterState(this);
    }

    private void Update()
    {
        CurrentState.UpdateState(this);

        if (AfroditeController.Instance.CurrentState != AfroditeController.Instance.FourthStageState) {
            Invoke(nameof(HandleDeath), 1f);
        }
    }

    public void HandleDeath()
    {
        Destroy(gameObject);
        Instantiate(DeathAnim, transform.position, Quaternion.identity);
    }

    public void SwitchState(AfroditeFourthStageStateSatelliteBaseState state)
    {
        CurrentState = state;
        state.EnterState(this);
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}