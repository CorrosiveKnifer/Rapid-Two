using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public class BloodScript : MonoBehaviour
{
    public float bloodCount { get; set; }
    public GameObject bloodSpherePrefab;
    public bool IsConsumed = false;

    private GameObject myConsumer;

    private float delay;
    private float targetSize;
    private List<GameObject> myBloodProj;
    // Start is called before the first frame update
    void Start()
    {
        myBloodProj = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0)
            delay -= Time.deltaTime;

        if(IsConsumed && delay <= 0)
        {
            if(bloodCount > 0)
            {
                delay = 0.5f;
                GameObject blood = GameObject.Instantiate(bloodSpherePrefab, transform);
                StartCoroutine(SendBloodProjTo(myConsumer, 1.0f));
                targetSize = 1.0f * bloodCount / 5.0f;
            }

            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.0f, 1.0f, 1.0f) * targetSize, 0.15f);
            if (transform.localScale.y <= 0.05)
            {
                Destroy(gameObject, 0.5f);
            }
        }
    }
    private void OnDestroy()
    {
        if (myConsumer != null)
            myConsumer.GetComponent<MinionScript>().bloodCount += myBloodProj.ToArray().Length;

        foreach (var blood in myBloodProj)
        {
            Destroy(blood);
        }
    }
    public void Consume(GameObject consumer)
    {
        IsConsumed = true;
        myConsumer = consumer;
    }

    private IEnumerator SendBloodProjTo(GameObject Consumer, float bloodTransferAmount)
    {
        GameObject blood = GameObject.Instantiate(bloodSpherePrefab, transform.position, Quaternion.identity);
        myBloodProj.Add(blood);
        bloodCount -= bloodTransferAmount;

        Vector3 inital = blood.transform.position;

        float t = 0.0f;
        float dt = 0.05f;
        float dt2 = 0.01f;

        while(blood.transform.position != Consumer.transform.position)
        {
            blood.transform.position = Vector3.Lerp(inital, Consumer.transform.position, t);
            t += dt * Time.deltaTime;
            dt += dt2;

            yield return new WaitForEndOfFrame();
        }

        Consumer.GetComponent<MinionScript>().bloodCount += bloodTransferAmount;
        myBloodProj.Remove(blood);
        Destroy(blood);
        yield return null;
    }
}
