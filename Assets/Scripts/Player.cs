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
    FrostRing,
    DemonSummon,
    None
}


public class Player : MonoBehaviour
{
    Spell m_SelectedSpell;
    GameObject m_SelectedTower;
    public Color m_ColourDim;
    public LayerMask m_SelectableMask;
    bool m_bDestroyMode = false;

    // Lerping
    public Vector3 m_LerpStart;
    public Vector3 m_LerpTarget;
    public bool m_isLerping = false;
    public float m_fLerpT = 0;

    [Header("Cursors")]
    public Texture2D m_CursorOn;
    public Texture2D m_CursorOff;

    [Header("Spells")]
    public GameObject m_Fireball;
    float m_fFireballCoolDown = 2.0f;
    float m_fFireballTimeLeft = 0.0f;
    public Sprite m_FireCircle;

    public GameObject m_FrostRing;
    float m_fFrostRingCoolDown = 10.0f;
    float m_fFrostRingTimeLeft = 0.0f;
    public Sprite m_FrostCircle;

    public GameObject m_SpellB;
    float m_fSpellBCoolDown = 2.0f;
    float m_fSpellBTimeLeft = 0.0f;

    [Header("Towers")]
    public GameObject m_BasicTower;
    public GameObject m_FireTower;
    public GameObject m_FrostTower;
    public GameObject m_LaserTower;

    [Header("Prices")]
    public float m_fRefund = 0.4f;
    int m_iBasicTowerCost = 10;
    int m_iFireTowerCost = 15;
    int m_iFrostTowerCost = 20;
    int m_iLaserTowerCost = 25;
    int m_iDemonCost = 200;

    [Header("Mana")]
    public float fManaMaximum = 100.0f;
    public float fManaPool = 100.0f;
    public float fManaRegen = 5.0f;

    [Header("Mana Cost")]
    public float fFireballCost = 30.0f;
    public float fFrostFieldCost = 50.0f;

    [Header("Values")]
    public float fCameraMoveSpeed = 0.3f;
    public float fCameraZoomSpeed = 0.3f;
    public float fCameraMaxZoom = 15.0f;
    float fMouseMoveSens = 5.0f;

    [Header("Camera Positon Clamp Values")]
    public float fCameraClampX1 = 30;
    public float fCameraClampX2 = -30;
    public float fCameraClampY1 = 45;
    public float fCameraClampY2 = -65;

    [Header("Attachements")]
    public Camera m_Camera;
    public GameObject m_Marker;

    public GameObject m_Harvester;
    public GameObject m_DemonPref;
    public GameObject m_Demon;
    private GameObject m_selected;


