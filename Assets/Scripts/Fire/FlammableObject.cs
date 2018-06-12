using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Wood = 0,
    Leather = 1,
    Fabric = 2,
    Plastic = 3,
    Paper = 4
}

public class FlammableObject : MonoBehaviour {

    public ObjectType type;
    public float flameIndex { get; private set; }

	// Use this for initialization
	void Start () {
        SetFlameIndex();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetFlameIndex()
    {
        if(type == ObjectType.Wood)
        {
            flameIndex = 5;
        }
        else if(type == ObjectType.Leather)
        {
            flameIndex = 2;
        }
        else if(type == ObjectType.Paper)
        {
            flameIndex = 8;
        }
    }
}
