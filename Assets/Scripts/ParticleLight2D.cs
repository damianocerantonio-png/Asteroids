using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ParticleLight2D : MonoBehaviour
{
    public ParticleSystem ps;
    Light2D light2D;
    float initialIntensity;



    void Start()
    {
        light2D = GetComponent<Light2D>();
        initialIntensity = GetComponent<Light>().intensity;

    }

   
    void Update()
    {
        if (ps.isPlaying)
        {
            GetComponent<Light>().intensity = initialIntensity * ((ps.main.duration - ps.totalTime) / ps.main.duration);


        }
    }
}
