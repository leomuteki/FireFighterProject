using System.Collections.Generic;
using UnityEngine;

public class HeatSourceManager : Singleton<HeatSourceManager>
{
    public List<HeatSource> HeatSources = new List<HeatSource>();
    [SerializeField]
    private List<Material> mats = new List<Material>();
    private List<Vector4> vectors = new List<Vector4>();
    [SerializeField, Range(0,1)]
    private float TemperatureCap = 1;
    [SerializeField, Range(1, 50)]
    private float Brightness = 1;

    public void Start()
    {
        UpdateShader();
    }

    // TEST
    private void Update()
    {
    }

    public void UpdateShader()
    {
        foreach (Material mat in mats)
        {
            mat.SetFloat("_HeatSources_Length", HeatSources.Count);
            mat.SetFloat("_TempCap", TemperatureCap);   
            mat.SetFloat("_Brightness", Brightness);
            foreach (HeatSource hs in HeatSources)
            {
                vectors.Add(new Vector4(hs.transform.position.x, hs.transform.position.y, hs.transform.position.z, hs.temperature));
            }
            mat.SetVectorArray("_HeatSources", vectors);
        }
    }
}