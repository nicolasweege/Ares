using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shot : MonoBehaviour
{
    [SerializeField] protected GameObject _damageAnimation;
    [SerializeField] protected float _speed;
    [SerializeField] protected int _defaultDamage;
    protected float _timeToDeactiveShot = 1f;

    public virtual void DeactiveShot()
    {
        _timeToDeactiveShot -= Time.deltaTime;

        if (_timeToDeactiveShot <= 0f && !GetComponentInChildren<SpriteRenderer>().isVisible)
            Destroy(gameObject);
    }

    public virtual void DestroyShot()
    {
        Destroy(gameObject);
        Instantiate(_damageAnimation, transform.position, Quaternion.identity);
    }

    public void DestroyShot_2()
    {
        Destroy(gameObject);
        Instantiate(_damageAnimation, transform.position, Quaternion.identity);
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public int GetDefaultDamage()
    {
        return _defaultDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "InstanceDestroyer":
                Destroy(gameObject);
                break;
        }
    }
}