using System;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeFourthStageSatelliteController : Singleton<AfroditeFourthStageSatelliteController>
{
    public GameObject DeathAnim;
    public GameObject ShootAnim;
    public GameObject Projectile;
    public List<Transform> ShootDirections = new List<Transform>();

    #region States
    [NonSerialized] public AfroditeFourthStageStateSatelliteBaseState CurrentState;
    [NonSerialized] public AfroditeFourthStageStateSatelliteIdleState IdleState = new AfroditeFourthStageStateSatelliteIdleState();
    [NonSerialized] public AfroditeFourthStageStateSatelliteShootingState ShootingState = new AfroditeFourthStageStateSatelliteShootingState();
    #endregion

    protected override void Awake()
    {
        base.Awake();
        CurrentState = IdleState;
        CurrentState.EnterState(this);
    }

    private void Update()
    {
        CurrentState.UpdateState(this);

        if (AfroditeController.Instance.CurrentState != AfroditeController.Instance.FourthStageState)
            HandleDeath();
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
}