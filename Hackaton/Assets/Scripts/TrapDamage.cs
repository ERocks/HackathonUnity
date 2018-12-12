using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour {

    public int m_TrapDamage = 100;
    private Collider2D m_TrapCollider;
    // Use this for initialization
    void Start () {
        m_TrapCollider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable dam = collision.gameObject.GetComponent<IDamageable>();
        if (dam != null) {
            dam.DoDamage(m_TrapDamage);
        }
    }
}
