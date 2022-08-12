using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSelectedAnimation : MonoBehaviour
{
    [SerializeField] private float _leanTweenDuration = 0.3f;
    private Vector3 _buttonPos;
    private BaseEventData _baseEventData;
    private bool _canResetPos = false;
    private bool _canAnim = false;

    private void Awake() {
        _buttonPos = transform.position;
    }

    private void Update() {
        if (EventSystem.current.currentSelectedGameObject == gameObject && _canAnim) {
            LeanTween.move(gameObject, 
            new Vector3(_buttonPos.x + 5, _buttonPos.y, 0), 
            _leanTweenDuration).setOnComplete(StopLeanTweenSelectedAnim);
            _canResetPos = true;
        }

        if (EventSystem.current.currentSelectedGameObject != gameObject && _canResetPos) {
            LeanTween.moveLocal(gameObject, _buttonPos, _leanTweenDuration).setOnComplete(SetCanResetPosVarToFalse);
        }
    }

    private void StopLeanTweenSelectedAnim() {
        _canAnim = false;
    }

    private void SetCanResetPosVarToFalse() {
        _canResetPos = false;
    }
}