using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LightDetector : MonoBehaviour {

    Texture2D tex;
    public RenderTexture renderTexture;
    int height = 256;
    int width = 256;

    public Image legendImage;
    public Text maxTempText;
    public Text minTempText;

    public float scanTime = 1.0f;
    private float timer = 0.0f;

    private const float MAX_TEMPERATURE = 150f;
    private const float MIN_TEMPERATURE = 0f;
    private const float X_MIN = 0f;
    private const float X_MAX = 1f;
    private float GRAYSCALE_THRESHOLD = 0.1f;
    public int pixelSkips = 5;

	// Use this for initialization
	void Start () {
        tex = new Texture2D(width, height, TextureFormat.RGB24, false);

		
	}
	
	// Update is called once per frame
	void Update () {
        if(timer >= scanTime)
        {
            UpdateLegend();
            timer = 0.0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
        
		
	}

    public void UpdateLegend()
    {
        Color lowestColor = new Color();
        Color highestColor = new Color();

        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        float lowestLuminance = GetLuminance(tex.GetPixel(0, 0));
        float highestLuminance = GetLuminance(tex.GetPixel(0, 0));
        for (int i = 0; i < height; i += pixelSkips)
        {
            for (int j = 0; j < width; j += pixelSkips)
            {
                Color color = tex.GetPixel(j, i);
                if(!IsGrayScale(color))
                {
                    continue;
                }
                float luminance = GetLuminance(color);
                if (luminance < lowestLuminance)
                {
                    lowestColor = color;
                    lowestLuminance = luminance;
                }
                else if (luminance > highestLuminance)
                {
                    highestColor = color;
                    highestLuminance = luminance;
                }
            }
        }
        legendImage.material.SetColor("_Color", lowestColor);
        legendImage.material.SetColor("_Color2", highestColor);

        maxTempText.text = Math.Round(CalculateTemperature(highestLuminance),2).ToString() + 'F';
        minTempText.text = Math.Round(CalculateTemperature(lowestLuminance),2).ToString() + 'F';
    }

    public float GetLuminance(Color color)
    {
        return (color.r * 0.2126f) + (color.g * 0.7152f) + (color.b * 0.0722f);
    }

    public bool IsGrayScale(Color color)
    {
        float sum = color.r + color.b + color.g;
        float dist = (Mathf.Pow(sum, 2) / 3) - Mathf.Pow(color.r, 2) - Mathf.Pow(color.g, 2) - Mathf.Pow(color.b, 2);

        return dist > -GRAYSCALE_THRESHOLD;
    }

    //Calculates a fake temperature value based off interpolation
    public float CalculateTemperature(float lum)
    {
        return ( (lum - X_MIN) * (MAX_TEMPERATURE - MIN_TEMPERATURE) / (X_MAX - X_MIN) ) + MIN_TEMPERATURE;
    }

}
