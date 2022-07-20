using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeController : Singleton<AfroditeController>
{
    public int Health;
    public float Speed;
    [SerializeField] private GameObject _deathAnim;
    [Range(0f, 100f)]
    public float TurnSpeed;
    public GameObject LaserBeam;
    public Transform MovePointCenter;
    public List<Transform> MovePoints = new List<Transform>();

    #region First Stage Props
    public GameObject FirstStageBullet;
    public Transform FirstStageBulletStartingPoint;
    public Transform FirstStageBulletDir;
    #endregion

    #region States
    public AfroditeBaseState CurrentState;
    public AfroditeIdleState IdleState = new AfroditeIdleState();
    public AfroditeLaserShootState LaserShootState = new AfroditeLaserShootState();
    public AfroditeAimingState AimingState = new AfroditeAimingState();
    public AfroditeCenterAttackState CenterAttackState = new AfroditeCenterAttackState();
    #endregion

    [SerializeField] private float _timeToCenterAttack;
    private float _centerAttackTimer;

    protected override void Awake()
    {
        base.Awake();
        CurrentState = IdleState;
        CurrentState.EnterState(this);
        _centerAttackTimer = _timeToCenterAttack;
    }

    private void Update()
    {
        CurrentState.UpdateState(this);

        _centerAttackTimer -= Time.deltaTime;
        if (_centerAttackTimer <= 0)
        {
            SwitchState(CenterAttackState);
            _centerAttackTimer = _timeToCenterAttack;
        }

        if (Health <= 0)
            Death();

        Debug.Log(CurrentState);
    }

    public void SwitchState(AfroditeBaseState state)
    {
        CurrentState = state;
        state.EnterState(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CurrentState.OnTriggerEnter(this, other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CurrentState.OnTriggerExit(this, other);
    }

    public virtual void Death()
    {
        Destroy(gameObject);
        Instantiate(_deathAnim, transform.position, Quaternion.identity);
    }

    public void GenerateBullet(AfroditeController context, Transform bulletStartingPos, GameObject bulletPrefab)
    {
        if (PlayerMainShipController.Instance == null)
            return;

        var bulletInst = Instantiate(bulletPrefab, bulletStartingPos.position, bulletStartingPos.rotation);
        Vector2 bulletDir = context.FirstStageBulletDir.position - bulletInst.transform.position;
        bulletDir.Normalize();
        float bulletAngle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        bulletInst.transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(bulletDir.x, bulletDir.y);
    }
}