using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable {

    public int m_MaxHealth = 100;
    public int m_CurrentHealth;

    // Use this for initialization
    void Start () {
        m_CurrentHealth = m_MaxHealth;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoDamage(int amount)
    {
        m_CurrentHealth -= amount;
    }

}
