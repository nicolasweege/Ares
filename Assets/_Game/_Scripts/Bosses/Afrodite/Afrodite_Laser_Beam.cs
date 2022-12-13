using System.Collections.Generic;
using UnityEngine;

public class Afrodite_Laser_Beam : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LineRenderer _laserFeedbackLineRenderer;
    [SerializeField] private BoxCollider2D _laserBoxCollider;
    [SerializeField] private Transform _fireStartingPos;
    [SerializeField] private Transform _fireDir;
    [SerializeField] private GameObject _startVFX;
    private List<ParticleSystem> _particles = new List<ParticleSystem>();

    private void Awake() {
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
        FillLists();
        DisableLaser();
    }

    public void EnableFeedbackLaser() {
        _laserFeedbackLineRenderer.enabled = true;
    }

    public void DisableFeedbackLaser() {
        _laserFeedbackLineRenderer.enabled = false;
    }

    public void UpdateFeedbackLaser() {
        _laserFeedbackLineRenderer.SetPosition(0, (Vector2)_fireStartingPos.position);
        _laserFeedbackLineRenderer.SetPosition(1, (Vector2)_fireDir.position);
    }

    public void EnableLaser() {
        _lineRenderer.enabled = true;
        _laserBoxCollider.enabled = true;
        for (int i = 0; i < _particles.Count; i++) {
            _particles[i].Play();
        }
    }

    public void DisableLaser() {
        _lineRenderer.enabled = false;
        _laserBoxCollider.enabled = false;
        for (int i = 0; i < _particles.Count; i++) {
            _particles[i].Stop();
        }
    }

    public void UpdateLaser() {
        _laserBoxCollider.enabled = _lineRenderer.enabled ? Player.Instance.CanTakeDamage : false;
        _lineRenderer.SetPosition(0, (Vector2)_fireStartingPos.position);
        _lineRenderer.SetPosition(1, (Vector2)_fireDir.position);
        _startVFX.transform.position = new Vector3(_fireStartingPos.position.x, _fireStartingPos.position.y, _startVFX.transform.position.z);
    }

    private void FillLists() {
        for (int i = 0; i < _startVFX.transform.childCount; i++) {
            var particleSystem = _startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (particleSystem != null) {
                _particles.Add(particleSystem);
            }
        }
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}