using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public class ParticleData
{
    public Vector2 pos;
    public Vector2 predPos;
    public Vector2 vel;
    public Vector2 nextVel;
    public Vector2 force;
    public float density;
    public float pressure;
    public float mass;
    public float radius;
    public GameObject particle;
    public ParticleData(Vector2 pos, Vector2 vel, float mass, float radius, GameObject particle)
    {
        this.pos = pos;
        predPos = this.pos;
        this.vel = vel;
        this.nextVel = vel;
        this.mass = mass;
        this.radius = radius;
        this.particle = particle;
    }
}

public class FluidSim : MonoBehaviour
{
    public List<ParticleData> particles;
    public Vector2 grav;
    public float particleRadius;
    public float collisionDamping;
    public Transform boundingBox;
    public int particlesPerRow;
    public int particlesPerCol;
    public float particleSpacing;
    public float repelForce;
    public float drag;
    public float repelRange = 2;
    public AnimationCurve repelCurve;
    public GameObject circleObjectContainer;
    
    private void Awake()
    {
       
    }

    public void Gen()
    {
        if (circleObjectContainer != null)
        {
            DestroyImmediate(circleObjectContainer);
        }
        circleObjectContainer = new GameObject("CircleContainer");
        
        particles = new List<ParticleData>();
        
        for (int i = 0; i < particlesPerCol; i++)
        {
            for (int j = 0; j < particlesPerRow; j++)
            {
                var pos = new Vector2((float)(particlesPerRow-1)*(particleSpacing)*-0.5f,(particlesPerCol-1)*(particleSpacing)*-0.5f) + new Vector2(j * (particleSpacing), i * (particleSpacing));
                var vel = Vector2.zero;
                var particle = DrawCircle(pos, 1, Color.white);
                particle.transform.parent = circleObjectContainer.transform;
                particle.transform.localScale = Vector3.one * particleRadius * 2;
                particles.Add(new ParticleData(pos, vel, 1, particleRadius, particle));
            }
        }
        
    }
    
    private void Update()
    {
        foreach (var v in particles)
        {
            ResolveCollisions(v);
            UpdateParticle(v);
        }
    }

    private void UpdateParticle(ParticleData particleData)
    {
        Vector2 pressureGradient = GetGradientVector(particleData.pos);
        particleData.vel += grav * Time.deltaTime;
        particleData.vel += pressureGradient * repelForce * Time.deltaTime;
        particleData.vel -= particleData.vel * drag * Time.deltaTime;
        particleData.pos += particleData.vel * Time.deltaTime;
        particleData.predPos = particleData.pos + particleData.vel * Time.deltaTime;

        particleData.particle.transform.position = particleData.pos;
        particleData.particle.transform.localScale = Vector3.one * particleData.radius * 2;
        particleData.radius = particleRadius;
    }

    public Vector2 GetGradientVector(Vector2 samplePoint)
    {
        Vector2 currentVector = Vector2.zero;
        foreach (var v in particles)
        {
            float dst = (v.pos - samplePoint).magnitude;
            Vector2 dir = (v.pos - samplePoint).normalized;
            currentVector += dir * repelCurve.Evaluate(1 - dst / repelRange)*v.mass;
        }
        
        return currentVector.normalized *-1;
    }
    

    public Vector2 GetGradient(Vector2 samplePoint)
    {
        Vector2 propertyGradient = Vector2.zero;

        foreach (var v in particles)
        {
            float dst = (v.pos - samplePoint).magnitude;
            Vector2 dir = (v.pos - samplePoint).normalized;
            
        }


        return propertyGradient;
    }

    public Vector2 CalculateDensityGradient(Vector2 samplePoint)
    {
        const float stepSize = 0.001f;
        float deltaX = CalculateDensity(samplePoint + Vector2.right*stepSize) - CalculateDensity(samplePoint);
        float deltaY = CalculateDensity(samplePoint + Vector2.up*stepSize) - CalculateDensity(samplePoint);
        Vector2 densityGradient = new Vector2(deltaX, deltaY) / stepSize;
        return densityGradient;
    }
    public void ApplyRepelForce(Vector2 pos, float force)
    {
        foreach (var v in particles)
        {
            var dist = Vector2.Distance(v.pos, pos);
            if (dist < repelRange)
            {
                var dir = (v.pos - pos).normalized;
                var f = repelCurve.Evaluate(1 - dist / repelRange) * force;
                v.vel += dir * f*Time.deltaTime;
            }
        }
    }

    public float CalculateDensity(Vector2 samplePoint)
    {
        float density = 0;
        const float mass = 1;

        foreach (var v in particles)
        {
            var dist = Vector2.Distance(v.pos, samplePoint);
            if (dist < repelRange)
            {
                density += mass * repelCurve.Evaluate(1 - dist / repelRange);
            }
        }
        
        return density;
    }
    
    private void ResolveCollisions(ParticleData particleData)
    {
        if (particleData.pos.x > boundingBox.localScale.x / 2 - particleData.radius)
        {
            particleData.pos.x = boundingBox.localScale.x / 2 - particleData.radius;
            particleData.vel.x *= -1 * collisionDamping;
        }

        if (particleData.pos.x < -boundingBox.localScale.x / 2 + particleData.radius)
        {
            particleData.pos.x = -boundingBox.localScale.x / 2 + particleData.radius;
            particleData.vel.x *= -1 * collisionDamping;
        }

        if (particleData.pos.y > boundingBox.localScale.y / 2 - particleData.radius)
        {
            particleData.pos.y = boundingBox.localScale.y / 2 - particleData.radius;
            particleData.vel.y *= -1 * collisionDamping;
        }

        if (particleData.pos.y < (-boundingBox.localScale.y / 2) + particleData.radius)
        {
            particleData.pos.y = (-boundingBox.localScale.y / 2) + particleData.radius;
            particleData.vel.y *= -1 * collisionDamping;
        }
        
    }

    GameObject DrawCircle(Vector3 center, float radius, Color color)
    {
        GameObject circle = new GameObject();
        circle.transform.position = center;
        circle.AddComponent<SpriteRenderer>();
        circle.GetComponent<SpriteRenderer>().color = color;
        circle.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("circle");
        circle.transform.localScale = new Vector3(radius, radius, 1);
        return circle;
    }
}
