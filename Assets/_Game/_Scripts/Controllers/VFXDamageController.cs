using System.Collections;
using UnityEngine;

public class VFXDamageController : MonoBehaviour
{
    [SerializeField] private float _timeToAutoDestroy = 0.2f;

    private void Awake() {
        StartCoroutine(AutoDestroy());
    }

    private IEnumerator AutoDestroy() {
        yield return new WaitForSeconds(_timeToAutoDestroy);
        Destroy(gameObject);
    }
}