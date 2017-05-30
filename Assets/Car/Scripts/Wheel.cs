using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
	public void ApplyMotion(BaseCar Car)
    {
        transform.Rotate(Car.CurrentSpeed * Car.Body.transform.forward.magnitude, 0, 0);
    }
}