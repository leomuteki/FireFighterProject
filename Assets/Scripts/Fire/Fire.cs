using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fire : MonoBehaviour {

    public GameObject fire;
    public float raySpawnOffset = 1f;
    public float rayLength = 1f;
    public float offset = 0.5f;
    public float spreadTime = 5f;
    private float baseSpreadTime = 5f;
    private float timer;
    private bool fireSpread = false;

    private List<Collider> nearbyObjects;

	// Use this for initialization
	void Start () {
        nearbyObjects = new List<Collider>();
    }
	
	// Update is called once per frame
	void Update () {
        if (timer >= spreadTime && !fireSpread)
        {
            SpreadFire();
            fireSpread = true;
        }
        else
        {
            timer += Time.deltaTime;
        }
	}

    abstract public void SpreadFire();

    public void SetSpreadTime(float index)
    {
        spreadTime = baseSpreadTime - index;
    }

    public void SpawnFire(List<Vector3> downSpawnPoints, List<Vector3> upSpawnPoints, bool ignoreFlammability)
    {
        RaycastHit hit;
        foreach (Vector3 spawn in downSpawnPoints)
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(spawn, transform.TransformDirection(Vector3.down), out hit, rayLength))
            {
                Debug.DrawRay(spawn, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                Debug.Log("Hit!");
                if ((!FireManager.Instance.FireExists(hit.point) && hit.transform.tag == "Flammable") || ignoreFlammability)
                {
                    GameObject new_fire = Instantiate(fire, hit.point, hit.transform.rotation);
                    FlammableObject flammableObject = hit.transform.GetComponent<FlammableObject>();

                    //This accounts for objects that aren't flammable like floors
                    if(flammableObject != null)
                    {
                        new_fire.GetComponent<FireSpreadable>().SetSpreadTime(flammableObject.flameIndex);
                    }
                    FireManager.Instance.AddFire(new_fire);
                }
            }
            else
            {
                Debug.DrawRay(spawn, transform.TransformDirection(Vector3.down) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }
        }

        foreach (Vector3 spawn in upSpawnPoints)
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(spawn, transform.TransformDirection(Vector3.up), out hit, rayLength))
            {
                Debug.DrawRay(spawn, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
                Debug.Log("Hit!");
                Debug.Log(hit.transform.gameObject.name);
                Debug.Log(hit.point);
                if ((!FireManager.Instance.FireExists(hit.point) && hit.transform.tag == "Flammable") || ignoreFlammability)
                {
                    GameObject new_fire = Instantiate(fire, hit.point, hit.transform.rotation);
                    FlammableObject flammableObject = hit.transform.GetComponent<FlammableObject>();
                    if (flammableObject != null)
                    {
                        new_fire.GetComponent<FireSpreadable>().SetSpreadTime(flammableObject.flameIndex);
                    }
                    FireManager.Instance.AddFire(new_fire);
                }
            }
            else
            {
                Debug.DrawRay(spawn, transform.TransformDirection(Vector3.up) * 1000, Color.white);
                foreach(Collider collider in nearbyObjects)
                {
                    if(collider.bounds.Contains(spawn))
                    {
                        GameObject new_fire = Instantiate(fire, spawn, fire.transform.rotation);
                        FireManager.Instance.AddFire(new_fire);
                        FlammableObject flammableObject = collider.gameObject.GetComponent<FlammableObject>();
                        if (flammableObject != null)
                        {
                            new_fire.GetComponent<FireSpreadable>().SetSpreadTime(flammableObject.flameIndex);
                        }
                        Debug.Log("Spawning new fire on object!");
                    }
                }
                Debug.Log("Did not Hit");
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Flammable")
        {
            nearbyObjects.Add(other);
            Debug.Log("Added Object.");
        }
    }
}
