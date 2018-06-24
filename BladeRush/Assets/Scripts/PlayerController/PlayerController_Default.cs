﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Default : MonoBehaviour
{
    public PlayerCharacter controllingCharacter;
    GameObject swordProjectile;

    public float movespeed = 7.0f;
    public float gravscale = 10.0f;
    public Camera camera;
    public Rigidbody rigidbody;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15.0f;
    public float sensitivityY = 15.0f;
    float minimumX = -360.0f;
    float maximumX = 360.0f;
    float minimumY = -90.0f;
    float maximumY = 90.0f;
    float rotationX = 0.0f;
    float rotationY = 0.0f;
    Quaternion originalRotation;

    float refireTimer;
    float maxRefire = 0.25f;

    public float midairPause = 0.75f;
    float currAirPause;
    bool bAirPause;

    // Use this for initialization
    void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        originalRotation = transform.localRotation;

        Physics.gravity = new Vector3(0, -1.0f * gravscale, 0);

        controllingCharacter = (PlayerCharacter)GetComponent<Character>();
    }

    void ThrowSword()
    {
        controllingCharacter.ThrowSword(transform.position, camera.transform.rotation);
    }

    public void enableAirPause()
    {
        bAirPause = true;
        currAirPause = midairPause;
        Physics.gravity = new Vector3(0, 0, 0);
        rigidbody.velocity = new Vector3(0, 0, 0);
    }

    void disableAirPause()
    {
        currAirPause = 0.0f;
        bAirPause = false;
        Physics.gravity = new Vector3(0, -1.0f * gravscale, 0);
        rigidbody.velocity = new Vector3(0, 0, 0);
    }

    void updateAirMove()
    {
        if (!bAirPause)
            return;
        else if (currAirPause > 0)
        {
            rigidbody.velocity = new Vector3(0, 0, 0);
            currAirPause -= Time.deltaTime;
        }
        else if (currAirPause < 0)
        {
            disableAirPause();
        }
    }

    void Update()
    {
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
        MovePlayer();
        updateAirMove();
        if (refireTimer > 0) refireTimer -= Time.deltaTime;

        if (Input.GetAxis("Fire2") != 0 && refireTimer <= 0)
        {
            ThrowSword();
            refireTimer = maxRefire;
        }
    }

    void MovePlayer()
    {
        if (!Application.isFocused) return;

        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation = originalRotation * xQuaternion;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
        camera.transform.localRotation = originalRotation * yQuaternion;

        Vector3 targetForward = transform.localRotation * Vector3.forward;
        Vector3 targetUp = transform.localRotation * Vector3.up;

        if (bAirPause) return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 9.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 9.0f;

        Vector3 move = transform.forward * z;
        move += transform.right * x;

        move.Normalize();
        move *= movespeed * Time.deltaTime;

        rigidbody.MovePosition(transform.position + move);

        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
    }

    public static float ClampAngle (float angle, float min, float max)
    {
         if (angle < -360.0f)
             angle += 360.0f;
         if (angle > 360.0f)
             angle -= 360.0f;
         return Mathf.Clamp (angle, min, max);
    }
}
