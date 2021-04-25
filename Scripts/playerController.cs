using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float jumpForce;
    public GameObject go_canvas, go_minigame;
    bool isGround = true, isStarted;
    Vector3 position;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space") && isGround && isStarted){
        	rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        	isGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
    	if(other.gameObject.tag == "ground"){
    		isGround = true;
    	}
    	if(other.gameObject.tag == "triagle"){
    		FindObjectOfType<main>().LowerScore(15);
    		EndGame();
    	}
    	if(other.gameObject.tag == "finish"){
    		EndGame();
    	}	
    }

    public void StartGame(){
    	isStarted = true;
    	FindObjectOfType<worldController>().StartGame();
    }

    void EndGame(){
    	isStarted = false;
    	FindObjectOfType<worldController>().EndGame();
    	transform.position = position;
    	go_minigame.SetActive(false);
    	go_canvas.SetActive(true);
    }
}
