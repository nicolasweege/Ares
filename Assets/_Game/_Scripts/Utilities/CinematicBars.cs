using UnityEngine;
using UnityEngine.UI;

public class CinematicBars : Singleton<CinematicBars> {
    private RectTransform _topBar, _bottomBar;
    private float _changeSizeAmount;
    private float _targetSize;
    private bool _isActive;

    protected override void Awake() {
        base.Awake();
        GameObject gameObject = new GameObject("topBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        _topBar = gameObject.GetComponent<RectTransform>();
        _topBar.anchorMin = new Vector2(0, 1);
        _topBar.anchorMax = new Vector2(1, 1);
        _topBar.sizeDelta = new Vector2(0, 0);

        gameObject = new GameObject("bottomBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        _bottomBar = gameObject.GetComponent<RectTransform>();
        _bottomBar.anchorMin = new Vector2(0, 0);
        _bottomBar.anchorMax = new Vector2(1, 0);
        _bottomBar.sizeDelta = new Vector2(0, 0);
    }

    private void Update() {
        if (!_isActive) return;

        Vector2 sizeDelta = _topBar.sizeDelta;
        sizeDelta.y += _changeSizeAmount * Time.deltaTime;
        if (_changeSizeAmount > 0) {
            if (sizeDelta.y > _targetSize) {
                sizeDelta.y = _targetSize;
                _isActive = true;
            }
        } else {
            if (sizeDelta.y <= _targetSize) {
                sizeDelta.y = _targetSize;
                _isActive = true;
            }
        }
        _topBar.sizeDelta = sizeDelta;
        _bottomBar.sizeDelta = sizeDelta;
    }

    public void Show(float targetSize, float time) {
        this._targetSize = targetSize;
        _changeSizeAmount = (_targetSize - _topBar.sizeDelta.y) / time;
        _isActive = true;
    }

    public void Hide(float time) {
        _targetSize = 0f;
        _changeSizeAmount = (_targetSize - _topBar.sizeDelta.y) / time;
        _isActive = true;
    }
}