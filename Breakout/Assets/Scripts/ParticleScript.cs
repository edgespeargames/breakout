using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    ParticleSystem particles;

    private void Start()
    {
        particles = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    public void SetColor(Color newColor)
    {
        particles = gameObject.GetComponentInChildren<ParticleSystem>();
        var main = particles.main;
        main.startColor = newColor;
    }

    void LateUpdate()
    {
        if (!particles.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
