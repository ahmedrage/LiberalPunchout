using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHitBox : MonoBehaviour {
    public int damage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            col.gameObject.GetComponent<playerScript>().hit(damage);
        }
    }
}
