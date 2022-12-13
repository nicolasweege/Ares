using System.Collections.Generic;
using UnityEngine;

public class Satellite_Laser : MonoBehaviour {
    public LineRenderer LineRenderer;
    public BoxCollider2D LaserBoxCollider;
    public Transform FireStartingPos;
    public Transform FireDir;
    public GameObject StartVFX;
    public GameObject EndVFX;
    // [SerializeField] private LayerMask _layerMask;
    private Vector2 LaserDir;
    private List<ParticleSystem> Particles = new List<ParticleSystem>();

    private void Awake() {
        FillLists();
        DisableLaser();
    }

    private void Update() {
        EnableLaser();
        UpdateLaser();
    }

    public void EnableLaser() {
        LineRenderer.enabled = true;
        LaserBoxCollider.enabled = true;
        for (int i = 0; i < Particles.Count; i++) {
            Particles[i].Play();
        }
    }

    public void DisableLaser() {
        LineRenderer.enabled = false;
        LaserBoxCollider.enabled = false;
        for (int i = 0; i < Particles.Count; i++) {
            Particles[i].Stop();
        }
    }

    public void UpdateLaser() {
        LineRenderer.SetPosition(0, (Vector2)FireStartingPos.position);
        LineRenderer.SetPosition(1, (Vector2)FireDir.position);
        LaserDir = (Vector2)FireDir.position - (Vector2)transform.position;
        StartVFX.transform.position = new Vector3(FireStartingPos.position.x, FireStartingPos.position.y, StartVFX.transform.position.z);
        EndVFX.transform.position = LineRenderer.GetPosition(1);
    }

    private void FillLists() {
        for (int i = 0; i < StartVFX.transform.childCount; i++) {
            var particleSystem = StartVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (particleSystem != null) {
                Particles.Add(particleSystem);
            }
        }

        for (int i = 0; i < EndVFX.transform.childCount; i++) {
            var particleSystem = EndVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (particleSystem != null) {
                Particles.Add(particleSystem);
            }
        }
    }
}