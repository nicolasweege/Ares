using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBase : MonoBehaviour
{
    public ShotStats Stats { get; private set; }
    [SerializeField] private GameObject _damageAnimation;
    [SerializeField] private float _timeToDeactiveShot = 1f;

    public virtual void SetStats(ShotStats stats) => Stats = stats;

    public virtual void DestroyShot()
    {
        Destroy(gameObject);
        Instantiate(_damageAnimation, transform.position, Quaternion.identity);
    }

    public virtual void DeactiveShot()
    {
        _timeToDeactiveShot -= Time.deltaTime;
        bool isShotVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
        if (!isShotVisible && _timeToDeactiveShot <= 0f)
            Destroy(gameObject);
    }
}