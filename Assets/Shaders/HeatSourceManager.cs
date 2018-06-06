using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSourceManager : Singleton<HeatSourceManager>
{
    public HeatSource deleteMe;
    public List<HeatSource> HeatSources = new List<HeatSource>();
    [SerializeField]
    private List<Material> mats = new List<Material>();
    private List<Vector4> vectors = new List<Vector4>();
    [SerializeField, Range(0, 1)]
    private float TemperatureCap = 1;
    [SerializeField, Range(0, 2)]
    private float Brightness = 1;
    public List<Color> heatColors = new List<Color>();

    public void Start()
    {
        InitializeShader();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DestroyHeatSource(deleteMe);
        }
    }

    public void InitializeShader()
    {
        foreach (HeatSource hs in HeatSources)
        {
            vectors.Add(new Vector4(hs.transform.position.x, hs.transform.position.y, hs.transform.position.z, hs.temperature));
        }
        foreach (Material mat in mats)
        {
            mat.SetFloat("_HeatSources_Length", HeatSources.Count);
            mat.SetFloat("_TempCap", TemperatureCap);
            mat.SetFloat("_Brightness", Brightness);
            mat.SetVectorArray("_HeatSources", vectors);
        }
    }

    private void UpdateShaderTemperatures()
    {
        vectors.Clear();
        foreach (HeatSource hs in HeatSources)
        {
            vectors.Add(new Vector4(hs.transform.position.x, hs.transform.position.y, hs.transform.position.z, hs.temperature));
        }
        foreach (Material mat in mats)
        {
            mat.SetVectorArray("_HeatSources", vectors);
        }
    }

    public void DestroyHeatSource(HeatSource hs)
    {
        StartCoroutine(FadeHeatSource(hs));
    }

    private IEnumerator FadeHeatSource(HeatSource hs)
    {
        float startTime = Time.realtimeSinceStartup;
        float timePassed = 0;
        float startTemp = hs.temperature;
        while (timePassed < hs.CoolDownDuration)
        {
            hs.temperature = startTemp * (hs.CoolDownDuration - timePassed) / hs.CoolDownDuration;
            UpdateShaderTemperatures();
            yield return null;
            timePassed = Time.realtimeSinceStartup - startTime;
        }
        HeatSources.Remove(hs);
        Destroy(hs);
        UpdateShaderTemperatures();
    }

}