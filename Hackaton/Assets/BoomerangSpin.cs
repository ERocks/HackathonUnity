using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangSpin : MonoBehaviour {

    public float m_RotSpeed = 1f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, m_RotSpeed * Time.deltaTime));
	}
    
}
