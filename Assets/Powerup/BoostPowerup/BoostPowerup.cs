using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPowerup : BasePowerup
{
    [Header("Boost")]
    public ParticleSystem PS;
    public float BoostSpeed = 30.0f;

    public void Start()
    {
        _Start();
    }

    public void OnTriggerEnter(Collider other)
    {
        _OnTriggerEnter(other);
    }

    public void Update()
    {
        _Update();
    }

    protected override void OnActivate()
    {
        print("BOOST GAINED!");
        PS.Stop();
        GetComponent<MeshRenderer>().enabled = false;
    }

    protected override void OnReset()
    {
        print("BOOST RESET!");
        PS.Play();
        GetComponent<MeshRenderer>().enabled = true;
    }

    protected override void OnEffectBegin()
    {
        print("BOOST STARTED!");
        Target.Car.FixSpeed = 5.0f;
    }

    protected override void OnEffectEnd()
    {
        print("BOOST ENDED!");
        Target.Car.FixSpeed = 0.0f;
    }
}