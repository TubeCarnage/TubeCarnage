﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    public BaseCar Car;

    public void FixedUpdate()
    {
        if (isLocalPlayer)
            Car.FixedSimulate();
    }

    public void Update()
    {
        if (isLocalPlayer)
            Car.Simulate();
    }
}