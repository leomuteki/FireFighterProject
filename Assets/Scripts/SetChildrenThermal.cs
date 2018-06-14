using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChildrenThermal : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Invoke("StartRecursion", 1);
	}

    private void StartRecursion()
    {
        recursiveThermalLayer(transform);
    }

    private void recursiveThermalLayer(Transform cur)
    {
        if (!cur) return;
        cur.gameObject.layer = LayerMask.NameToLayer("SmokeLayer");
        int count = cur.childCount;
        for (int i = 0; i < count; ++i)
        {
            recursiveThermalLayer(cur.GetChild(i));
        }
    }
}
