using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderAlphaHandler : MonoBehaviour {
    private float _alpha;
    [SerializeField] private TMP_Text _sliderValueText;
    [SerializeField] private Image _bgImage, _fillImage, _handleImage;

    private void Awake() {
        _alpha = _sliderValueText.color.a * 0.3f;
        _sliderValueText.canvasRenderer.SetAlpha(_alpha);
        _bgImage.canvasRenderer.SetAlpha(_alpha);
        _fillImage.canvasRenderer.SetAlpha(_alpha);
        _handleImage.canvasRenderer.SetAlpha(_alpha);
    }

    private void OnEnable() {
        _sliderValueText.canvasRenderer.SetAlpha(_alpha);
        _bgImage.canvasRenderer.SetAlpha(_alpha);
        _fillImage.canvasRenderer.SetAlpha(_alpha);
        _handleImage.canvasRenderer.SetAlpha(_alpha);
    }
}