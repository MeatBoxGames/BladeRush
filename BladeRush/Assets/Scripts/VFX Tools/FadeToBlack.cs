using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlack : MonoBehaviour {
    public float Fade_Time = 1;

    private float fposition;
    private float fadestep;

    private UnityEngine.UI.Image Overlay_Image;
	// Use this for initialization
	void Start () {
        fadestep = 1 / Fade_Time;
        Overlay_Image = GetComponent<UnityEngine.UI.Image>();
        Debug.Assert(Overlay_Image != null);
	}
	
	// Update is called once per frame
	void Update () {
        fposition += fadestep * Time.deltaTime;
        Color old_color = Overlay_Image.color;
        old_color.a = Mathf.Lerp(0.0f, 1.0f, fposition);
        Overlay_Image.color = old_color;
    }
}
