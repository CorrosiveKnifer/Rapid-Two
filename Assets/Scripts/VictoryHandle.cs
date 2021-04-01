using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryHandle : MonoBehaviour
{
    public GameObject WinText;
    public GameObject LostText;
    public LevelLoader LevelLoader;

    private void Awake()
    {
        WinText.SetActive(false);
        LostText.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] source = LevelLoader.GetComponents<AudioSource>();
        if (LevelLoader.hasWon)
        {
            WinText.SetActive(true);
            source[0].Play();
        }
        else
        {
            LostText.SetActive(true);
            source[1].Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
