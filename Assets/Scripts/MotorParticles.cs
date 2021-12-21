using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorParticles : MonoBehaviour
{
    protected ParticleSystem ParticleSystem;
    void Start()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
    }

    public void MotorActive(float directionMultiplier, float strength)
    {
        ParticleSystem.Stop();
        var startSpeed = ParticleSystem.main.startSpeed;
        startSpeed = directionMultiplier * strength* 8f;

        var startSizeX = ParticleSystem.main.startSizeX;
        startSizeX = 0.3f;
        var startSizeY = ParticleSystem.main.startSizeY;
        startSizeY = 1f;
        var startSizeZ = ParticleSystem.main.startSizeZ;
        startSizeZ = 0.1f;

        var gravityMod = ParticleSystem.main.gravityModifier;
        gravityMod = 1f;

        ParticleSystem.Play();
    }

    public void MotorInactive()
    {
        ParticleSystem.Stop();


        ParticleSystem.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
