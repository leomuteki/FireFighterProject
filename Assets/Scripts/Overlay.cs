using UnityEngine;

public class Overlay : MonoBehaviour {

    public GameObject FLIRCamera;

	// Use this for initialization
	void Start () {
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
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            toggleOutlines();
        }
		
	}

    private void turnOnOverlay(bool is_on)
    {
        if (is_on)
        {
            GetComponent<Camera>().cullingMask = (1 << LayerMask.NameToLayer("ThermalLayer") | 1 << LayerMask.NameToLayer("SmokeLayer"));// | (1 << LayerMask.NameToLayer("FireLayer")) | (1 << LayerMask.NameToLayer("FLIRLayer"));
        }
        else
        {
            GetComponent<Camera>().cullingMask = ~(1 << LayerMask.NameToLayer("ThermalLayer"));
        }
    }

    private void toggleOutlines()
    {
        GetComponent<cakeslice.OutlineEffect>().enabled = GetComponent<cakeslice.OutlineEffect>().enabled ? false : true;
        FLIRCamera.GetComponent<cakeslice.OutlineEffect>().enabled = GetComponent<cakeslice.OutlineEffect>().enabled ? false : true;
    }
}
