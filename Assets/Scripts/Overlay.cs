using UnityEngine;

public class Overlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //turnOnOverlay(true);
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            turnOnOverlay(false);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            turnOnOverlay(true);
        }
		
	}

    private void turnOnOverlay(bool is_on)
    {
        if (is_on)
        {
            GetComponent<Camera>().cullingMask = (1 << LayerMask.NameToLayer("ThermalLayer") | 1 << LayerMask.NameToLayer("SmokeLayer"));// | (1 << LayerMask.NameToLayer("FireLayer")) | (1 << LayerMask.NameToLayer("FLIRLayer"));
            //GetComponent<cakeslice.OutlineEffect>().enabled = true;
        }
        else
        {
            GetComponent<Camera>().cullingMask = ~(1 << LayerMask.NameToLayer("ThermalLayer"));
            //GetComponent<cakeslice.OutlineEffect>().enabled = false;
        }
    }
}
