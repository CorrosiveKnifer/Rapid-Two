using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryHandle : MonoBehaviour
{
    public GameObject WinText;
    public GameObject LostText;

    private void Awake()
    {
        WinText.SetActive(false);
        LostText.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (LevelLoader.hasWon)
        {
            WinText.SetActive(true);
        }
        else
        {
            LostText.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
