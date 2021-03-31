using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject[] spawners;
    public GameObject[] enemyPrefabs;
    public string[] waveCode; //"0.5,0,0,0,0,0"

    public int waveIndex;
    public int enemyCount;
    public LevelLoader loader;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                StartNextWave();
        }

        if(waveIndex >= waveCode.Length)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                LevelLoader.hasWon = true;
                loader.LoadNextLevel();
            }
        }
    }

    public void StartNextWave()
    {
        string[] substring = waveCode[waveIndex++].Split(',');
        int[] quantity = new int[5];

        float delay = float.Parse(substring[0]);

        for (int i = 0; i < 5; i++)
            quantity[i] = int.Parse(substring[i + 1]);

        foreach (var item in spawners)
        {
            StartCoroutine(StartSpawners(item, quantity, delay));
        }
    }

    private IEnumerator StartSpawners(GameObject spawner, int[] quantity, float delay)
    {
        int size = 0;

        //Initially should be all the numbers, but gets overridden!
        int[] selection = new int[5];

        for (int i = 0; i < 5; i++)
            selection[i] = quantity[i];

        int[] indexes = new int[5];
        //Inital check
        for (int i = 0; i < selection.Length; i++)
        {
            //Check each index for if the value is not zero
            if (selection[i] > 0)
            {
                //Increment size and save the index of it.
                indexes[size++] = i;
            }
        }

        //While there atleast one index has a non zero number
        while (size > 0)
        {
            //Select the next spawner
            int spawned = Random.Range(0, size);
            GameObject prefab = enemyPrefabs[indexes[spawned]];
            selection[indexes[spawned]] -= 1; //decrease that index;

            //Spawn prefab!
            spawner.GetComponent<SpawnerScript>().SpawnGameObject(prefab);

            //Set up for next spawn
            size = 0;
            for (int i = 0; i < selection.Length; i++)
            {
                if (selection[i] > 0)
                {
                    indexes[size++] = i;
                }
            }

            //Start delay
            float time = delay;
            while(time > 0.0f)
            {
                time -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        
        yield return null;
    }
}