    // Start is called before the first frame update
    void Start()
    {
        m_SelectedSpell = Spell.None;
        GameManager.instance.Tower1.GetComponentInChildren<Text>().text = m_iBasicTowerCost.ToString();
        GameManager.instance.Tower2.GetComponentInChildren<Text>().text = m_iFireTowerCost.ToString();
        GameManager.instance.Tower3.GetComponentInChildren<Text>().text = m_iFrostTowerCost.ToString();
        GameManager.instance.Tower4.GetComponentInChildren<Text>().text = m_iLaserTowerCost.ToString();
        GameManager.instance.Spell3.GetComponentInChildren<Text>().text = m_iDemonCost.ToString();
    }
    private void CoolDowns()
    {
        // Fireball cooldown
        GameManager.instance.Spell1.GetComponentInChildren<Slider>().value = (m_fFireballTimeLeft/m_fFireballCoolDown);
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

        // Frostring cooldown
        GameManager.instance.Spell2.GetComponentInChildren<Slider>().value = (m_fFrostRingTimeLeft / m_fFrostRingCoolDown);
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

        switch (m_SelectedSpell)
        {
            case Spell.Fireball:
                m_Marker.GetComponentInChildren<SpriteRenderer>().sprite = m_FireCircle;
                if (m_fFireballTimeLeft > 0.0f)
                {
                    m_Marker.GetComponentInChildren<SpriteRenderer>().color = m_ColourDim;
                }
                else
                {
                    m_Marker.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                }
                    break;
            case Spell.FrostRing:
                m_Marker.GetComponentInChildren<SpriteRenderer>().sprite = m_FrostCircle;
                if (m_fFrostRingTimeLeft > 0.0f)
                {
                    m_Marker.GetComponentInChildren<SpriteRenderer>().color = m_ColourDim;
                }
                else
                {
                    m_Marker.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                }
                break;
            case Spell.DemonSummon:
                break;

        }
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

        CoolDowns();

        // Player movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move player with keyboard
        transform.position += (transform.right * x + transform.forward * z) * fCameraMoveSpeed * m_Camera.orthographicSize / fCameraMaxZoom;
        if (x != 0 || z != 0)
        {
            m_isLerping = false;
            m_fLerpT = 0;
        }

        x = 0;
        z = 0;

        if (Input.mousePosition.x < fMouseMoveSens)
        {
            x = -1;
        }
        if (Input.mousePosition.x > (Screen.width - fMouseMoveSens))
        {
            x = 1;
        }
        if (Input.mousePosition.y < fMouseMoveSens)
        {
            z = -1;
        }
        if (Input.mousePosition.y > (Screen.height - fMouseMoveSens))
        {
            z = 1;
        }

        if (x != 0 || z != 0)
        {
            m_isLerping = false; 
            m_fLerpT = 0;
        }

        if (m_isLerping)
        {
            transform.position = Vector3.Lerp(transform.position, m_LerpTarget, m_fLerpT);
            m_fLerpT += Time.deltaTime;
            if (m_fLerpT > 1.0f)
            {
                m_fLerpT = 1.0f;
            }
        }
        else
        {
            m_fLerpT = 0.0f;
        }

        transform.position += (transform.right * x + transform.forward * z) * fCameraMoveSpeed * 1.5f * m_Camera.orthographicSize / fCameraMaxZoom;

        if (transform.position.x > fCameraClampX1)
        {
            transform.position = new Vector3(30, transform.position.y, transform.position.z);
        }
        if (transform.position.x < fCameraClampX2)
        {
            transform.position = new Vector3(-30, transform.position.y, transform.position.z);
        }
        if (transform.position.z > fCameraClampY1)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 45);
        }
        if (transform.position.z < fCameraClampY2)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -65);
        }

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
        else if (m_SelectedSpell == Spell.None && m_SelectedTower == null && !m_bDestroyMode)
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
        if (m_SelectedSpell == Spell.None)
        {
            Cursor.visible = true;
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
        }
        else
        {
            Cursor.visible = false;
        }
        if (m_SelectedSpell == Spell.None) // Check if spell is selected
        {
            // Select minion
            HarvesterScript harvester = hit.collider.gameObject.GetComponentInChildren<HarvesterScript>();
            if ((harvester != null && Input.GetMouseButtonDown(0))) // Select minion with click
            {
                MinionSelect();
                return;
            }
            if (m_selected != null)
            {
                if (Input.GetMouseButtonDown(1) && m_selected.GetComponent<HarvesterScript>() != null) // Create target location for minion
                {
                    m_selected.GetComponent<HarvesterScript>().SetTargetLocation(hit.point);
                    Debug.Log(hit.point);
                    return;
                }
            }

            // Select Demon
            DemonScript demon = hit.collider.gameObject.GetComponentInChildren<DemonScript>();
            if ((demon != null && Input.GetMouseButtonDown(0))) // Select minion with click
            {
                DemonSelect();
                return;
            }
            if (m_selected != null)
            {
                if (Input.GetMouseButtonDown(1) && m_selected.GetComponent<DemonScript>() != null) // Create target location for minion
                {
                    m_selected.GetComponent<DemonScript>().SetTargetLocation(hit.point);
                    Debug.Log(hit.point);
                    return;
                }
            }

            // Select tower
            TowerScript tower1 = hit.collider.gameObject.GetComponentInChildren<TowerScript>();
            BombTowerScript tower2 = hit.collider.gameObject.GetComponentInChildren<BombTowerScript>();
            IceTowerScript tower3 = hit.collider.gameObject.GetComponentInChildren<IceTowerScript>();
            LaserTowerScript tower4 = hit.collider.gameObject.GetComponentInChildren<LaserTowerScript>();
            if ((tower1 != null || tower2 != null || tower3 != null || tower4 != null)  && Input.GetMouseButtonDown(0))
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
        if (m_selected != null && m_selected.gameObject.GetComponent<HarvesterScript>() != null)
        {
            //DeselectObject();
            //m_selected = null;
            //GameManager.instance.SelectFrame.GetComponent<Image>().enabled = false;
            m_isLerping = true;
            m_LerpTarget = new Vector3(m_selected.gameObject.transform.position.x, transform.position.y, m_selected.gameObject.transform.position.z) - transform.forward * 10.0f;
        }
        else
        {
            GameManager.instance.SelectFrame.GetComponent<Image>().enabled = true;
            GameManager.instance.MoveFrame(GameManager.instance.Minion.GetComponent<RectTransform>());
            m_selected = m_Harvester;
            m_selected.GetComponent<HarvesterScript>().SetSelected(true);
        }
    }

    private void DemonSelect()
    {
        m_SelectedSpell = Spell.None;
        if (m_selected != null && m_selected.gameObject.GetComponent<DemonScript>() != null)
        {
            //DeselectObject();
            //m_selected = null;
            //GameManager.instance.SelectFrame.GetComponent<Image>().enabled = false;
            //transform.position = new Vector3(m_selected.gameObject.transform.position.x, transform.position.y, m_selected.gameObject.transform.position.z) - transform.forward * 10.0f;
            m_isLerping = true;
            m_LerpTarget = new Vector3(m_selected.gameObject.transform.position.x, transform.position.y, m_selected.gameObject.transform.position.z) - transform.forward * 10.0f;
        }
        else
        {
            if (m_Demon == null) // Summon new demon.
            {
                if (m_iDemonCost <= GameManager.instance.blood)
                {
                    GameManager.instance.blood -= m_iDemonCost;
                    m_Demon = Instantiate(m_DemonPref, new Vector3(-12.78f, 6.0f, -26.78f), Quaternion.identity);
                    GameManager.instance.SelectFrame.GetComponent<Image>().enabled = true;
                    GameManager.instance.MoveFrame(GameManager.instance.Demon.GetComponent<RectTransform>());
                    m_selected = m_Demon;
                    m_selected.GetComponent<DemonScript>().SetSelected(true);
                    m_isLerping = true;
                    m_LerpTarget = new Vector3(m_selected.gameObject.transform.position.x, transform.position.y, m_selected.gameObject.transform.position.z) - transform.forward * 10.0f;
                }
            }
            else
            {
                GameManager.instance.SelectFrame.GetComponent<Image>().enabled = true;
                GameManager.instance.MoveFrame(GameManager.instance.Demon.GetComponent<RectTransform>());
                m_selected = m_Demon;
                m_selected.GetComponent<DemonScript>().SetSelected(true);
            }
        }
    }
    private void TowerSelect(RaycastHit hit)
    {
        DeselectObject();
        if (m_bDestroyMode)
        {
            if (hit.collider.gameObject.GetComponentInChildren<TowerScript>() != null)
            {
                GameManager.instance.blood += m_iBasicTowerCost * m_fRefund;
            }
            else if (hit.collider.gameObject.GetComponentInChildren<BombTowerScript>() != null)
            {
                GameManager.instance.blood += m_iFireTowerCost * m_fRefund;
            }
            else if (hit.collider.gameObject.GetComponentInChildren<IceTowerScript>() != null)
            {
                GameManager.instance.blood += m_iFrostTowerCost * m_fRefund;
            }
            else if (hit.collider.gameObject.GetComponentInChildren<LaserTowerScript>() != null)
            {
                GameManager.instance.blood += m_iLaserTowerCost * m_fRefund;
            }
            else
            {
                Debug.Log("No Turret");
            }
            Destroy(hit.collider.gameObject);
            return;
        }

        m_selected = hit.collider.gameObject;
        m_selected.GetComponent<TowerScript>()?.SetSelected(true);
        m_selected.GetComponent<BombTowerScript>()?.SetSelected(true);
        m_selected.GetComponent<IceTowerScript>()?.SetSelected(true);
        m_selected.GetComponent<LaserTowerScript>()?.SetSelected(true);
    }
    private void PlotSelect(TurretPlot plot)
    {
        DeselectObject();
        if (m_bDestroyMode)
        {
            if (plot.m_AttachedTurret != null)
            {
                if (plot.m_AttachedTurret.GetComponentInChildren<TowerScript>() != null)
                {
                    GameManager.instance.blood += m_iBasicTowerCost * m_fRefund;
                }
                else if (plot.m_AttachedTurret.GetComponentInChildren<BombTowerScript>() != null)
                {
                    GameManager.instance.blood += m_iFireTowerCost * m_fRefund;
                }
                else if (plot.m_AttachedTurret.GetComponentInChildren<IceTowerScript>() != null)
                {
                    GameManager.instance.blood += m_iFrostTowerCost * m_fRefund;
                }
                else if (plot.m_AttachedTurret.GetComponentInChildren<LaserTowerScript>() != null)
                {
                    GameManager.instance.blood += m_iLaserTowerCost * m_fRefund;
                }
                else
                {
                    Debug.Log("No Turret");
                }
                plot.DestroyTurret();
            }
            return;
        }
        if (m_SelectedTower != null)
        {
            if (m_SelectedTower == m_BasicTower && m_iBasicTowerCost > GameManager.instance.blood)
            {
                return;
            }
            else if (m_SelectedTower == m_BasicTower)
            {
                GameManager.instance.blood -= m_iBasicTowerCost;
            }
            if (m_SelectedTower == m_FireTower && m_iFireTowerCost > GameManager.instance.blood)
            {
                return;
            }
            else if (m_SelectedTower == m_FireTower)
            {
                GameManager.instance.blood -= m_iFireTowerCost;
            }
            if (m_SelectedTower == m_FrostTower && m_iFrostTowerCost > GameManager.instance.blood)
            {
                return;
            }
            else if (m_SelectedTower == m_FrostTower)
            {
                GameManager.instance.blood -= m_iFrostTowerCost;
            }
            if (m_SelectedTower == m_LaserTower && m_iLaserTowerCost > GameManager.instance.blood)
            {
                return;
            }
            else if (m_SelectedTower == m_LaserTower)
            {
                GameManager.instance.blood -= m_iLaserTowerCost;
            }

            plot.SpawnTurret(m_SelectedTower);
        }


        if (plot.m_AttachedTurret != null)
        {
            m_SelectedSpell = Spell.None;
            m_SelectedTower = null;

            m_selected = plot.m_AttachedTurret;
            m_selected.GetComponent<TowerScript>()?.SetSelected(true);
            m_selected.GetComponent<BombTowerScript>()?.SetSelected(true);
            m_selected.GetComponent<IceTowerScript>()?.SetSelected(true);
            m_selected.GetComponent<LaserTowerScript>()?.SetSelected(true);
        }
    }
    private void AbilitySelect()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Fireball
        {
            m_bDestroyMode = false;
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
        if (Input.GetKeyDown(KeyCode.E)) // Frost Ring
        {
            m_bDestroyMode = false;
            if (m_SelectedSpell != Spell.FrostRing)
            {
                m_SelectedTower = null;
                m_SelectedSpell = Spell.FrostRing;
            }
            else
            {
                m_SelectedSpell = Spell.None;
            }
            DeselectObject();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // Basic Tower
        {
            m_bDestroyMode = false;
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
            m_bDestroyMode = false;
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
            m_bDestroyMode = false;
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
            m_bDestroyMode = false;
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
        if (Input.GetKeyDown(KeyCode.Alpha5)) // Laser Tower
        {
            m_SelectedSpell = Spell.None;
            m_SelectedTower = null;
            DeselectObject();

            if (m_bDestroyMode)
            {
                m_bDestroyMode = false;
            }
            else
            {
                m_bDestroyMode = true;
            }
        }


        if (Input.GetKeyDown(KeyCode.F)) // Select minion with keypress
        {
            MinionSelect();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            DemonSelect();
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
        else if (m_bDestroyMode)
        {
            GameManager.instance.EnableFrame(true);
            GameManager.instance.MoveFrame(GameManager.instance.TowerDestroy.GetComponent<RectTransform>());
        }
        switch (m_SelectedSpell)
        {
            case Spell.Fireball:
                GameManager.instance.EnableFrame(true);
                GameManager.instance.MoveFrame(GameManager.instance.Spell1.GetComponent<RectTransform>());
                break;
            case Spell.FrostRing:
                GameManager.instance.EnableFrame(true);
                GameManager.instance.MoveFrame(GameManager.instance.Spell2.GetComponent<RectTransform>());
                break;
            case Spell.DemonSummon:
                GameManager.instance.EnableFrame(true);
                GameManager.instance.MoveFrame(GameManager.instance.Spell3.GetComponent<RectTransform>());
                break;
        }
        
    }
    private void DeselectObject()
    {
        if (m_selected != null)
        {
                m_selected.GetComponent<HarvesterScript>()?.SetSelected(false);
                m_selected.GetComponent<DemonScript>()?.SetSelected(false);
                m_selected.GetComponent<TowerScript>()?.SetSelected(false);
                m_selected.GetComponent<BombTowerScript>()?.SetSelected(false);
                m_selected.GetComponent<IceTowerScript>()?.SetSelected(false);
                m_selected.GetComponent<LaserTowerScript>()?.SetSelected(false);
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
            case Spell.FrostRing:
                if (m_fFrostRingTimeLeft == 0.0f && fFrostFieldCost <= fManaPool)
                {
                    Instantiate(m_FrostRing, _targetPos + Vector3.up * 20.0f, Quaternion.identity);
                    m_fFrostRingTimeLeft = m_fFrostRingCoolDown;
                    fManaPool -= fFrostFieldCost;
                }
                break;
        }
    }
    public float GetMana()
    {
        return fManaPool;
    }
}
