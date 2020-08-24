using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shakeScript : MonoBehaviour {
    public float magnitude;
    public float duration;
    public GameObject shakeObject;
    float startTime;
    float endTime;
    Vector3 originalPosition;

    bool reset = true;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        endTime = Time.time;
        if (shakeObject == null) {
            shakeObject = gameObject;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(Time.time < endTime && reset == false) {
            transform.localPosition = originalPosition + Random.insideUnitSphere * magnitude;
        } else if (reset == false) {
            transform.localPosition = originalPosition;
            reset = true;
        }		
	}

    public void Shake() {
        if (reset == true) {
            reset = false;
            startTime = Time.time;
            endTime = startTime + duration;
            originalPosition = transform.localPosition;
        }
    }

}
