﻿using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class Rocket : MonoBehaviour
{
    //SerializeField make variable visible in the inspector
    [SerializeField] float rcsThrust = 100.0f;
    [SerializeField] float mainThrust = 100.0f;
    [SerializeField] float levelLoadDelay = 2f;
    
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deadSound;
    [SerializeField] AudioClip succesSound;

    [SerializeField] ParticleSystem mainParticles;
    [SerializeField] ParticleSystem deadParticles;
    [SerializeField] ParticleSystem succesParticles;

    new Rigidbody rigidbody = new Rigidbody();
    AudioSource audioSource;
    bool collisionsAreDisabled = false;
    bool isTransitioning = false;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning) {
            RespondToRotateInput();
            RespondToThrustInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKey();
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionsAreDisabled) {return;}
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccesSequence();
                succesParticles.Play();
                break;
            default:
                StartDeadSeuence();
                break;
        }
    }

    private void StartSuccesSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(succesSound);
        succesParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
        //state = State.Alive;
    }

    private void StartDeadSeuence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(deadSound);
        deadParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private  void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int maxIndex = SceneManager.sceneCountInBuildSettings - 1;
        if (currentIndex < maxIndex)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else
        {
            LoadFirstLevel();
        }
    }
    private void RespondToDebugKey() {
        if (Input.GetKeyDown(KeyCode.L)) {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsAreDisabled = !collisionsAreDisabled;
            if (collisionsAreDisabled)
            {
                print("collisions are OFF");
            }else
            {
                print("collisions ar ON");
            }
            
        }

        
    }
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();

        }
        else
        {
            StopApplyingTrust();
        }

    }

    private void StopApplyingTrust()
    {
        audioSource.Stop();
        if (mainParticles.isPlaying)
        {
            mainParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        float ThrustForceThisFrame = Time.deltaTime * mainThrust;
        rigidbody.AddRelativeForce(Vector3.up * ThrustForceThisFrame);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainParticles.Play();

    }

    private void RespondToRotateInput()
    {
        rigidbody.angularVelocity = Vector3.zero;//stop rotation due to physics
        float RotationThisFrame = Time.deltaTime * rcsThrust;
         if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.forward * RotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * RotationThisFrame);
        }

    }

}
