using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chantScript : MonoBehaviour {
    public float speed;
    public int _damage;
    public GameObject _particles;
    public GameObject _text;
    bool destroyed = false;

    public GameObject chantObject;
	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!destroyed) {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Enemy") {
            _particles.SetActive(true);
            _text.SetActive(false);
            destroyed = true;
            GetComponent<BoxCollider2D>().enabled = false;
            col.gameObject.transform.parent.GetComponent<enemyScript>().hit(_damage);
            GameObject.FindGameObjectWithTag("GM").GetComponent<GameManagerScript>().playHurt2();
            Destroy(gameObject, 0.5f);
        }
    }
}
