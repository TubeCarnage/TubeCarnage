using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public BaseCar Car;
    public Transform Target;
    public float Height = 1.0f;
    public float Distance = -25.0f;
    public float DistSmoothing = 1.0f;
    public float RotSmoothing = 2.0f;

    public void LateSimulate()
    {
        GetComponent<Camera>().fieldOfView = Car.CurrentSpeed * 0.05f + 70;

        Vector3 velocity = Vector3.zero;
        Vector3 forward = -Target.forward * Distance;

        Vector3 up = Target.up * Height;
        Vector3 needPos = Target.position - forward + up;
        transform.position = Vector3.SmoothDamp(transform.position, needPos,
                                                ref velocity, DistSmoothing * Time.deltaTime);
        //transform.LookAt(Target);
        transform.rotation = Quaternion.Slerp(transform.rotation, Target.rotation, RotSmoothing * Time.deltaTime);
    }
}