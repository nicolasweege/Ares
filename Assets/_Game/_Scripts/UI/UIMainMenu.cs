using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public void PlayGame() => SceneManager.LoadScene("Afrodite Fight");

    public void QuitGame() => Application.Quit();
}