using UnityEngine;

public class HeatSource : MonoBehaviour
{
    [Range(0, 1)]
    public float temperature = 0.5f;
    public float CoolDownDuration = 2.0f;

    private void Awake()
    {
        HeatSourceManager.Instance.HeatSources.Add(this.GetComponent<HeatSource>());
    }

}
