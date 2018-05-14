using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidbody;
    AudioSource audioSource;
    bool collisionDisabled = false;

    [SerializeField] GameObject maincanvas;


    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 2f;
    [SerializeField] float timeTochangeLvl = 1f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem engineExhaust;
    [SerializeField] ParticleSystem winBoom;
    [SerializeField] ParticleSystem deathBoom;
    
    enum State {Alive,Dying,Transcending};
    State state = State.Alive;


    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            state = State.Transcending;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(state == State.Alive)
        {
            
            Rotate();
            Thrust();
        }
        RespondToDebugKeys();
    }
    public void menuButtonClick()
    {
        maincanvas.SetActive(!maincanvas.activeSelf);
        state = State.Alive;
    }

    private void RespondToDebugKeys()
    {
        if (!Debug.isDebugBuild)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionDisabled)
        {
            return;
        }
        
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                Success();
                break;

            default:
                Fail();
                break;
        }
    }

    private void Fail()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        state = State.Dying;
        Invoke("reloadScene", timeTochangeLvl);
        deathBoom.Play();
    }

    private void Success()
    {
        audioSource.PlayOneShot(win);
        state = State.Transcending;
        Invoke("LoadNextScene", timeTochangeLvl);
        winBoom.Play();
    }

    private void LoadNextScene()
    {
        int currentlvl = SceneManager.GetActiveScene().buildIndex;
        if (currentlvl+1 == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(currentlvl + 1);
        }
        
        
    }

    private void reloadScene()
    {
        SceneManager.LoadScene(0);
    }



    private void Rotate()
    {
        float rotationspeed = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationspeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationspeed);
        }
    }

    private void Thrust()
    {

        rigidbody.angularVelocity = Vector3.zero; // Quita la rotación dada por las fisicas

        if (Input.GetKey(KeyCode.Space))
        {
            engineExhaust.Play();
            rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
            
        }
        else
        {
            audioSource.Stop();
            engineExhaust.Stop();
        }
    } 
       
    
    
}
