using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttachTrigger : MonoBehaviour {

    public float intensity = 10f;
    public float range = 10f;
    private float originalIntensity;
    private float originalRange;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Light light = other.GetComponent<Light>();
            originalIntensity = light.intensity;
            originalRange = light.range;
            light.range = range;
            light.intensity = intensity;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Light light = other.GetComponent<Light>();
            light.intensity = originalIntensity;
            light.range = originalRange;
        }
    }
}
