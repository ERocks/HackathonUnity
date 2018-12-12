using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualHealth : MonoBehaviour {
    PlayerHealth playerHitpoints;
    public int threshhold;
	// Use this for initialization
	void Start () {
        playerHitpoints = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {
        if (playerHitpoints.m_CurrentHealth <= threshhold)
            Destroy(this.gameObject);
	}
}
