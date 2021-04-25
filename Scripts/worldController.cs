using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    Vector3 position;
    public bool isStarted;
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    	if(isStarted)
       	 	transform.Translate(Vector3.left * Time.deltaTime * speed);
    }

    public void StartGame(){
    	isStarted = true;
    }

    public void EndGame(){
    	isStarted = false;
    	transform.position = position;
    }
}
