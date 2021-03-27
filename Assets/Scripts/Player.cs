using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// William de Beer, Michael Jordan
/// </summary>

enum Spell
{
    Fireball,
    SpellA,
    SpellB,
    None
}


public class Player : MonoBehaviour
{
    Spell m_SelectedSpell;
    GameObject m_SelectedTower;
    public Color m_ColourDim;
    public LayerMask m_SelectableMask;

    [Header("Cursors")]
    public Texture2D m_CursorOn;
    public Texture2D m_CursorOff;

    [Header("Spells")]
    public GameObject m_Fireball;
    float m_fFireballCoolDown = 2.0f;
    float m_fFireballTimeLeft = 0.0f;

    public GameObject m_FrostRing;
    float m_fFrostRingCoolDown = 2.0f;
    float m_fFrostRingTimeLeft = 0.0f;

    public GameObject m_SpellB;
    float m_fSpellBCoolDown = 2.0f;
    float m_fSpellBTimeLeft = 0.0f;


    [Header("Towers")]
    public GameObject m_BasicTower;
    int m_iBasicTowerCost = 10;

    public GameObject m_FireTower;
    int m_iFireTowerCost = 15;

    public GameObject m_FrostTower;
    int m_iFrostTowerCost = 20;

    public GameObject m_LaserTower;
    int m_iLaserTowerCost = 25;



    [Header("Mana")]
    public float fManaMaximum = 100.0f;
    public float fManaPool = 100.0f;
    public float fManaRegen = 5.0f;

    [Header("Mana Cost")]
    public float fFireballCost = 30.0f;

    [Header("Values")]
    public float fCameraMoveSpeed = 0.3f;
    public float fCameraZoomSpeed = 0.3f;
    public float fCameraMaxZoom = 15.0f;

    [Header("Attachements")]
    public Camera m_Camera;
    public GameObject m_Marker;

    public GameObject m_Minion;
    private GameObject m_selected;

    // Start is called before the first frame update
    void Start()
    {
        m_SelectedSpell = Spell.None;

        GameManager.instance.Tower1.GetComponentInChildren<Text>().text = m_iBasicTowerCost.ToString();
        GameManager.instance.Tower2.GetComponentInChildren<Text>().text = m_iFireTowerCost.ToString();
        GameManager.instance.Tower3.GetComponentInChildren<Text>().text = m_iFrostTowerCost.ToString();
        GameManager.instance.Tower4.GetComponentInChildren<Text>().text = m_iLaserTowerCost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Mana update
        fManaPool += Time.deltaTime * fManaRegen;
        if (fManaPool > fManaMaximum)
        {
            fManaPool = fManaMaximum;
        }

        GameManager.instance.SetMana(fManaPool);

        // Fireball cooldown
        if (m_fFireballTimeLeft > 0.0f)
        {
            m_fFireballTimeLeft -= Time.deltaTime;
            if (m_fFireballTimeLeft < 0.0f)
            {
                m_fFireballTimeLeft = 0.0f;
            }
            m_Marker.GetComponentInChildren<SpriteRenderer>().color = m_ColourDim;
        }
        else
        {
            m_Marker.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }

        if (m_fFrostRingTimeLeft > 0.0f)
        {
            m_fFrostRingTimeLeft -= Time.deltaTime;
            if (m_fFrostRingTimeLeft < 0.0f)
            {
                m_fFrostRingTimeLeft = 0.0f;
            }
            m_Marker.GetComponentInChildren<SpriteRenderer>().color = m_ColourDim;
        }
        else
        {
            m_Marker.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }

        // Player movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move player
        transform.position += (transform.right * x + transform.forward * z) * fCameraMoveSpeed * m_Camera.orthographicSize / fCameraMaxZoom;

        // Camera zoom
        m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize - Input.mouseScrollDelta.y * fCameraZoomSpeed, 1, fCameraMaxZoom);

        AbilitySelect();

        RaycastHit[] hits;

