using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour {
    public Vector3 target;
    private Transform transform;
	// Use this for initialization
	void Start () {
        transform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        LookAtCursor();
	}

    void LookAtCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // must have collider so the ray point to screen can reflect back
        if (Physics.Raycast(ray, out hit)) 
        {
            target = hit.point;
        }
        transform.LookAt(target);
    }
}
