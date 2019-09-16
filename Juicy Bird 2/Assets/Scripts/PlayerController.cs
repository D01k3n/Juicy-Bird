using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor.Animations;
using UnityEditor.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    bool started = false;
    float spawntimer = 2;
    public bool lost = false;

    public Animator uianimator;    
    GameObject gameovertext;
    GameObject pressbuttontext;

    Animator playeranimator; 

    public GameObject[] pipe_prefabs; 

    GameManager gm;
    public AudioManager am; 
    private void Start()
    {
        lost = false; 
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameovertext = GameObject.Find("GameOver_Text");        
        gameovertext.SetActive(false);
        pressbuttontext = GameObject.Find("PressButton_Text");
        playeranimator = GetComponentInChildren<Animator>();        
    }

    private void Update()
    {
        Movement();
        if (started && !lost) // Spawns pipes if not lost and has started
        {
            spawntimer -= Time.deltaTime; // when reaching 0, spawns pipe
            if(spawntimer <= 0)
            {
                int rand = Random.Range(0, 3); //randomizes which pipe to spawn
                GameObject npipe; 
                if(rand == 0)
                {
                    npipe = Instantiate(pipe_prefabs[0], new Vector3(5.5f, 3.45f, 0), Quaternion.identity); 
                    Rigidbody rb2 = npipe.GetComponent<Rigidbody>();
                    rb2.velocity -= Vector3.right * 100 * Time.deltaTime;
                    gm.pipes.Add(npipe);
                }
                if(rand == 1)
                {
                    npipe = Instantiate(pipe_prefabs[1], new Vector3(5.5f, .5f, 0), Quaternion.Euler(0, 0, 180));
                    Rigidbody rb2 = npipe.GetComponent<Rigidbody>();
                    rb2.velocity -= Vector3.right * 100 * Time.deltaTime;
                    gm.pipes.Add(npipe);
                }
                if (rand == 2)
                {
                    float r = Random.Range(1.25f, 3.1f);
                    npipe = Instantiate(pipe_prefabs[2], new Vector3(5.5f, r, 0), Quaternion.identity);
                    Rigidbody rb2 = npipe.GetComponent<Rigidbody>();
                    rb2.velocity -= Vector3.right * 100 * Time.deltaTime;
                    gm.pipes.Add(npipe);
                }
                spawntimer = 2; //resets spawntimer
            }
        }
    }

    private void Movement() //Starts game and controlls character 
    {
        if (Input.GetKeyDown(KeyCode.Space) && !lost && !started)
        {
            started = true;
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            uianimator.SetBool("PressButton", false);
            pressbuttontext.SetActive(false);
            gm.score = 0;
            gm.score_text.text = "Score: " + gm.score.ToString();
            playeranimator.SetTrigger("Flap");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 300);
            playeranimator.SetTrigger("Flap");
        }
    }

    void GameOver() //Reloads scene after dying
    {
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);       
        started = false;
        lost = false;
        am.ResumeMusic();
        gameovertext.SetActive(false);
        pressbuttontext.SetActive(true);
        uianimator.SetBool("PressButton", true);       
    }

    private void OnCollisionEnter(Collision collision) //Stops the game when touching pipe/roof/floor
    {
        if (collision.collider.tag == "DONTTOUCH" && !lost)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            lost = true;
            gameovertext.SetActive(true);
            uianimator.SetTrigger("GameOver");
            am.PauseMusic();
            am.PlayAudioEffect(0);
            gm.DestroyPipes();
            Invoke("GameOver", 3);
        }
    }
}
