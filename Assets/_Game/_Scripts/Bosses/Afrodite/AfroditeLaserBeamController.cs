using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeLaserBeamController : MonoBehaviour
{
    [SerializeField] private int _defaultDamage;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LineRenderer _laserFeedbackLineRenderer;
    [SerializeField] private BoxCollider2D _laserBoxCollider;
    [SerializeField] private Transform _fireStartingPos;
    [SerializeField] private Transform _fireDir;
    [SerializeField] private GameObject _startVFX;
    [SerializeField] private GameObject _endVFX;
    [SerializeField] private LayerMask _layerMask;
    private Vector2 _laserDir;
    private List<ParticleSystem> _particles = new List<ParticleSystem>();

    private void Awake()
    {
        FillLists();
        DisableLaser();
    }

    private void Update()
    {
        UpdateLaser();
    }

    public void EnableFeedbackLaser()
    {
        _laserFeedbackLineRenderer.enabled = true;
    }

    public void DisableFeedbackLaser()
    {
        _laserFeedbackLineRenderer.enabled = false;
    }

    public void UpdateFeedbackLaser()
    {
        _laserFeedbackLineRenderer.SetPosition(0, (Vector2)_fireStartingPos.position);
        _laserFeedbackLineRenderer.SetPosition(1, (Vector2)_fireDir.position);
    }

    public void EnableLaser()
    {
        _lineRenderer.enabled = true;
        _laserBoxCollider.enabled = true;
        for (int i = 0; i < _particles.Count; i++)
        {
            _particles[i].Play();
        }
    }

    public void DisableLaser()
    {
        _lineRenderer.enabled = false;
        _laserBoxCollider.enabled = false;
        for (int i = 0; i < _particles.Count; i++)
        {
            _particles[i].Stop();
        }
    }

    public void UpdateLaser()
    {
        _lineRenderer.SetPosition(0, (Vector2)_fireStartingPos.position);
        _lineRenderer.SetPosition(1, (Vector2)_fireDir.position);
        _laserDir = (Vector2)_fireDir.position - (Vector2)transform.position;
        _startVFX.transform.position = new Vector3(_fireStartingPos.position.x, _fireStartingPos.position.y, _startVFX.transform.position.z);

        RaycastHit2D laserHit = Physics2D.Raycast((Vector2)transform.position, _laserDir.normalized, _laserDir.magnitude, _layerMask);

        if (laserHit && _lineRenderer.enabled)
        {
            if (laserHit.collider.gameObject.CompareTag("PlayerMainShip"))
            {
                if (laserHit.collider.GetComponentInChildren<SpriteRenderer>().isVisible)
                {
                    // laserHit.collider.GetComponent<PlayerMainShipController>().TakeDamage(_defaultDamage);
                    laserHit.collider.GetComponent<PlayerMainShipController>().Death();
                }
            }

            _lineRenderer.SetPosition(1, laserHit.point);
            // Debug.Log(laserHit.collider.name);
        }
        _endVFX.transform.position = _lineRenderer.GetPosition(1);
    }

    private void FillLists()
    {
        for (int i = 0; i < _startVFX.transform.childCount; i++)
        {
            var particleSystem = _startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                _particles.Add(particleSystem);
            }
        }

        for (int i = 0; i < _endVFX.transform.childCount; i++)
        {
            var particleSystem = _endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                _particles.Add(particleSystem);
            }
        }
    }
}