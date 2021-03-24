using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Michael Jordan
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Second Instance of GameManager was created, this instance was destroyed.");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    #endregion

    //Volume Settings
    public static float MasterVolume { get; set; } = 1.0f;
    public static float SoundEffectVolume { get; set; } = 1.0f;
    public static float BackGroundVolume { get; set; } = 1.0f;

    public double GameTime = 0.0;

    public GameObject BasicEnemy;

    public SpawnerScript[] WorldSpawners;

    public int waveSize;
    public int lives = 100;
    public float blood;
    public Text BloodDisplay;
    private void Start()
    {
        Physics.IgnoreLayerCollision(8, 8); //Enemy Ignore Enemy
    }

    // Update is called once per frame
    void Update()
    {
        GameTime += Time.deltaTime;

        if(BloodDisplay != null)
            BloodDisplay.text = $"Blood: {Mathf.FloorToInt(blood)}";

        if (WorldSpawners.Length > 0)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < waveSize)
            {
                foreach (var spawner in WorldSpawners)
                {
                    spawner.SpawnGameObject(BasicEnemy);
                }
            }
        }
    }
}
