﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlack : MonoBehaviour {
    public float Fade_Time = 1;
    public string Next_Scene;

    private float fposition;
    private float fadestep;

    private UnityEngine.UI.Image Overlay_Image;
	// Use this for initialization
	void Start () {
        fadestep = 1 / Fade_Time;
        Overlay_Image = GetComponentInChildren<UnityEngine.UI.Image>();
        Debug.Assert(Overlay_Image != null);
	}
	
	// Update is called once per frame
	void Update () {
        // Increment our position in the transition
        fposition += fadestep * Time.deltaTime;
        // Get the old color
        Color old_color = Overlay_Image.color;
        // Interpolate towards opaque
        old_color.a = Mathf.Lerp(0.0f, 1.0f, fposition);
        // Set the new color
        Overlay_Image.color = old_color;

        // If we're done
        if (fposition >= 1)
        {
            // Move to the next scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName: Next_Scene);
        }
    }
}
