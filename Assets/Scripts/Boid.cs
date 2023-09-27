using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private Rigidbody rb;

    public float velocity = 10;
    public Transform target;
    public float separationDistance = 55.5f;

    public float separationIntensity = 5;
    public float cohesionIntensity = 5;
    public float alignmentIntensity = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (target is not null)
        {
            Vector3 targetRotationVector = CalculateBehaviour();
            Vector3 directionToTarget = target.position - transform.position;
            //Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);

            Quaternion targetRotation = Quaternion.LookRotation(targetRotationVector + directionToTarget, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
        }
        rb.velocity = rb.transform.forward * velocity;
    }

    private Vector3 CalculateBehaviour()
    {
        Vector3 separation = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        Vector3 cohesion = Vector3.zero;
        Boid[] flock = FindObjectsOfType<Boid>();

        foreach(Boid boid in flock)
        {
            if (boid != this)
            {
                float distanceFromBoid = Vector3.Distance(this.transform.position, boid.transform.position);

                if (distanceFromBoid < separationDistance)
                {
                    separation += (transform.position - boid.transform.position).normalized;
                }
                alignment += boid.transform.forward;

                cohesion += boid.transform.position - transform.position;
            }
        }

        return alignment.normalized * alignmentIntensity + cohesion.normalized * cohesionIntensity + separation.normalized * separationIntensity;
    }
}
