using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  William
/// </summary>
public class TurretPlot : MonoBehaviour
{
    bool m_bHasTurret = false;
    public Transform m_SpawnLoc;
    public GameObject m_AttachedTurret;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTurret(GameObject _turret)
    {
        if (!m_bHasTurret)
        {
            m_AttachedTurret = Instantiate(_turret, m_SpawnLoc.position, Quaternion.identity);
            m_bHasTurret = true;
            Debug.Log("Spawned Turret");
        }
    }

    public void DestroyTurret()
    {
        if (m_bHasTurret)
        {
            Destroy(m_AttachedTurret);
            m_bHasTurret = false;
        }
    }
}
