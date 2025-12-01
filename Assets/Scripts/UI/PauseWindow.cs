using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseWindow : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
