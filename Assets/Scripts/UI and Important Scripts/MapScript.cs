using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Michael Jordan
/// </summary>
public class MapScript : MonoBehaviour
{
    public RawImage MiniMap;
    public GameObject BigMap;

    private bool ShowMap = false;
    // Start is called before the first frame update
    void Start()
    {
        MiniMap.enabled = true;
        BigMap.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            ShowMap = !ShowMap;
        }

        MiniMap.enabled = !ShowMap;
        BigMap.SetActive(ShowMap);
    }
}
