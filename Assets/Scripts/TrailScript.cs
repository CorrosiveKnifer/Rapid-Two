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
        index = Mathf.Clamp(index, 0, waypoints.Length - 1);

        return waypoints[index].transform;
    }
    public Transform GetClosestWaypointTo(Vector3 position, out int index)
    {
        int closest = 0;
        float distance = Vector3.Distance(waypoints[0].transform.position, position);
        for (int i = 1; i < waypoints.Length; i++)
        {
            float testDistance = Vector3.Distance(waypoints[i].transform.position, position);
            if (testDistance < distance)
            {
                distance = testDistance;
                closest = i;
            }
        }

        index = closest;
        return waypoints[index].transform;
    }
}
