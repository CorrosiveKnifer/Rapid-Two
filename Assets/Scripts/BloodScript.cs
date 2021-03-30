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
    public float lifeTime = 5.0f;
    private float maxBlood;

    private bool IsDestroyed;
    private List<GameObject> myBloodProj;
    private float life = 0.0f;
    private bool FadingStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        myBloodProj = new List<GameObject>();
        maxBlood = bloodCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (life < lifeTime)
            life += Time.deltaTime;
        else if(!FadingStarted)
            StartCoroutine(StartFading());

        //Set the scale of the blood to be relective of how much blood there is.
        float targetSize = 1.0f * (bloodCount / maxBlood);
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.0f, 1.0f, 1.0f) * targetSize, 0.15f);

        if(!IsDestroyed && bloodCount <= 0)
        {
            IsDestroyed = true;
            Destroy(gameObject, 2.5f);
        }
    }

    private void OnDestroy()
    {
        //For each blood projectile, 
        foreach (var blood in myBloodProj)
        {
            //clense the game world of them
            Destroy(blood);
        }
    }

    //External access to the coroutine to start harvesting a single blob of blood.
    public void Consume(HarvesterScript harvester, float HarvestAmount)
    {
        if(bloodCount > 0)
            StartCoroutine(SendBloodProjToHarvester(harvester, HarvestAmount));
    }

    private IEnumerator StartFading()
    {
        FadingStarted = true;
        Material material = GetComponentInChildren<Renderer>().material;

        float t = 0.0f;
        float dt = 0.05f;
        float dt2 = 0.01f;
        float alpha = 1.0f;

        for (int i = 0; i < 5; i++)
        {
            t = 0.0f;
            dt = 0.05f;
            dt2 = 0.01f;
            alpha = 1.0f;

            do
            {
                alpha = Mathf.Lerp(1.0f, 0.0f, t);
                material.SetFloat("Alpha", alpha);

                t += dt * Time.deltaTime;
                dt += dt2;

                yield return new WaitForEndOfFrame();
            } while (alpha > 0.0f);

            do
            {
                alpha = Mathf.Lerp(1.0f, 0.0f, t);
                material.SetFloat("Alpha", alpha);

                t -= dt * Time.deltaTime;
                dt += dt2;

                yield return new WaitForEndOfFrame();
            } while (alpha < 1.0f);
        }

        do
        {
            alpha = Mathf.Lerp(1.0f, 0.0f, t);
            material.SetFloat("Alpha", alpha);

            t += dt * Time.deltaTime;
            dt += dt2;

            yield return new WaitForEndOfFrame();
        } while (alpha > 0.0f);

        Destroy(gameObject);

        yield return null;
    }

    private IEnumerator SendBloodProjToHarvester(HarvesterScript consumer, float bloodTransferAmount)
    {
        //Create the blob of blood
        GameObject blood = GameObject.Instantiate(bloodSpherePrefab, transform.position, Quaternion.identity);
        blood.transform.localScale = blood.transform.localScale * bloodTransferAmount;

        //Set up the inital variables
        float t = 0.0f;
        float dt = 0.05f;
        float dt2 = 0.01f;
        Vector3 inital = blood.transform.position;
        float bloodContents = Mathf.Min(bloodTransferAmount, bloodCount);

        //Reduce this blood pool of the amount the blob is carrying.
        bloodCount = Mathf.Clamp(bloodCount - bloodTransferAmount, 0, maxBlood);

        //Add to a list for clean up
        myBloodProj.Add(blood);

        //In sync with frames, lerp the position of the blob to the consumer
        while(blood.transform.position != consumer.transform.position)
        {
            blood.transform.position = Vector3.Lerp(inital, consumer.transform.position, t);
            t += dt * Time.deltaTime;
            dt += dt2;

            yield return new WaitForEndOfFrame();
        }

        //Blob has arrived andso we give them the blood contents.
        consumer.bloodHold += bloodContents;

        //No need to remember for clean up
        myBloodProj.Remove(blood);

        //Clean up ourselfs
        Destroy(blood);

        //Done
        yield return null;
    }
}
