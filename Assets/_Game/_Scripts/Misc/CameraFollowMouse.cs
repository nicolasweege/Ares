using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{
    [SerializeField] private float threshold;
    [SerializeField] private Player _playerController;

    private void Awake() {
        GameManager.OnAfterGameStateChanged += OnGameStateChanged;
    }

    private void Update()
    {
        Vector3 mousePos = Utils.GetMouseWorldPosition();
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 targetPos;

        if (_playerController.IsGamepad) {
            targetPos = (playerPos + new Vector3(_playerController._playerDirGamepadMode.x * 5, _playerController._playerDirGamepadMode.y * 5, 0f));
        }
        else {
            targetPos = (playerPos + mousePos) / 2f;
        }

        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + playerPos.x, threshold + playerPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + playerPos.y, threshold + playerPos.y);

        transform.position = targetPos;
    }

    private void OnDestroy() {
        GameManager.OnAfterGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        enabled = newState == GameState.Gameplay;
    }
}