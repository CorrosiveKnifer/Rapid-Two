using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public class TrailScript : MonoBehaviour
{
    public WaypointScript[] waypoints;

    public int waypointCount;

    // Start is called before the first frame update
    void Start()
    {
        waypointCount = waypoints.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetWaypointLocation(int index)
    {
        if (index > waypoints.Length)
            index = waypoints.Length - 1;

        return waypoints[index].transform;
    }
}
