using UnityEngine;
using System.Collections;

public class WaveMovement : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        rb.velocity = new Vector3(transform.position.x,
                                  -speed * Time.deltaTime,
                                  transform.position.z);
	
	}
}
