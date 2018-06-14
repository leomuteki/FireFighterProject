using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    [SerializeField]
    private bool AlwaysThermal = false;
    [SerializeField]
    private Material squareLinesMaterial;
    private cakeslice.OutlineEffect CompleteOutlines;
    private Camera Cam;
    private bool overlayIsOn = false;
    private enum outlines { none=0, squares, NumberOfTypes };
    private outlines outlineType = outlines.none;
    [SerializeField, Range(0, 1)]
    private float xGap = 0.02f;
    [SerializeField, Range(0, 1)]
    private float yGap = 0.02f;
    [SerializeField]
    private float OutlineScanDelay = 0.1f;
    private Dictionary<string, OutlineSquare> currentSquares = new Dictionary<string, OutlineSquare>();
    private List<Vector3> renderSquares = new List<Vector3>();
    [SerializeField]
    private List<Light> allLights = new List<Light>();
    [SerializeField]
    private Transform MenuStartPosition;
    [SerializeField]
    private Transform Menu;
    private int outlineToggle = 0;

    private class OutlineSquare
    {
        public float xMin = Mathf.Infinity;
        public float xMax = Mathf.NegativeInfinity;
        public float yMin = Mathf.Infinity;
        public float yMax = Mathf.NegativeInfinity;
        public bool updated = false;
        public List<Vector3> points = new List<Vector3>();

        public void resetCorners()
        {
            xMin = Mathf.Infinity;
            xMax = Mathf.NegativeInfinity;
            yMin = Mathf.Infinity;
            yMax = Mathf.NegativeInfinity;
        }

        public void UpdateMyPoints()
        {
            Vector3 LL = new Vector3(xMin, yMin, 0);
            Vector3 LR = new Vector3(xMax, yMin, 0);
            Vector3 UL = new Vector3(xMin, yMax, 0);
            Vector3 UR = new Vector3(xMax, yMax, 0);
            
            points.Clear();
            points.Add(LL);
            points.Add(LR);
            points.Add(LL);
            points.Add(UL);
            points.Add(UL);
            points.Add(UR);
            points.Add(LR);
            points.Add(UR);
        }
    }
    
    // Use this for initialization
    void Start()
    {
        Cam = GetComponent<Camera>();
        if (!Cam)
        {
            Debug.LogError("Camera not attached to this game object.");
        }
        // TODO remove this test
        turnOnOverlay(false);
        outlineType = outlines.none;
        // END TEST
        if (AlwaysThermal)
        {
            turnOnOverlay(true);
            outlineType = outlines.squares;
        }
        StartCoroutine(ScanOutline());
    }

    public Transform sphere;
    // Update is called once per frame
    void Update()
    {
        /*
         * if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            Menu.gameObject.SetActive(true);
        }
        else
        {
            Menu.gameObject.SetActive(false);
        }*/
        if (!AlwaysThermal)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                turnOnOverlay(false);
                CompleteOutlines.enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                turnOnOverlay(true);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                if (allLights.Count > 0)
                {
                    bool on_status = allLights[0].enabled;
                    foreach (Light light in allLights)
                    {
                        light.enabled = !on_status;
                    }
                }
            }
            if (OVRInput.GetDown(OVRInput.Button.Three))
            {
                if (overlayIsOn)
                {
                    turnOnOverlay(false);
                    CompleteOutlines.enabled = false;
                }
                else
                {
                    turnOnOverlay(true);
                }
            }
            if (OVRInput.GetDown(OVRInput.Button.Four))
            {
                outlineToggle++;

                if (outlineToggle >= (int)outlines.NumberOfTypes)
                {
                    outlineToggle = 0;
                }
                outlineType = (outlines)outlineToggle;
            }
        }
        // Draw outlines
        if (overlayIsOn)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                outlineType = outlines.none;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                outlineType = outlines.squares;
            }
        }
    }

    public void OverlayOn(bool status)
    {
        turnOnOverlay(status);
    }

    private void turnOnOverlay(bool is_on)
    {
        overlayIsOn = is_on;
        if (is_on)
        {
            GetComponent<Camera>().cullingMask = (1 << LayerMask.NameToLayer("ThermalLayer") | 1 << LayerMask.NameToLayer("SmokeLayer") | 1 << LayerMask.NameToLayer("OutlineLayer"));// | (1 << LayerMask.NameToLayer("FireLayer")) | (1 << LayerMask.NameToLayer("FLIRLayer"));
        }
        else
        {
            GetComponent<Camera>().cullingMask = ~( (1 << LayerMask.NameToLayer("ThermalLayer") | (1 << LayerMask.NameToLayer("OutlineLayer"))));
        }
    }

    private IEnumerator ScanOutline()
    {
        while (true)
        {
            if (overlayIsOn && outlineType == outlines.squares)
            {
                UpdateSquares();
            }
            yield return new WaitForSeconds(OutlineScanDelay);
        }
    }
    
    private void UpdateSquares()
    {
        // reset updated flags
        foreach (KeyValuePair<string, OutlineSquare> squareEntry in currentSquares)
        {
            squareEntry.Value.updated = false;
            squareEntry.Value.resetCorners();
        }
        
        for (float x = 0; x <= 1; x += xGap)
        {
            for (float y = 0; y <= 1; y += yGap)
            {
                Ray ray = Cam.ViewportPointToRay(new Vector3(x, y, 0));
                RaycastHit hit;
                int mask = 1 << LayerMask.NameToLayer("OutlineLayer");
                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.layer == LayerMask.NameToLayer("OutlineLayer"))
                {
                    // Find parent that is in the outlineLayer
                    Transform rootName = hit.transform;
                    while (rootName.parent != null &&  rootName.parent.gameObject.layer == LayerMask.NameToLayer("OutlineLayer"))
                    {
                        rootName = rootName.parent;
                    }
                    
                    OutlineSquare currentSquare;
                    // if not in dictionary, add to dictionary
                    if (currentSquares.ContainsKey(rootName.gameObject.name))
                    {
                        currentSquare = currentSquares[rootName.gameObject.name];
                    }
                    else
                    {
                        currentSquare = new OutlineSquare();
                        currentSquares.Add(rootName.gameObject.name, currentSquare);
                    }

                    // set cursquare values
                    if (x <= currentSquare.xMin)
                    {
                        currentSquare.xMin = x;
                    }
                    if (x >= currentSquare.xMax)
                    {
                        currentSquare.xMax = x;
                    }
                    if (y <= currentSquare.yMin)
                    {
                        currentSquare.yMin = y;
                    }
                    if (y >= currentSquare.yMax)
                    {
                        currentSquare.yMax = y;
                    }

                    currentSquare.updated = true;
                }
            }
        }
        renderSquares.Clear();
        // if updated, add square to renderList, else remove it from renderSquares
        List<string> squareKeys = new List<string>(currentSquares.Keys);
        foreach (string squareKey in squareKeys)
        {
            if (currentSquares[squareKey].updated == true)
            {
                currentSquares[squareKey].UpdateMyPoints();
                foreach (Vector3 point in currentSquares[squareKey].points)
                {
                    renderSquares.Add(point);
                }
            }
            else
            {
                currentSquares.Remove(squareKey);
            }
        }
    }

    private void OnPostRender()
    {
        if (overlayIsOn && outlineType == outlines.squares && renderSquares.Count > 0)
        {
            if (!squareLinesMaterial)
            {
                Debug.LogError("Please Assign a material on the inspector");
                return;
            }
            GL.PushMatrix();
            squareLinesMaterial.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.LINES);
            foreach (Vector3 point in renderSquares)
            {
                GL.Vertex(point);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
}
