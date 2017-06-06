using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseCar : MonoBehaviour
{
    public float MotorStrength = 25000.0f;
    public float BreakStrength = 100000.0f;
    public float SteeringStrength = 50000.0f;
    public float TurnStrength = 200000.0f;
    public float GravityScale = 150;
    public float MaxSpeed = 350;
    public float SpeedScale = 4;
    public float TurnStrengthInAir = 100000;

    [HideInInspector]
    public float FixSpeed = 0;

    public ParticleSystem ThrusterPS;
    public Suspension[] Suspensions;
    public Wheel[] Wheels;

    public Transform CenterOfMass;
    public Rigidbody Body;
    public Collider Collider;

    private Text SteeringDebug;

    [HideInInspector]
    public float CurrentSpeed;

    [HideInInspector]
    public float AverageSpeed;

    private int AverageCount;
    private float AverageMax;

    void Start ()
    {
        //m_Body.centerOfMass = CenterOfMass.position - transform.position;
        SteeringDebug = GameObject.Find("CarDebugLabel").GetComponent<Text>();
    }

    public void Simulate()
    {
        //Flips the car
        if (Input.GetButtonDown("Reset"))
        {
            transform.Translate(transform.up * 5);
            transform.Rotate(new Vector3(0, 0, 180));
        }

        //Average speed
        AverageMax += CurrentSpeed;
        AverageCount++;
        AverageSpeed = AverageMax / AverageCount;

        //Wheel movement
        foreach (var wheel in Wheels)
            wheel.ApplyMotion(this);
    }

    public void FixedSimulate()
    {
        RaycastHit hit;

        //Axes
        float Sideways = Input.GetAxis("Sideways");
        float Forward = Input.GetAxis("Forward");
        float Upwards = Input.GetAxis("Upwards");

        CurrentSpeed = Body.velocity.magnitude;
        Vector3 angularVel = Body.angularVelocity;
        angularVel.Scale(transform.up);

        Body.angularDrag = 3.0f;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 4f))
        {
            Body.angularDrag = 1f;

            //Cancel out rotation
            float rotSpeed = angularVel.magnitude;

            //Rotation
            float shouldRotate = CurrentSpeed < 50 ? CurrentSpeed * 0.02f : 1;
            float speedResistance = 1 - (CurrentSpeed / MaxSpeed);
            speedResistance *= SpeedScale;

            Body.AddTorque(this.transform.up * Sideways * TurnStrength * shouldRotate);

            //Cancel out rotation
            //m_Body.AddTorque(-steeringPower * 0.2f * (int)(1-Mathf.Abs(Input.GetAxis("Sideways"))));
            //FIX BUG

            //Push the car sideways (so it does not drift away)
            Vector3 currentSpeedVector = Body.velocity;
            float rightSpeed = Vector3.Dot(currentSpeedVector, transform.right);
            Body.AddForce(transform.right * -rightSpeed * SteeringStrength);

            //Forward
            Vector3 projectedForward = Vector3.Project(hit.normal, transform.forward);
            float acceleration = (Forward > 0.1 ? MotorStrength * speedResistance : BreakStrength);

            if (FixSpeed > 0)
                Forward = FixSpeed;

            if (projectedForward.magnitude < 0.1)
                Body.AddForceAtPosition(transform.forward * Forward * acceleration, CenterOfMass.position);
            else
                Body.AddForceAtPosition(projectedForward * Forward * acceleration, CenterOfMass.position);

            //DEBUG --------
            SteeringDebug.text = "";
            SteeringDebug.text = SteeringDebug.text + "\nAverage speed " + AverageSpeed.ToString();
            SteeringDebug.text = SteeringDebug.text + "\nSpeed " + CurrentSpeed.ToString();
            SteeringDebug.text = SteeringDebug.text + "\nResistance " + speedResistance.ToString();
            //DEBUG --------

            //Gravity
            Body.AddForce(-transform.up * GravityScale * CurrentSpeed);
        }
        else
        {
            Body.AddTorque(this.transform.forward * -Sideways * TurnStrengthInAir);
            Body.AddTorque(this.transform.right * -Upwards * TurnStrengthInAir * 5);

            Body.AddForce(Physics.gravity * 5000);
        }

        foreach (var susp in Suspensions)
        {
            susp.FixedSimulate(this);
        }
    }
}
