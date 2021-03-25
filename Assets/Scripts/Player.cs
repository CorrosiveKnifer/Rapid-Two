using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// William de Beer, Michael Jordan
/// </summary>

enum Spell
{
    Fireball,
    None
}

public class Player : MonoBehaviour
{
    Spell m_SelectedSpell;
    public Color m_ColourDim;

    [Header("Spells")]
    public GameObject m_Fireball;

    float m_fFireballCoolDown = 2.0f;
    float m_fFireballTimeLeft = 0.0f;

    [Header("Values")]
    public float fCameraMoveSpeed = 0.3f;
    public float fCameraZoomSpeed = 0.3f;
    public float fCameraMaxZoom = 15.0f;

    [Header("Attachements")]
    public Camera m_Camera;
    public GameObject m_Marker;

    public GameObject m_TempTurret;
    private GameObject m_selected;

    // Start is called before the first frame update
    void Start()
    {
        m_SelectedSpell = Spell.None;
    }

    // Update is called once per frame
    void Update()
    {
        // Fireball cooldown
        if (m_fFireballTimeLeft > 0.0f)
        {
            m_fFireballTimeLeft -= Time.deltaTime;
            if (m_fFireballTimeLeft < 0.0f)
            {
                m_fFireballTimeLeft = 0.0f;
            }
            m_Marker.GetComponent<SpriteRenderer>().color = m_ColourDim;
        }
        else
        {
            m_Marker.GetComponent<SpriteRenderer>().color = Color.white;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        transform.position += (transform.right * x + transform.forward * z) * fCameraMoveSpeed * m_Camera.orthographicSize / fCameraMaxZoom;

        m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize - Input.mouseScrollDelta.y * fCameraZoomSpeed, 1, fCameraMaxZoom);

        SpellSelect();

        RaycastHit[] hits;

        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50.0f, Color.red, 0.5f);
        hits = Physics.RaycastAll(ray.origin, ray.direction, 50.0f);

        if (hits.Length != 0)
        {
            RaycastHit closestHit = hits[0];

            for (int i = 0; i < hits.Length; i++)
            {
                if (closestHit.distance > hits[i].distance && hits[i].collider.gameObject != m_Marker)
                    closestHit = hits[i];
            }

            if (m_SelectedSpell != Spell.None)
            {
                m_Marker.SetActive(true);
                m_Marker.transform.position = closestHit.point;
            }
            else
            {
                m_Marker.SetActive(false);
            }

            HandleRayCastHit(closestHit);
        }

    }

    private void HandleRayCastHit(RaycastHit hit)
    {

        if (m_SelectedSpell == Spell.None)
        {
            MinionScript minion = hit.collider.gameObject.GetComponentInChildren<MinionScript>();
            if (minion != null && Input.GetMouseButtonDown(0))
            {
                m_selected = hit.collider.gameObject;
                m_selected.GetComponent<MinionScript>().SetSelected(true);
                return;
            }

            if (m_selected != null)
            {
                if (Input.GetMouseButtonDown(1) && m_selected.tag == "Minion")
                {
                    m_selected.GetComponent<MinionScript>().SetTargetLocation(hit.point);
                    Debug.Log(hit.point);
                    return;
                }
            }

            TurretPlot plot = hit.collider.gameObject.GetComponentInChildren<TurretPlot>();
            if (plot != null && Input.GetMouseButtonDown(0))
            {
                plot.SpawnTurret(m_TempTurret);
                return;
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            SpellCast(hit.point);

            if (m_selected != null)
            {
                m_selected.GetComponent<MinionScript>().SetSelected(false);
            }
            m_selected = null;
            return;
        }
    }

    private void SpellSelect()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (m_SelectedSpell != Spell.Fireball)
            {
                m_SelectedSpell = Spell.Fireball;
            }
            else
            {
                m_SelectedSpell = Spell.None;
            }
            m_selected = null;
        }
    }

    private void SpellCast(Vector3 _targetPos)
    {
        switch (m_SelectedSpell)
        {
            case Spell.Fireball:
                if (m_fFireballTimeLeft == 0.0f)
                {
                    Instantiate(m_Fireball, _targetPos + Vector3.up * 20.0f, Quaternion.identity);
                    m_fFireballTimeLeft = m_fFireballCoolDown;
                }
                break;
        }
    }
}
