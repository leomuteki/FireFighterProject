using UnityEngine;

public class HeatSource : MonoBehaviour
{
    [Range(0.5f, 1)]
    public float temperature = 0.5f;

    private void Awake()
    {
        HeatSourceManager.Instance.HeatSources.Add(this.GetComponent<HeatSource>());
    }

    private void OnBecameVisible()
    {
        HeatSourceManager.Instance.UpdateTemperature(temperature);
    }
}
