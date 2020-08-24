using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {
    public GameObject _playerObject;
    public GameObject _enemyObject;
    public playerScript _playerScript;
    public enemyScript _enemyScript;

    public AudioSource Music2;

    public AudioSource Hurt1;
    public AudioSource Hurt2;
    public AudioSource Death1;
    public AudioSource Death2;
    public AudioSource Windup;

    public AudioSource canMove;
    public AudioSource cantMove;


    // Use this for initialization
    void Start () {
        _playerObject = GameObject.FindGameObjectWithTag("Player");
        _enemyObject = GameObject.FindGameObjectWithTag("Enemy");

        _playerScript = _playerObject.GetComponent<playerScript>();
        _enemyScript = _enemyObject.GetComponent<enemyScript>();

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(0);
        }
    }

    public void playHurt1() {
        Hurt1.Play();
    }

    public void playHurt2() {
        Hurt2.Play();
    }

    public void playDeath1() {
        Death1.Play();
    }

    public void playDeath2() {
        Death2.Play();
    }

    public void playWindUp() {
        Windup.Play();
    }

    public void playCantMove() {
        canMove.Play();
    }

    public void playCanMove() {
        cantMove.Play();
    }
}
