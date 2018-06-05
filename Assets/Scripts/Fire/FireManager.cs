using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : Singleton<FireManager> {

    private List<GameObject> fires;
    private GameObject fireContainer;
    private const float DISTANCE_THRESHOLD = 0.2f;

	// Use this for initialization
	void Start () {
        fires = new List<GameObject>();
        fireContainer = new GameObject();
        fireContainer.name = "Fire Container";
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //Checks if a fire exists already at this position
    public bool FireExists(Vector3 location)
    {
        if (fires.Count == 0) return false;

        foreach(GameObject fire in fires)
        {
            if(Vector3.Distance(location, fire.transform.position) < DISTANCE_THRESHOLD)
            {
                Debug.Log("Equal!");
                return true;
            }
        }
        return false;
    }

    public void AddFire(GameObject fire)
    {
        fires.Add(fire);
        fire.transform.parent = fireContainer.transform;
    }

    public void RemoveFire(GameObject fire)
    {
        fires.Remove(fire);
    }
}
