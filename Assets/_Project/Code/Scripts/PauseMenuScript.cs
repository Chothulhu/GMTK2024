using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public static bool isPaused;
    [SerializeField] private GameObject canvasGameObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseGame();
        }
    }

    void PauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
            canvasGameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            canvasGameObject.SetActive(false);
        }
    }
}
