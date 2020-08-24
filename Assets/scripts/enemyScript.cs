using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour {

    // Classes
    [System.Serializable]
    public class Stage {
        public State _stageState;

        public float _duration;
        public int _repititions;

        float _startTime;
        public bool _started = false;

        public Stage(State stageState, float duration, int repititions) {
            _stageState = stageState;
            _duration = duration;
            _repititions = repititions;
        }

        public void startStage() {
            _started = true;
            _startTime = Time.time;
        }

    }


    [System.Serializable]
    public class Pattern {
        public Stage[] _stages;
       
        public bool _loop;
        public Pattern _nextPattern;
        public int currentStage = 0;

        public Pattern(Stage[] stages, Pattern nextPattern) {
            _stages = stages;
            _nextPattern = nextPattern;
        }

        public Stage getStage() {
            return _stages[currentStage];
        }

        public bool nextStage() {
            if (currentStage + 1 < _stages.Length) {
                currentStage++;
                return true;
            }
            currentStage = 0;
            return false;
        }
    }



    public enum State { idle, attacking };

    public int _health;
    public float hitDelay;
    public shakeScript _shakeScript;
    public playerScript _playerScript;
    public float hitAnimTime;
    public GameObject hitFace;
    public State enemyState;
    public Animator anim;



    public Stage attackStage;
    public Stage idleStage;

    public Pattern startPattern;
    public Pattern currentPattern;


    float timeToHit;
    float timeToNextStage;
    float timeToChange;
    public bool hitting = false;

    bool playedWindUp;

    // Use this for initialization
    void Start () {
        _shakeScript = GetComponent<shakeScript>();
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<playerScript>();

        timeToChange = Time.time;
        hitFace = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        anim = transform.GetChild(0).GetComponent<Animator>();
      
        //Setting Patterns

        startPattern = new Pattern(new Stage[] { idleStage, attackStage, idleStage, attackStage, attackStage, idleStage, attackStage, idleStage, idleStage, attackStage, attackStage, attackStage, idleStage }, startPattern);
        startPattern._nextPattern = startPattern;
        currentPattern = startPattern;
        playedWindUp = false;
	}

    void managePattern() {
        Stage currentStage = currentPattern.getStage();

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !playedWindUp) {
            GameObject.FindGameObjectWithTag("GM").GetComponent<GameManagerScript>().playWindUp();
            playedWindUp = true;
        }

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && playedWindUp) {
            playedWindUp = false;
        }

        if (currentStage._started == false) {
            currentStage._started = true;
            timeToNextStage = Time.time + currentStage._duration;
        }

        if (Time.time > timeToNextStage) {
            currentStage._started = false;
            bool cont = currentPattern.nextStage();

            if (!cont) {
                currentPattern = currentPattern._nextPattern;
            }

        }
        enemyState = currentStage._stageState;
    }



	
	// Update is called once per frame
	void Update () {
        managePattern();
        //enemyState = State.idle;

        if (_health <= 0) {
            Die();
        }


        if (Time.time < timeToChange) {
            hitFace.SetActive(true);
        } else {
            hitFace.SetActive(false);
        }

        if (enemyState == State.attacking) {
            Attack();
        }



	}




    void Attack() {
        anim.ResetTrigger("Attack");
        anim.SetTrigger("Attack");
        hitting = true;
        timeToHit = Time.time + hitDelay;
        
    }

    public void hit(int damage) {
        _health -= damage;
        _shakeScript.Shake();
        timeToChange = Time.time + hitAnimTime;
    }

    void animateEnemy() {
        
    }

    void Die() {
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameManagerScript>().playDeath1();
        Destroy(gameObject);
    }
}
