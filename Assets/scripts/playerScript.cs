using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerScript : MonoBehaviour {
    //TODO:
    //ATTACKS
    //1.Basic Structure
    //2.Visual Feedback

    public int _health;
    public int _damage1;

    public Transform _left;
    public Transform _leftThreshold;
    public Transform _right;
    public Transform _rightThreshold;
    public Transform leftFire;
    public Transform rightFire;

    public GameObject chantObject;
    public GameObject _graphics;
    public GameObject _gm;

    public Animator _anim;

    public float _dodgeWait;
    float timeToReturn;

    public bool _dodge;
    public float dodgeLength;

    public float moveSpeed;
    public float coolDownLength;

    public Color origColor;
    public Color hurtColor;
    public float animTime;

    public enemyScript _enemyScript;
    public shakeScript _shakeScript;

    float timer;
    Vector3 startPosition;
    Vector3 currentStart;
    float startTime;
    float journeyLength;
    float time;
    float timeToDodge;
    float timeToFire;
    float timeToAnim;

    bool moving;
    bool left;
    bool right;
    bool back;

    bool canAttack = true;
	// Use this for initialization
	void Start () {
        moving = false;
        startPosition = transform.position;
        left = false;
        right = false;
        back = false;
        _health = 100;
        _dodge = false;
        _enemyScript = GameObject.FindGameObjectWithTag("Enemy").GetComponent<enemyScript>();
        _shakeScript = _graphics.GetComponent<shakeScript>();
        _gm = GameObject.FindGameObjectWithTag("GM");
        _anim = _graphics.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        Move();
        canAttack = !moving && (Time.time > timeToFire);

        if (Input.GetButtonDown("Fire1") && canAttack) {
            Attack(_damage1, true);
        }

        if(Input.GetButtonDown("Fire2") && canAttack) {
            Attack(_damage1, false);
        }

        if (Time.time > timeToDodge) {
            _dodge = false;
        }

        if (Time.time > timeToAnim) {
            _graphics.GetComponent<SpriteRenderer>().color = origColor;
        }

        if(_health <= 0) {
            Die();
        }

        _anim.SetBool("stunned", Time.time < timeToFire);

	}

    void Move() {

        if (Time.time < timeToFire) {

            if(Input.GetButtonDown("Horizontal")) {
                _gm.GetComponent<GameManagerScript>().playCantMove();
            }

            return;
        }

        if (transform.position == startPosition) {
            moving = false;
        } else {
            moving = true;
        }


        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0 && moving == false && right == false && left == false) {
            _gm.GetComponent<GameManagerScript>().playCanMove();
            startTime = Time.time;
            journeyLength = Vector2.Distance(startPosition, _right.position);
            right = true;
        } 

        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0 && moving == false && right == false && left == false) {
            _gm.GetComponent<GameManagerScript>().playCanMove();
            startTime = Time.time;
            journeyLength = Vector2.Distance(startPosition, _left.position);
            left = true;
        } 

        if(left) {
            MoveLeft();
        }

        if(right) {
            MoveRight();
        }

        if(back && Time.time > timeToReturn) {
            moveBack();
        }
    }

    void MoveLeft() {

        if(left == true) {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            float f = Time.deltaTime * moveSpeed;
            transform.position = Vector2.Lerp(startPosition, _left.position, fractionOfJourney);
            if (_left.position == transform.position) {
                left = false;
                timeToReturn = Time.time + _dodgeWait;
                startTime = timeToReturn;
                journeyLength = Vector2.Distance(startPosition, _left.position);
                currentStart = transform.position;
                back = true;
            }
        }

    }

    void MoveRight() {

        if (right == true) {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            float f = Time.deltaTime * moveSpeed;
            transform.position = Vector2.Lerp(startPosition, _right.position, fractionOfJourney);
            if (_right.position == transform.position) {
                right = false;
                timeToReturn = Time.time + _dodgeWait;
                startTime = timeToReturn;
                journeyLength = Vector2.Distance(startPosition, _right.position);
                currentStart = transform.position;
                back = true;
            }
        }

    }

    void moveBack() {
        if (back == true) {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            float f = Time.deltaTime * moveSpeed;
            transform.position = Vector2.Lerp(currentStart, startPosition, fractionOfJourney);
            if (startPosition == transform.position) {
                back = false;
            }
        }
    }

    bool canBeHit() {
        if (_dodge) {
            return false;
        }
        if ( (transform.position.x > _rightThreshold.position.x || transform.position.x < _leftThreshold.position.x)) {
            return false;
        }
        return false;
    }

    void Attack(int damage, bool left) {
        timeToFire = Time.time + coolDownLength;
        Transform spawn;
        if (left) {
            spawn = leftFire;
        } else {
            spawn = rightFire;
        }
        Instantiate(chantObject, spawn.position, spawn.rotation);
    }

    public void hit(int damage) {
        if (!_dodge) {
            _shakeScript.Shake();
            _health -= damage;
            _graphics.GetComponent<SpriteRenderer>().color = hurtColor;
            timeToAnim = Time.time + animTime;
            _gm.GetComponent<GameManagerScript>().playHurt1();
            _dodge = true;
            timeToDodge = Time.time + dodgeLength;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Safe" && _dodge == false) {
            _dodge = true;
            timeToDodge = Time.time + dodgeLength;
        }
    }

    void Die() {
        _gm.GetComponent<GameManagerScript>().playDeath2();
        Destroy(gameObject);
    }
}
