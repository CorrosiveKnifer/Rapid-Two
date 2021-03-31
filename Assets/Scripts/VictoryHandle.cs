using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryHandle : MonoBehaviour
{
    public GameObject HappyBoi;
    public GameObject SadBoi;

    public GameObject WinText;
    public GameObject LostText;

    private void Awake()
    {
        WinText.SetActive(false);
        LostText.SetActive(false);
        HappyBoi.SetActive(false);
        SadBoi.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (LevelLoader.hasWon)
        {
            WinText.SetActive(true);
            HappyBoi.SetActive(true);
        }
        else
        {
            LostText.SetActive(true);
            SadBoi.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
