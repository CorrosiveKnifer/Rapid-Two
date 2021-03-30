﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// William de Beer
/// </summary>
public class LevelLoader : MonoBehaviour
{
    public static bool hasWon = false;
    public Animator transition;

    public float transitionTime = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MenuScreen")
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine(LoadLevel(0));
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ResetScene();
        }

        if (Input.GetKeyDown(KeyCode.L)) // Lose lmao
        {
            hasWon = false;
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.K)) // Win lmao
        {
            hasWon = true;
            LoadNextLevel();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        if (SceneManager.sceneCountInBuildSettings <= SceneManager.GetActiveScene().buildIndex + 1) // Check if index exceeds scene count
        {
            StartCoroutine(LoadLevel(0)); // Load menu
            //SceneManager.LoadScene(0); // Load menu
        }
        else
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1)); // Loade next scene
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Loade next scene
        }
    }
    public void ResetScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // Play Animation
        transition.SetTrigger("Start");

        // Wait to let animation finish playing
        yield return new WaitForSeconds(transitionTime);

        if (levelIndex == 0 || levelIndex == SceneManager.sceneCountInBuildSettings - 1) // Check if either in menu or end screen
        {
            Cursor.lockState = CursorLockMode.None; // Make cursor usable.
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Make cursor unusable.
            Cursor.visible = false;
        }

        // Load Scene
        SceneManager.LoadScene(levelIndex);
    }
}
