using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{
    //SerializeField make variable visible in the inspector
    [SerializeField] float rcsThrust = 100.0f;
    [SerializeField] float mainThrust = 100.0f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deadSound;
    [SerializeField] AudioClip succesSound;
    Rigidbody rigidbody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending};
    [SerializeField] State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive) { 
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) {return;}
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("friendly");
                break;
            case "Finish":
                StartSuccesSequence();
                break;
            default:
                StartDeadSeuence();
                break;
        }
    }

    private void StartSuccesSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(succesSound);
        Invoke("LoadNextLevel", 1f);
        //state = State.Alive;
    }

    private void StartDeadSeuence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deadSound);
        Invoke("LoadFirstLevel", 1f);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private  void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();

        }
        else
        {
            audioSource.Stop();
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

    }

    private void RespondToRotateInput()
    {
        rigidbody.freezeRotation = true;//take manual control of rotation
        float RotationThisFrame = Time.deltaTime * rcsThrust;
         if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.forward * RotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * RotationThisFrame);
        }
        rigidbody.freezeRotation = false;//let physics take control again

    }

}
