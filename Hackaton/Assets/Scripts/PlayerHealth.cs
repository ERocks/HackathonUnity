using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable {

    public int m_MaxHealth = 4;
    public int m_CurrentHealth;

    bool canTakeDamage;
    float timer;

    public GameObject deathparticle;
    GameObject deathScreen;

    PlayerMovement myMovement;
    CharacterController2D myController;
    bool countDeath;
    float deathTimer;

    // Use this for initialization
    void Start () {
        m_CurrentHealth = m_MaxHealth;
        canTakeDamage = false;
        timer = 0;
        deathTimer = 0;
        myMovement = this.gameObject.GetComponent<PlayerMovement>();
        myController = this.gameObject.GetComponent<CharacterController2D>();
        deathScreen = GameObject.Find("DefeatState");
    }
	
	// Update is called once per frame
	void Update () {
		if (!canTakeDamage)
        {
            timer += Time.deltaTime;

            if (timer >= 1)
                canTakeDamage = true;
        }

        if (m_CurrentHealth <= 0 && m_CurrentHealth > -1000)
        {
            Die();
            m_CurrentHealth = -1000;
        }

        if (countDeath)
        {
            deathTimer += Time.deltaTime;
            if (deathTimer >= 1)
            {
                GameManagement manager = GameObject.Find("GameManager").GetComponent<GameManagement>();
                manager.SetVisiblePos(deathScreen);
                Time.timeScale = 0;
                countDeath = false;
            }
        }
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Spikes")
            DoDamage(1);
    }

    public void DoDamage(int amount)
    {
        if (canTakeDamage)
        {
            m_CurrentHealth -= amount;
            timer = 0;
            canTakeDamage = false;
        }
    }
    void Die()
    {
        myMovement.enabled = !myMovement.enabled;
        myController.enabled = !myController.enabled;

        Instantiate(deathparticle, this.transform.position, Quaternion.identity);
        countDeath = true;
    }

}
