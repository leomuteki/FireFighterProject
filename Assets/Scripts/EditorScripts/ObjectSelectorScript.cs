using UnityEngine;
using System.Collections;

public class ObjectSelectorScript : MonoBehaviour
{
    public Material Material1;


    public void SelectObjects()
    {
        object[] obj = GameObject.FindSceneObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (g.GetComponent<MeshRenderer>())
            {
                g.GetComponent<MeshRenderer>().material = Material1;
            }
        }
    }
}