using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100.0f;
    [SerializeField] float mainThrust = 100.0f;
    Rigidbody rigidbody;
    AudioSource locAudio;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        locAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate(); 
    }
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("friendly");
                break;
            case "Finish":
                print("hit finish");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
                break;
            
            default:
                print("die");
                SceneManager.LoadScene(0);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

                break;
        }
    }
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float ThrustForceThisFrame = Time.deltaTime * mainThrust;
            rigidbody.AddRelativeForce(Vector3.up * ThrustForceThisFrame);


        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            locAudio.Play();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            locAudio.Stop();
        }
    }

    private void Rotate()
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
