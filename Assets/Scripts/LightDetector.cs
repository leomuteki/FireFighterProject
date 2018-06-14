﻿using System;
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
	void LateUpdate() {
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
        Color lowestColor = tex.GetPixel(width / 2, height / 2);
        Color highestColor = lowestColor;

        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        float lowestLuminance = GetLuminance(tex.GetPixel(width/2, height/2));
        float highestLuminance = lowestLuminance;
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

        maxTempText.text = Math.Round(CalculateTemperature(highestLuminance),2).ToString() + 'C';
        minTempText.text = Math.Round(CalculateTemperature(lowestLuminance),2).ToString() + 'C';
    }

    public float GetLuminance(Color color)
    {
        return (color.r * 0.2126f) + (color.g * 0.7152f) + (color.b * 0.0722f);
    }

    public bool IsGrayScale(Color color)
    {
        float ave = (color.r + color.b + color.g) / 3;
        if ((color.r - ave) > GRAYSCALE_THRESHOLD || (color.r - ave) < -GRAYSCALE_THRESHOLD)
        {
            return false;
        }
        if ((color.g - ave) > GRAYSCALE_THRESHOLD || (color.g - ave) < -GRAYSCALE_THRESHOLD)
        {
            return false;
        }
        if ((color.b - ave) > GRAYSCALE_THRESHOLD || (color.b - ave) < -GRAYSCALE_THRESHOLD)
        {
            return false;
        }
        return true;
    }

    //Calculates a fake temperature value based off interpolation
    public float CalculateTemperature(float lum)
    {
        return ( (lum - X_MIN) * (MAX_TEMPERATURE - MIN_TEMPERATURE) / (X_MAX - X_MIN) ) + MIN_TEMPERATURE;
    }

}
