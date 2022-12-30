using System;
using System.Collections.Generic;
using UnityEngine;

public class FunctionTimer {

    private static List<FunctionTimer> _activeTimersList;
    private static GameObject _initGameObject;

    private static void InitTimersList() {
        if (_initGameObject == null) {
            _initGameObject = new GameObject("FunctionTimer_InitGameObject");
            _activeTimersList = new List<FunctionTimer>();
        }
    }

    private static void RemoveTimer(FunctionTimer funcTimer) {
        InitTimersList();
        _activeTimersList.Remove(funcTimer);
    }

    private static void StopTimer(string timerDesc) {
        for (int i = 0; i < _activeTimersList.Count; i++) {
            if (_activeTimersList[i]._timerDesc == timerDesc) {
                _activeTimersList[i].DestroySelf();
                i--;
            }
        }
    }

    public static FunctionTimer Create(Action action, float timer, string timerDesc = null) {
        InitTimersList();
        GameObject hookedGameObject = new GameObject($"FunctionTimer ({timerDesc})", typeof(MonoBehaviourHook));
        FunctionTimer funcTimer = new FunctionTimer(action, timer, timerDesc, hookedGameObject);
        hookedGameObject.GetComponent<MonoBehaviourHook>().OnUpdate = funcTimer.Update;
        _activeTimersList.Add(funcTimer);
        return funcTimer;
    }

    public static void ClearTimers() {
        UnityEngine.Object.Destroy(_initGameObject);
        _activeTimersList.Clear();
    }

    public class MonoBehaviourHook : MonoBehaviour {
        public Action OnUpdate;

        private void Update() {
            if (OnUpdate != null)
                OnUpdate();
        }
    }

    private Action _action;
    private float _timer;
    private string _timerDesc;
    private bool _isDestroyed;
    private GameObject _hookedGameObject;

    private FunctionTimer(Action action, float timer, string timerDesc, GameObject hookedGameObject) {
        _action = action;
        _timer = timer;
        _timerDesc = timerDesc;
        _hookedGameObject = hookedGameObject;
        _isDestroyed = false;
    }

    public void Update() {
        if (!_isDestroyed) {
            _timer -= Time.deltaTime;
            if (_timer <= 0f) {
                _action();
                DestroySelf();
            }
        }
    }

    private void DestroySelf() {
        _isDestroyed = true;
        UnityEngine.Object.Destroy(_hookedGameObject);
        RemoveTimer(this);
    }
}