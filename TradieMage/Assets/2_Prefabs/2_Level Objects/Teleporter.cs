using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;
    public ParticleSystem idleParticles; 

    public void Awake()
    {
        idleParticles.Play();
    }


    public Transform GetDestination()
    {
        return destination;
    }

}
