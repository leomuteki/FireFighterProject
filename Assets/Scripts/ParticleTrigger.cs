using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour {
    public ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnParticleTrigger()
    {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        for(int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            Color color = p.startColor;
            p.startColor = new Color(255, 0, 0, 0);
            enter[i] = p;
            /*while (p.startColor.a > 0)
            {
                p.startColor = new Color(color.r, color.g, color.b, color.a - (Time.deltaTime));
                enter[i] = p;
                //ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
                Debug.Log(p.startColor.a);
            }*/
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
    }
}
