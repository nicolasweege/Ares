using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfroditeFourthStageState : AfroditeBaseState
{
    private Vector2 _currentMovePoint;

    public override void EnterState(AfroditeController context)
    {
        if (PlayerMainShipController.Instance.transform.position.x > 0f)
        {
            _currentMovePoint = context.FourthStageMovePointLeft.position;
        }
        else
        {
            _currentMovePoint = context.FourthStageMovePointRight.position;
        }
    }

    public override void UpdateState(AfroditeController context)
    {
        HandleMovement(context);
    }

    private void HandleMovement(AfroditeController context)
    {
        if (PlayerMainShipController.Instance == null)
            return;

        context.transform.position = Vector2.SmoothDamp(context.transform.position, _currentMovePoint, ref context.Velocity, context.Speed);
    }
}