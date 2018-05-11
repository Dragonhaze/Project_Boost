using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidbody;
    AudioSource audioSource;


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
	}
	
	// Update is called once per frame
	void Update () {
        if(state == State.Alive)
        {
            Rotate();
            Thrust();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":

                audioSource.PlayOneShot(win);
                state = State.Transcending;
                Invoke("LoadNextScene",timeTochangeLvl);
                winBoom.Play();

                break;
            default:
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                state = State.Dying;
                Invoke("reloadScene", timeTochangeLvl);
                deathBoom.Play();
                break;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
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

        rigidbody.freezeRotation = true;

        if (Input.GetKey(KeyCode.Space))
        {
            
            rigidbody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
            engineExhaust.Play();
        }
        else
        {
            audioSource.Stop();
            engineExhaust.Stop();
        }
        rigidbody.freezeRotation = false;
    } 
       
    
    
}
