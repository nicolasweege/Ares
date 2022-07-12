using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeseuController : EnemyBase
{
    public GameObject NormalBulletPrefab;
    public GameObject EspecialBulletPrefab;
    public Transform NormalBulletStartingPos;
    public Transform EspecialBulletStartingPosLeft;
    public Transform EspecialBulletStartingPosRight;
    public float TimeToNormalShoot;
    public float TimeToEspecial;
    public float TimeToBreak;
    public float BreakTimer;
    public float NormalShootTimer;
    public float EspecialShootTimer;
    public BoxCollider2D BoxCollider;

    public TeseuBaseState CurrentState;
    public TeseuIdleState IdleState = new TeseuIdleState();
    public TeseuFollowingPlayerState FollowingPlayerState = new TeseuFollowingPlayerState();
    public TeseuEspecialShootState EspecialShootState = new TeseuEspecialShootState();
    public TeseuBreakToNormalState BreakToNormalState = new TeseuBreakToNormalState();
    public TeseuBreakToEspecialState BreakToEspecialState = new TeseuBreakToEspecialState();

    protected override void Awake()
    {
        base.Awake();
        EspecialShootTimer = TimeToEspecial;
        BreakTimer = TimeToBreak;
        CurrentState = IdleState;
        CurrentState.EnterState(this);
    }

    private void Update()
    {
        CurrentState.UpdateState(this);

        if (_health <= 0)
            Death();
    }

    public void SwitchState(TeseuBaseState state)
    {
        CurrentState = state;
        state.EnterState(this);
    }

    public void HandleNormalShoot()
    {
        bool isEnemyVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!isEnemyVisible)
            return;

        NormalShootTimer -= Time.deltaTime;
        if (NormalShootTimer <= 0f)
        {
            GenerateBullet(NormalBulletStartingPos, NormalBulletPrefab);
            NormalShootTimer = TimeToNormalShoot;
        }
    }

    public void GenerateBullet(Transform bulletStartingPos, GameObject bulletPrefab)
    {
        if (PlayerMainShipController.Instance == null)
            return;

        GameObject bulletInst = Instantiate(bulletPrefab, bulletStartingPos.position, Quaternion.identity);
        Vector2 bulletDir = PlayerMainShipController.Instance.transform.position - bulletInst.transform.position;
        bulletDir.Normalize();
        float bulletAngle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        bulletInst.transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle + 90f);
        bulletInst.GetComponent<BulletBase>().Direction = new Vector3(bulletDir.x, bulletDir.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CurrentState.OnTriggerEnter(this, other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CurrentState.OnTriggerExit(this, other);
    }
}