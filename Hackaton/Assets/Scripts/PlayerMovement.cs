using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour {

    public CharacterController2D mCharacterController2D;
    

    private float mHorizontalMovement = 0;
    private bool mJump = false;
    private bool mAttack = false;
    private bool mCrouch = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        mHorizontalMovement = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            mJump = true;
        }

        if (Input.GetButtonDown("Attack"))
        {
            mCharacterController2D.Attack();
        }

        if (Input.GetButtonDown("Crouch")) {
            mCrouch = true;

        }else if (Input.GetButtonUp("Crouch")) {
            mCrouch = false;
        }

    }

    void FixedUpdate() {
        mCharacterController2D.Move(mHorizontalMovement * Time.fixedDeltaTime, mCrouch, mJump);
        mJump = false;
    }
}
