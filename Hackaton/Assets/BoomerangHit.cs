using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangHit : MonoBehaviour {

    public float m_BoomerangForce = 1;
    // Use this for initialization
    void Start () {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.isTrigger) {
            
            IMovable mov = collider.gameObject.GetComponent<IMovable>();
            Slime_Controller sc = collider.gameObject.GetComponent<Slime_Controller>();
            if (mov != null && sc != null)
            {
                if (sc.type == Slime_Controller.EnemyType.jumping) {
                    Vector2 dir = collider.transform.position - transform.position;
                    dir = dir.normalized;
                    mov.Move(new Vector3(dir.x, dir.y, 0) * m_BoomerangForce);
                }
                
            }
        }
    }
}
