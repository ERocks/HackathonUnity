using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStick : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy"))
        {
            other.transform.parent = transform;

        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.isTrigger && (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy"))
        {
            other.transform.parent = null;

        }
    }
}
