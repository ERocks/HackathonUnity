﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Controller : MonoBehaviour, IDamageable, IMovable
{

    public float jumpForceY = 400;
    public float jumpForceX = 400;
    public float clickForceY = 300;
    public float clickForceX = 300;
    //public float shootForce = 200;
    public float boomerangThreshold = 1f;
    public float jumpOverThreshold = 1.5f;
    public bool grounded;
    public int direction = 1;
    public int secondsToJump = 3;
    public int secondsToShoot = 0;
    public int m_EnemyDamage = 50;
    //public GameObject bulet;
    public LayerMask groundLayer;
    public Transform groundCheck;
    //public Transform shooter;
    //public Transform nodo;
    public enum EnemyType { jumping, stand, immovable
    };
    public EnemyType type;
    public Material[] slimeMaterials = new Material[3];
    Rigidbody2D body;
    Vector3 overCircle;
    float timeToJump;// timeToShoot;
    int secondsJump;// secondsShoot;

    // Use this for initialization
    void Start()
    {

    }

    private void Awake()
    {
        body = this.GetComponent<Rigidbody2D>();
        timeToJump = 0f;
        //this.transform.position = nodo.position;
        if (type == EnemyType.jumping)
        {
            this.GetComponent<Renderer>().material = slimeMaterials[0];
        } else if (type == EnemyType.stand)
        {
            jumpForceX = 0;
            this.GetComponent<Renderer>().material = slimeMaterials[1];
        }
        else if (type == EnemyType.immovable)
        {
            jumpForceY = 0;
            jumpForceX = 7;
            this.GetComponent<Renderer>().material = slimeMaterials[2];
        }
    }

    private void FixedUpdate()
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, .2f, groundLayer);
        grounded = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                break;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        timeToJump += Time.deltaTime;
        //timeToShoot += Time.deltaTime;
        secondsJump = (int)timeToJump % 60;
        //secondsShoot = (int)timeToShoot % 60;
        overCircle = this.transform.position;
        overCircle.x += (jumpForceX / 25) * direction;
        overCircle.y -= 0.5f;
        //Debug.Log("Overlap" + Physics2D.OverlapCircle(overCircle, 0.5f).tag);
        if (secondsJump >= secondsToJump && grounded && type == EnemyType.jumping && secondsToJump > 0)
        {
            if (Physics2D.OverlapCircleAll(overCircle, 0.5f, groundLayer).Length > 0)
            {
                
                jump();
                
            } else
            {
                direction *= -1;
            }
            timeToJump = 0;
        }
        overCircle = this.transform.position;
        overCircle.x += 1 * direction;
        overCircle.y -= 0.5f;
        if (type == EnemyType.immovable)
        {
            if (Physics2D.OverlapCircleAll(overCircle, 0.5f, groundLayer).Length > 0)
            {
                jump();
            }
            else
            {
                direction *= -1;
            }
        }
        /*
        if (secondsShoot >= secondsToShoot && secondsToShoot > 1)
        {
            shoot();
            timeToShoot = 0;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Weapon" && grounded && type == EnemyType.stand)
        {
            if ((other.transform.position.y - transform.position.y) < boomerangThreshold)
            {
                jump();

            };
        }

    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player" && grounded && type == EnemyType.stand)
        {
            if ((other.transform.position.y - transform.position.y) > jumpOverThreshold) {
                jump();

            };
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable dam = collision.gameObject.GetComponent<IDamageable>();
        if (dam != null)
        {
            dam.DoDamage(m_EnemyDamage);
        }
    }

    void jump()
    {
        float gravityMultiplier = Mathf.Sqrt( body.gravityScale);
        body.AddForce(transform.up * jumpForceY * gravityMultiplier);
        body.AddForce(transform.right * jumpForceX * direction * gravityMultiplier);
        grounded = false;
    }

    void OnDrawGizmos()
    {
        UnityEngine.Gizmos.color = Color.yellow;
        UnityEngine.Gizmos.DrawSphere(overCircle, 0.5f);
    }

    /*
    void shoot()
    {
        // Create the Bullet from the Bullet Prefab

        GameObject shoot = Instantiate(bulet, shooter.transform);

        // Add velocity to the bullet
        shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * shootForce;
        Destroy(shoot, 2.0f);
    }
    */
    public void DoDamage(int amount)
    {
        Debug.Log("Entro en Daño, " + amount);
        if (amount > 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void Move(Vector3 direction)
    {
        float gravityMultiplier = Mathf.Sqrt(body.gravityScale);
        if (!grounded)
        {
            body.AddForce(transform.up * direction.y * clickForceY * gravityMultiplier);
            body.AddForce(transform.right * direction.x * clickForceX * gravityMultiplier);
        }
    }
}
