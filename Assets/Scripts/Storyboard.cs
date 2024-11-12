using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Storyboard
{
    public static void Win() {
        SceneManager.LoadSceneAsync("Win");
    }
    public static void Lose() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
