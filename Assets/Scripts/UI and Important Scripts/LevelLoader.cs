using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// William de Beer
/// </summary>
public class LevelLoader : MonoBehaviour
{
    public static bool hasWon = false;
    public static bool cheatsEnabled = false;

    public Toggle cheatToggle;

    public Animator transition;

    public float transitionTime = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MenuScreen") // Back to menu
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine(LoadLevel(0));
            }

            if (cheatsEnabled)
            {
                CheatInputs();
            }
        }

        if (cheatToggle != null)
        {
            cheatsEnabled = cheatToggle.isOn;
        }

        if (Input.GetKeyDown(KeyCode.O)) // Reset scene
        {
            ResetScene();
        }



    }

    private void CheatInputs()
    {
        if (Input.GetKeyDown(KeyCode.L)) // Lose
        {
            hasWon = false;
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.Semicolon)) // Win
        {
            hasWon = true;
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.K)) // Kill all enemies
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponentInParent<EnemyScript>().DealDamageToEnemy(9999);
            }
        }

        if (Input.GetKeyDown(KeyCode.I)) // Gain 100 blood
        {
            GameManager.instance.blood += 100;
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
