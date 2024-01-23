using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FluidSim : MonoBehaviour
{
    public GameObject fluidParticle;
    public Vector2 pos;
    public Vector2 vel;
    public Vector2 grav;
    public float particleRadius;
    public float collisionDamping;
    public Transform boundingBox;
    private void Awake()
    {
        fluidParticle = DrawCircle(new Vector3(0, 0, 0), particleRadius, Color.blue);
    }

    private void Update()
    {
       
        pos += vel * Time.deltaTime;
        vel += grav * Time.deltaTime;
        fluidParticle.transform.localPosition = pos;
        if (pos.x > boundingBox.localScale.x / 2 - particleRadius)
        {
            pos.x = boundingBox.localScale.x / 2 - particleRadius;
            vel.x *= -1 * collisionDamping;
        }

        if (pos.x < -boundingBox.localScale.x / 2 + particleRadius)
        {
            pos.x = -boundingBox.localScale.x / 2 + particleRadius;
            vel.x *= -1 * collisionDamping;
        }

        if (pos.y > boundingBox.localScale.y / 2 - particleRadius)
        {
            pos.y = boundingBox.localScale.y / 2 - particleRadius;
            vel.y *= -1 * collisionDamping;
        }

        if (pos.y < -boundingBox.localScale.y / 2 + particleRadius)
        {
            pos.y = -boundingBox.localScale.y / 2 + particleRadius;
            vel.y *= -1 * collisionDamping;
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
