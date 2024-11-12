using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Storyboard
{
    public static void Win() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public static void Lose() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
