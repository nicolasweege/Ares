using UnityEngine;

public class Afrodite_State_1 : Afrodite_State {
    private float _defaultSpeed = 2f;
    private float _timeToSwitchState = 1.5f;
    private float _switchStateTimer;
    private int _randomIndex;
    private Vector2 _currentMovePoint;
    private float _timeToFirstWaveShoot = 0.1f;
    private float _firstWaveShootTimer;
    private float _timeToSecondWaveShoot = 0.25f;
    private float _secondWaveShootTimer;
    private float _timeToSecondAttack = 1f;
    private float _secondAttackTimer;
    private bool _isFirstWaveFinished = false;

    public override void EnterState(Afrodite context) {
        _switchStateTimer = _timeToSwitchState;
        _randomIndex = Random.Range(0, context.MovePoints.Count);
        _currentMovePoint = context.MovePoints[_randomIndex].position;
        _secondAttackTimer = _timeToSecondAttack;
    }

    public override void UpdateState(Afrodite context) {
        if (Vector2.Distance(context.transform.position, _currentMovePoint) < 0.5f) {
            _switchStateTimer -= Time.deltaTime;
            if (_switchStateTimer <= 0f) context.SwitchState(context.FirstState);
        }
        else HandleSecondAttack(context);

        HandleMovement(context);
        HandleTurretAim(context.TurretTransform1);
        HandleTurretAim(context.TurretTransform2);
        HandleFirstAttack(context);
    }

    private void HandleMovement(Afrodite context) {
        if (Player.Instance == null) return;

        Vector2 playerPos = Player.Instance.transform.position;
        Vector2 lookDir = playerPos - new Vector2(context.transform.position.x, context.transform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270f;
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.Euler(0, 0, lookAngle), context.TurnSpeed * Time.deltaTime);
        context.transform.position = Vector2.SmoothDamp(context.transform.position, _currentMovePoint, ref context.Velocity, _defaultSpeed);
    }

    private void HandleFirstAttack(Afrodite context) {
        if (!_isFirstWaveFinished) {
            _firstWaveShootTimer -= Time.deltaTime;
            if (_firstWaveShootTimer <= 0f) {
                GenerateBullet(context, context.FirstStageProjectileStartingPoint1, context.FirstStageProjectile, context.FirstStageProjectileDir1);
                SoundManager.PlaySound(SoundManager.Sound.AfroditeFirstStageShoot, context.transform.position, 0.5f);
                _isFirstWaveFinished = true;
                _firstWaveShootTimer = _timeToFirstWaveShoot;
            }
        }

        if (_isFirstWaveFinished) {
            _secondWaveShootTimer -= Time.deltaTime;
            if (_secondWaveShootTimer <= 0f) {
                GenerateBullet(context, context.FirstStageProjectileStartingPoint2, context.FirstStageProjectile, context.FirstStageProjectileDir2);
                _isFirstWaveFinished = false;
                _secondWaveShootTimer = _timeToSecondWaveShoot;
            }
        }
    }

    private void HandleSecondAttack(Afrodite context) {
        _secondAttackTimer -= Time.deltaTime;
        if (_secondAttackTimer <= 0f) {
            for (int i = 0; i < context.ThirdStageFirstWaveShootDirections.Count; i++) {
                GenerateBulletSecondAttack(context, context.transform, context.ThirdStageProjectile, context.ThirdStageFirstWaveShootDirections[i]);
                SoundManager.PlaySound(SoundManager.Sound.AfroditeFirstStageShoot, context.transform.position, 0.3f);
            }
            _secondAttackTimer = _timeToSecondAttack;
        }
    }

    private void GenerateBullet(Afrodite context, Transform bulletStartingPos, GameObject bulletPrefab, Transform projectileDir) {
        if (Player.Instance == null) return;

        var bulletInst = Object.Instantiate(bulletPrefab, bulletStartingPos.position, bulletStartingPos.rotation);
        context.CurrentFirstStageProjectileDir = projectileDir.position - bulletInst.transform.position;
        context.CurrentFirstStageProjectileDir.Normalize();
        bulletInst.GetComponent<Afrodite_Bullet_1>().Direction = new Vector3(context.CurrentFirstStageProjectileDir.x, context.CurrentFirstStageProjectileDir.y);
    }

    private void GenerateBulletSecondAttack(Afrodite context, Transform bulletStartingPos, GameObject bulletPrefab, Transform projectileDir) {
        Object.Instantiate(context.ThirdStageShootAnim, bulletStartingPos.position, bulletStartingPos.rotation);
        var bulletInst = Object.Instantiate(bulletPrefab, bulletStartingPos.position, bulletStartingPos.rotation);
        Vector2 bulletDir = projectileDir.position - bulletInst.transform.position;
        bulletDir.Normalize();
        float bulletAngle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        bulletInst.transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);

        // this is using the 'Afrodite_Bullet_2' script
        bulletInst.GetComponent<Afrodite_Bullet_2>().Direction = new Vector3(bulletDir.x, bulletDir.y);
    }

    private Vector2 HandleTurretAim(Transform turretTransform) {
        if (Player.Instance == null) return Vector2.zero;

        Vector2 playerPos = Player.Instance.transform.position;
        Vector2 lookDir = playerPos - new Vector2(turretTransform.position.x, turretTransform.position.y);
        lookDir.Normalize();
        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        turretTransform.rotation = Quaternion.Slerp(turretTransform.rotation, Quaternion.Euler(0, 0, lookAngle), 20f * Time.deltaTime);
        return lookDir;
    }
}