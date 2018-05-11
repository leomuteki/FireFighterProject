using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetComponent<Camera>().cullingMask = ~(1 << LayerMask.NameToLayer("ThermalLayer"));
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            GetComponent<Camera>().cullingMask = (1 << LayerMask.NameToLayer("ThermalLayer")) | (1 << LayerMask.NameToLayer("FireLayer")) | (1 << LayerMask.NameToLayer("FLIRLayer"));
        }
		
	}
}
