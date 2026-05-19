using System.Collections;
using System.Collections. Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMarkerless()
    {
        SceneManager. LoadScene(1);
    }

    public void LoadMarkerBased()
    {
        SceneManager. LoadScene(2);
    }

    public void QuitApplication()
    {
        Debug.Log("Application Quit");

        Application.Quit();
    }
}