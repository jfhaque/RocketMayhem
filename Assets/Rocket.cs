using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audiosource;

    enum State {Alive, Dying, Transcending}

    State state = State.Alive;

    [SerializeField] float rcsThrust= 120f;
    [SerializeField] float thrust= 100f;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody= GetComponent<Rigidbody>();
        audiosource= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state== State.Alive){
            Thrust();
            Rotate();
        }
    }

    private void OnCollisionEnter(Collision collision){
        if (state != State.Alive){
            return;
        }
        switch(collision.gameObject.tag){
            case "Friendly":
            break;
            case "Finish":
            state = State.Transcending;
            Invoke("LoadNextScene",1);
            break;
            default:
            print("Dead");
            state= State.Dying;
            Invoke("LoadFirstScene",1);
            break;
        }
    }

    void LoadNextScene(){
        SceneManager.LoadScene(1);
    }

    void LoadFirstScene(){
        SceneManager.LoadScene(0);
    }


    private void Thrust(){
        if(Input.GetKey(KeyCode.Space)){
            rigidBody.AddRelativeForce(Vector3.up * thrust);
            if (!audiosource.isPlaying){
                audiosource.Play();
            }
        }
        else{
            audiosource.Stop();
        }
    }
    private void Rotate(){
        rigidBody.freezeRotation = true ;
        float rotationSpeed= rcsThrust * Time.deltaTime;
        if(Input.GetKey(KeyCode.A)){
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D)){   
            transform.Rotate(- Vector3.forward * rotationSpeed);
        }
        rigidBody.freezeRotation= false;
    }
}
