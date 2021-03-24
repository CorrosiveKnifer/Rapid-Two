using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlot : MonoBehaviour
{
    bool m_bHasTurret = false;
    public Transform m_SpawnLoc;
    GameObject m_AttachedTurret;

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
        // TEMP
        if (!m_bHasTurret)
        {
            m_AttachedTurret = Instantiate(_turret, m_SpawnLoc.position, Quaternion.identity);
            m_bHasTurret = true;
            Debug.Log("Spawned Turret");
        }
        else
        {
            Destroy(m_AttachedTurret);
            m_bHasTurret = false;
        }
    }
}