        // Raycast
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1500.0f, Color.red, 0.5f);
        hits = Physics.RaycastAll(ray.origin, ray.direction, 1500.0f);

        if (hits.Length != 0) // Check if raycast detects any objects
        {
            RaycastHit closestHit = hits[0];

            for (int i = 0; i < hits.Length; i++)
            {
                if (closestHit.distance > hits[i].distance && hits[i].collider.gameObject != m_Marker)
                    closestHit = hits[i];
            }

            if (m_SelectedSpell != Spell.None)
            {
                // Activate spell blast marker
                m_Marker.SetActive(true);
                m_Marker.transform.position = closestHit.point;
            }
            else
            {
                m_Marker.SetActive(false);
            }

            HandleRayCastHit(closestHit);
        }

        CursorSelect();

        if (m_selected != null)
        {
            if (m_selected.tag == "Minion") // Check if minion is currently selected and move frame to minion on hotbar
            {
                GameManager.instance.EnableFrame(true);
            }
            else
            {
                GameManager.instance.EnableFrame(false);
            }
        }
        else if (m_SelectedSpell == Spell.None && m_SelectedTower == null)
        {
            GameManager.instance.EnableFrame(false);
        }
        else
        {
            GameManager.instance.EnableFrame(true);
        }

    }

    private void HandleRayCastHit(RaycastHit hit)
    { 
        if (hit.collider.gameObject.layer == 9) // Check if object is selectable
        {
            // Set cursor to hovering
            Cursor.SetCursor(m_CursorOn, new Vector2(16, 16), CursorMode.Auto);
        }
        else
        {
            // Set cursor to not hovering
            Cursor.SetCursor(m_CursorOff, new Vector2(16, 16), CursorMode.Auto);
        }

        if (m_SelectedSpell == Spell.None) // Check if spell is selected
        {
            // Select minion
            MinionScript minion = hit.collider.gameObject.GetComponentInChildren<MinionScript>();
            if ((minion != null && Input.GetMouseButtonDown(0))) // Select minion with click
            {
                MinionSelect();
                return;
            }
            if (m_selected != null)
            {
                if (Input.GetMouseButtonDown(1) && m_selected.tag == "Minion") // Create target location for minion
                {
                    m_selected.GetComponent<MinionScript>().SetTargetLocation(hit.point);
                    Debug.Log(hit.point);
                    return;
                }
            }

            // Select tower
            TowerScript tower = hit.collider.gameObject.GetComponentInChildren<TowerScript>();
            if (tower != null && Input.GetMouseButtonDown(0))
            {
                TowerSelect(hit);
                return;
            }

            TurretPlot plot = hit.collider.gameObject.GetComponentInChildren<TurretPlot>();
            if (plot != null && Input.GetMouseButtonDown(0)) // Select turret plot
            {
                PlotSelect(plot);
                return;
            }

        }

        if (Input.GetMouseButtonDown(0)) // Cast spell
        {
            SpellCast(hit.point);
            DeselectObject();
            return;
        }
    }
    private void MinionSelect()
    {
        m_SelectedSpell = Spell.None;
        if (m_selected != null)
        {
            if (m_selected.tag == "Minion")
            {
                DeselectObject();
                m_selected = null;
                GameManager.instance.SelectFrame.GetComponent<Image>().enabled = false;
            }
        }
        else
        {
            GameManager.instance.SelectFrame.GetComponent<Image>().enabled = true;
            GameManager.instance.MoveFrame(GameManager.instance.Minion.GetComponent<RectTransform>());
            m_selected = m_Minion;
            m_selected.GetComponent<MinionScript>().SetSelected(true);
        }
    }
    private void TowerSelect(RaycastHit hit)
    {
        DeselectObject();
        m_selected = hit.collider.gameObject;
        m_selected.GetComponent<TowerScript>().SetSelected(true);
    }
    private void PlotSelect(TurretPlot plot)
    {
        DeselectObject();
        if (m_SelectedTower != null)
        {
            plot.SpawnTurret(m_SelectedTower);
        }
        if (plot.m_AttachedTurret != null)
        {
            m_selected = plot.m_AttachedTurret;
            m_selected.GetComponent<TowerScript>().SetSelected(true);
        }
    }
    private void AbilitySelect()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Fireball
        {
            if (m_SelectedSpell != Spell.Fireball)
            {
                m_SelectedTower = null;
                m_SelectedSpell = Spell.Fireball;
            }
            else
            {
                m_SelectedSpell = Spell.None;
            }
            DeselectObject();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // Basic Tower
        {
            m_SelectedSpell = Spell.None;
            if (m_SelectedTower != m_BasicTower)
            {
                m_SelectedTower = m_BasicTower;
            }
            else
            {
                m_SelectedTower = null;
            }
            DeselectObject();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Fire Tower
        {
            m_SelectedSpell = Spell.None;
            if (m_SelectedTower != m_FireTower)
            {
                m_SelectedTower = m_FireTower;
            }
            else
            {
                m_SelectedTower = null;
            }
            DeselectObject();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) // Frost Tower
        {
            m_SelectedSpell = Spell.None;
            if (m_SelectedTower != m_FrostTower)
            {
                m_SelectedTower = m_FrostTower;
            }
            else
            {
                m_SelectedTower = null;
            }
            DeselectObject();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) // Laser Tower
        {
            m_SelectedSpell = Spell.None;
            if (m_SelectedTower != m_LaserTower)
            {
                m_SelectedTower = m_LaserTower;
            }
            else
            {
                m_SelectedTower = null;
            }
            DeselectObject();
        }


        if (Input.GetKeyDown(KeyCode.F)) // Select minion with keypress
        {
            MinionSelect();
        }



    }
    private void CursorSelect()
    {
        if (m_SelectedTower == m_BasicTower)
        {
            GameManager.instance.EnableFrame(true);
            GameManager.instance.MoveFrame(GameManager.instance.Tower1.GetComponent<RectTransform>());
        }
        else if (m_SelectedTower == m_FireTower)
        {
            GameManager.instance.EnableFrame(true);
            GameManager.instance.MoveFrame(GameManager.instance.Tower2.GetComponent<RectTransform>());
        }
        else if (m_SelectedTower == m_FrostTower)
        {
            GameManager.instance.EnableFrame(true);
            GameManager.instance.MoveFrame(GameManager.instance.Tower3.GetComponent<RectTransform>());

        }
        else if (m_SelectedTower == m_LaserTower)
        {
            GameManager.instance.EnableFrame(true);
            GameManager.instance.MoveFrame(GameManager.instance.Tower4.GetComponent<RectTransform>());
        }
        switch (m_SelectedSpell)
        {
            case Spell.Fireball:
                GameManager.instance.EnableFrame(true);
                GameManager.instance.MoveFrame(GameManager.instance.Spell1.GetComponent<RectTransform>());
                break;
            case Spell.SpellA:
                GameManager.instance.EnableFrame(true);
                GameManager.instance.MoveFrame(GameManager.instance.Spell1.GetComponent<RectTransform>());
                break;
            case Spell.SpellB:
                GameManager.instance.EnableFrame(true);
                GameManager.instance.MoveFrame(GameManager.instance.Spell1.GetComponent<RectTransform>());
                break;
        }
        
    }
    private void DeselectObject()
    {
        if (m_selected != null)
        {
            if (m_selected.tag == "Minion")
                m_selected.GetComponent<MinionScript>().SetSelected(false);

            if (m_selected.tag == "Tower")
                m_selected.GetComponent<TowerScript>().SetSelected(false);


        }
        m_selected = null;
    }
    private void SpellCast(Vector3 _targetPos)
    {
        switch (m_SelectedSpell)
        {
            case Spell.Fireball:
                if (m_fFireballTimeLeft == 0.0f && fFireballCost <= fManaPool)
                {
                    Instantiate(m_Fireball, _targetPos + Vector3.up * 20.0f, Quaternion.identity);
                    m_fFireballTimeLeft = m_fFireballCoolDown;
                    fManaPool -= fFireballCost;
                }
                break;
        }
    }
    public float GetMana()
    {
        return fManaPool;
    }
}
