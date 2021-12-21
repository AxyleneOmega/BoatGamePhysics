using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPhysics;

public class BoatControl : MonoBehaviour
{
    //visible Properties
    public Transform Motor;
    public float SteerPower = 200f;
    public float Power = 1f;
    public float MaxSpeed = 5f;
    public float MaxRotationSpeed = 2f;
    public float Drag = 0.1f;

    //used Components
    protected Rigidbody Rigidbody;
    protected Quaternion StartRotation;
    protected ParticleSystem ParticleSystem;
    protected Camera Camera;
    protected MotorParticles mp;

    //internal Properties
    protected Vector3 CamVel;

    public void Awake()
    {
        ParticleSystem = GetComponentInChildren<ParticleSystem>();
        Rigidbody = GetComponent<Rigidbody>();
        StartRotation = Motor.localRotation;
        Camera = Camera.main;
        mp = FindObjectOfType<MotorParticles>();
    }

    public void FixedUpdate()
    {
        //default direction
        var forceDirection = transform.forward;
        var steer = 0;

        //steer direction [-1,0,1]
        if (Input.GetKey(KeyCode.A))
            steer = 1;
        if (Input.GetKey(KeyCode.D))
            steer = -1;


        //Rotational Force
        Rigidbody.maxAngularVelocity = MaxRotationSpeed * Rigidbody.mass;
        Rigidbody.AddForceAtPosition(steer * transform.right * SteerPower / 100f, Motor.position);

        //compute vectors
        var forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        var targetVel = Vector3.zero;

        //forward/backward poewr
        if (Input.GetKey(KeyCode.W))
            WaterPhysics.ApplyForceToReachVelocity(Rigidbody, forward * MaxSpeed, Power);
        if (Input.GetKey(KeyCode.S))
            WaterPhysics.ApplyForceToReachVelocity(Rigidbody, forward * -MaxSpeed, Power);

        //Motor Animation // Particle system
        Motor.SetPositionAndRotation(Motor.position, transform.rotation * StartRotation * Quaternion.Euler(0, 30f * steer, 0));
        if (ParticleSystem != null)
        {
            if (Input.GetKey(KeyCode.W))
            {
                Motor.SetPositionAndRotation(Motor.position, transform.rotation * StartRotation * Quaternion.Euler(0, 30f * steer, 0));
                mp.MotorActive(1, 1);
            }
            else if ( Input.GetKey(KeyCode.S))
            {
                Motor.SetPositionAndRotation(Motor.position, transform.rotation * StartRotation * Quaternion.Euler(-20f, 180f + 30f * steer, 0));
                mp.MotorActive(-1, 0.5f);
            }
            else
            {
                mp.MotorInactive();
            }
        }

        //moving forward
        var movingForward = Vector3.Cross(transform.forward, Rigidbody.velocity).y < 0;

        //move in direction
        Rigidbody.velocity = Quaternion.AngleAxis(Vector3.SignedAngle(Rigidbody.velocity, (movingForward ? 1f : 0f) * transform.forward, Vector3.up) * Drag, Vector3.up) * Rigidbody.velocity;

        //camera position
        //Camera.transform.LookAt(transform.position + transform.forward * 6f + transform.up * 2f);
        //Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, transform.position + transform.forward * -8f + transform.up * 2f, ref CamVel, 0.05f);
    }
}
