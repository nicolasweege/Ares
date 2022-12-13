using UnityEngine;

public class VFX_Destroyer : MonoBehaviour
{
    public float DestroyTimer = 0.5f;

    private void Update()
    {
        DestroyTimer -= Time.deltaTime;
        if (DestroyTimer <= 0f) Destroy(gameObject);
    }
}