using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            Cursor.lockState = CursorLockMode.Confined; 
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

    [Header("Hotbar")]
    public GameObject Spell1;
    public GameObject Spell2;
    public GameObject Spell3;
    public GameObject Tower1;
    public GameObject Tower2;
    public GameObject Tower3;
    public GameObject Tower4;
    public GameObject TowerDestroy;

    public GameObject Minion;
    public GameObject Demon;

    public Slider LifeBar;
    public Slider ManaBar;
    public Slider MinionBloodBar;
    public Slider RockHealth;

    public GameObject SelectFrame;
    public LevelLoader levelLoader;

    //Volume Settings
    public static float MasterVolume { get; set; } = 1.0f;
    public static float SoundEffectVolume { get; set; } = 1.0f;
    public static float BackGroundVolume { get; set; } = 1.0f;

    public double GameTime = 0.0;

    public GameObject BasicEnemy;

    public Player player;

    public float maxVolumeDistance = 50.0f;
    public int lives = 100;
    public float blood;

    public Text BloodDisplay;
    public Text WaveCount;
    public Text EnemyCount;

    public GameObject NextWave;
    public GameObject ActivePos;
    public GameObject DeactivePos;

    float m_fLerpVal = 0;

    private Vector3 middlePoint;

    private void Start()
    {
        LevelLoader.hasWon = false;

        Physics.IgnoreLayerCollision(8, 8); //Enemy Ignore Enemy

    }

    void UpdateTooltips()
    {
        //Spell1.GetComponentInChildren<TooltipTrigger>().cost = Spell1.GetComponentInChildren<Text>().text;
        //Spell2.GetComponentInChildren<TooltipTrigger>().cost = Spell2.GetComponentInChildren<Text>().text;
        Spell3.GetComponentInChildren<TooltipTrigger>().cost = Spell3.GetComponentInChildren<Text>().text;
        Tower1.GetComponentInChildren<TooltipTrigger>().cost = Tower1.GetComponentInChildren<Text>().text;
        Tower2.GetComponentInChildren<TooltipTrigger>().cost = Tower2.GetComponentInChildren<Text>().text;
        Tower3.GetComponentInChildren<TooltipTrigger>().cost = Tower3.GetComponentInChildren<Text>().text;
        Tower4.GetComponentInChildren<TooltipTrigger>().cost = Tower4.GetComponentInChildren<Text>().text;
    }
    // Update is called once per frame
    void Update()
    {
        GameTime += Time.deltaTime;

        if(BloodDisplay != null)
            BloodDisplay.text = $"{Mathf.FloorToInt(blood)}";

        middlePoint = RayCastToMiddlePoint();

        SetLifeBar(lives);
        if (WaveCount != null)
            WaveCount.text = $"Wave: {Mathf.FloorToInt(GetComponent<WaveManager>().waveIndex)}";

        if (EnemyCount != null)
            EnemyCount.text = $"Enemies Left: {Mathf.FloorToInt(GameObject.FindGameObjectsWithTag("Enemy").Length)}";

        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            NextWave.SetActive(true);
        }
        else
        {
            NextWave.SetActive(false);
        }


        if (lives <= 0)
        {
            levelLoader.LoadNextLevel();
        }

        UpdateTooltips();
    }
    public float CalculateVolumeModifier(Vector3 soundPos)
    {
        Vector2 mid = new Vector3(middlePoint.x, middlePoint.z);
        Vector2 pos = new Vector3(soundPos.x, soundPos.z);
        float distance = Vector2.Distance(pos, mid);
        float height = player.GetScreenSizeRatio();

        return height * Mathf.Clamp(1.0f - distance/maxVolumeDistance, 0.0f, 1.0f);
    }

    private Vector3 RayCastToMiddlePoint()
    {
        Ray ray = player.GetComponentInChildren<Camera>().ScreenPointToRay(new Vector3(Screen.width/2.0f,Screen.height/2.0f, 0.0f));
        Debug.DrawRay(ray.origin, ray.direction * 1500.0f, Color.red, 0.5f);

        RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, 1500.0f);
        if (hits.Length != 0) // Check if raycast detects any objects
        {
            RaycastHit closestHit = hits[0];

            for (int i = 0; i < hits.Length; i++)
            {
                if (closestHit.distance > hits[i].distance)
                    closestHit = hits[i];
            }
            return closestHit.point;
        }

        return new Vector3();
    }

    public void MoveFrame(RectTransform _transform)
    {
        SelectFrame.GetComponent<RectTransform>().position = _transform.position;
    }

    public void EnableFrame(bool _visible)
    {
        SelectFrame.GetComponent<Image>().enabled = _visible;
    }

    public void SetLifeBar(float _life)
    {
        LifeBar.value = _life;
    }

    public void SetMana(float _mana)
    {
        ManaBar.value = _mana;
    }

    public void SetMinionBlood(float _blood)
    {
        MinionBloodBar.value = _blood;
    }

    public void SetRockHealth(float _health)
    {
        RockHealth.value = _health;
    }
}
