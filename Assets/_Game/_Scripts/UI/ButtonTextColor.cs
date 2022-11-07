using UnityEngine;
using TMPro;

public class ButtonTextColor : MonoBehaviour {
    private float _alpha;
    private TMP_Text _text;

    private void Awake() {
        _text = GetComponent<TMP_Text>();
        _alpha = _text.color.a * 0.3f;
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _alpha);
    }

    private void OnEnable() {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _alpha);
    }
}