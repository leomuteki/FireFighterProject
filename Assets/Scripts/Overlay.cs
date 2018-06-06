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
    private List<Vector3> points = new List<Vector3>();
    private bool overlayIsOn = false;
    private enum outlines { none, squares, complete };
    private outlines outlineType = outlines.none;
    [SerializeField, Range(0, 1)]
    private float xGap = 0.02f;
    [SerializeField, Range(0, 1)]
    private float yGap = 0.02f;
    [SerializeField]
    private float OutlineScanDelay = 0.1f;

    // Use this for initialization
    void Start()
    {
        CompleteOutlines = GetComponent<cakeslice.OutlineEffect>();
        if (!CompleteOutlines)
        {
            Debug.LogError("Outline Effect script not attached to camera.");
        }
        Cam = GetComponent<Camera>();
        if (!Cam)
        {
            Debug.LogError("Camera not attached to this game object.");
        }
        // TODO remove this test
        turnOnOverlay(true);
        outlineType = outlines.squares;
        // END TEST
        if (AlwaysThermal)
        {
            turnOnOverlay(true);
            outlineType = outlines.squares;
        }
        StartCoroutine(ScanOutline());
    }

    // Update is called once per frame
    void Update()
    {
        if (!AlwaysThermal && Input.GetKeyDown(KeyCode.Alpha1))
        {
            turnOnOverlay(false);
            CompleteOutlines.enabled = false;
        }
        else if (!AlwaysThermal && Input.GetKeyDown(KeyCode.Alpha2))
        {
            turnOnOverlay(true);
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
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                outlineType = outlines.complete;
            }
            switch (outlineType)
            {
                case outlines.none:
                    CompleteOutlines.enabled = false;
                    break;
                case outlines.squares:
                    CompleteOutlines.enabled = false;
                    //UpdateSquares();
                    break;
                case outlines.complete:
                    CompleteOutlines.enabled = true;
                    break;
                default:
                    break;
            }
        }
    }

    private void turnOnOverlay(bool is_on)
    {
        overlayIsOn = is_on;
        if (is_on)
        {
            GetComponent<Camera>().cullingMask = (1 << LayerMask.NameToLayer("ThermalLayer") | 1 << LayerMask.NameToLayer("SmokeLayer"));// | (1 << LayerMask.NameToLayer("FireLayer")) | (1 << LayerMask.NameToLayer("FLIRLayer"));
        }
        else
        {
            GetComponent<Camera>().cullingMask = ~(1 << LayerMask.NameToLayer("ThermalLayer"));
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
        float Xmin = Mathf.Infinity;
        float Xmax = Mathf.NegativeInfinity;
        float Ymin = Mathf.Infinity;
        float Ymax = Mathf.NegativeInfinity;

        for (float x = 0; x <= 1; x += xGap)
        {
            for (float y = 0; y <= 1; y += yGap)
            {
                Ray ray = Cam.ViewportPointToRay(new Vector3(x, y, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "outline")
                {
                    if (x <= Xmin)
                    {
                        Xmin = x;
                    }
                    if (x >= Xmax)
                    {
                        Xmax = x;
                    }
                    if (y <= Ymin)
                    {
                        Ymin = y;
                    }
                    if (y >= Ymax)
                    {
                        Ymax = y;
                    }
                }
            }
        }

        Vector3 LL = new Vector3(Xmin, Ymin, 0);
        Vector3 LR = new Vector3(Xmax, Ymin, 0);
        Vector3 UL = new Vector3(Xmin, Ymax, 0);
        Vector3 UR = new Vector3(Xmax, Ymax, 0);

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

    private void OnPostRender()
    {
        if (overlayIsOn && outlineType == outlines.squares && points.Count > 0 && (points.Count % 2 == 0))
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
            for (int i = 0; i < (points.Count - 1); ++i)
            {
                GL.Vertex(points[i]);
                GL.Vertex(points[i + 1]);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
}
