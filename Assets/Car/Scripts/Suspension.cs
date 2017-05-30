using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    public float SpringConstant;
    public float DamperConstant;
    public float SuspensionLength;

    public Vector3 HitPosition;

    private float LastLength;
    private float CurrentLength;

	public void FixedSimulate(BaseCar Car)
    {
        RaycastHit hit;
        int layerMask = 1 << 9;

        if (Physics.Raycast(transform.position, -transform.up, out hit, SuspensionLength, layerMask))
        {
            HitPosition = hit.point;
            LastLength = CurrentLength;

            CurrentLength = (SuspensionLength - hit.distance) * (SuspensionLength - hit.distance);

            float springVelocity = (CurrentLength - LastLength) / Time.fixedDeltaTime;
            float springForce = SpringConstant * CurrentLength;
            float damperForce = DamperConstant * springVelocity;

            Car.Body.AddForceAtPosition(transform.up * (springForce + damperForce), transform.position);
        }
	}
}
