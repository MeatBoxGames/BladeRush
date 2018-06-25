using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToSceneOnClick : MonoBehaviour {
    public string First_Scene;
    public GameObject Transition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        GameObject transitionobject = Instantiate(Transition);
        FadeToBlack fadecontroller = transitionobject.GetComponent<FadeToBlack>();
        fadecontroller.Next_Scene = First_Scene;
    }
    
}
