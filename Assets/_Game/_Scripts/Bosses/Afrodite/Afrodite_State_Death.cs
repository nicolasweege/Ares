using UnityEngine;
using System.Threading.Tasks;

public class Afrodite_State_Death : Afrodite_State {
    private bool _canScreenShake = false;

    public override void EnterState(Afrodite context) {
        context.LaserBeam.GetComponent<Afrodite_Laser_Beam>().DisableLaser();
        context.LaserBeam.GetComponent<Afrodite_Laser_Beam>().DisableFeedbackLaser();
        _canScreenShake = true;
        HandleScreenShake(500, context);
        HandleDeath(2000, context);
    }

    public override void UpdateState(Afrodite context) {}

    private async void HandleScreenShake(int millisecondsDelay, Afrodite context) {
        while (_canScreenShake) {
            await Task.Delay(millisecondsDelay);
            CinemachineManager.Instance.ScreenShakeEvent(context.ScreenShakeEvent);
        }
    }

    private async void HandleDeath(int millisecondsDelay, Afrodite context) {
        await Task.Delay(millisecondsDelay);
        _canScreenShake = false;
        Object.Destroy(context.gameObject);
        Object.Instantiate(context.DeathAnim, context.transform.position, Quaternion.identity);
        GameManager.Instance.SetGameState(GameState.WinState);
    }
}