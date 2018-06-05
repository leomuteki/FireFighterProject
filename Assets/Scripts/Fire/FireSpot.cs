using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpot : MonoBehaviour {

    private Mesh mesh;
    public GameObject fire;
    public int spawnAmount = 5;
	// Use this for initialization
	void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaceFire()
    {
        StartCoroutine(SpawnFire(5));
    }

    IEnumerator SpawnFire(int time)
    {
        yield return new WaitForSeconds(time);
        Vector3[] verts = mesh.vertices;
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 randomVertex = transform.TransformPoint(verts[Random.Range(0, verts.Length)]);
            GameObject new_fire = Instantiate(fire, randomVertex, fire.transform.rotation);
        };
    }
}
