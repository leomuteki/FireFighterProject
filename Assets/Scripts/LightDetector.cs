using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LightDetector : MonoBehaviour {

    Texture2D tex;
    public RenderTexture renderTexture;
    int height = 256;
    int width = 256;

	// Use this for initialization
	void Start () {
        tex = new Texture2D(width, height, TextureFormat.RGB24, false);

		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.P))
        {
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            byte[] bytes = tex.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/Images/Test.png", bytes);
            Color[,] colors = new Color[width, height];
            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    colors[i,j] = tex.GetPixel(j, i);
                    //Debug.Log(colors[i, j]);
                }
            }
            Debug.Log(colors[0,0]);
        }
		
	}
}
