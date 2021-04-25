using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float jumpForce;
    bool isGround = true;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space") && isGround){
        	rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        	isGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
    	
    	if(other.gameObject.tag == "ground"){
    		isGround = true;
    	}
    	if(other.gameObject.tag == "triagle"){
    		//FindObjectOfType<main>().LowerScore(15);
    		
    	}
    	if(other.gameObject.tag == "finish"){
    		
    	}	
    }
}
