using UnityEngine;
using System.Collections;

public class WaveMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private Vector3 startingPos;
    public float speed;

    private Vector3 savedVelocity;
    private bool isMoving = true;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
	
	}

    void Update()
    {
        CheckPosition();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (isMoving)
        {
            rb.velocity = new Vector3(transform.position.x,
                                 -speed * Time.deltaTime,
                                 transform.position.z);
        }

       
	
	}

    void CheckPosition()
    {
        Vector3 _pos = Camera.main.WorldToViewportPoint(transform.position);

        if(_pos.y < 0)
        {
            foreach(Transform _object in transform)
            {
               
                _object.transform.parent = null;
                _object.gameObject.SetActive(false);
            }

            StartCoroutine(TurnOffShape());
        }
    }

    IEnumerator TurnOffShape()
    {
        yield return new WaitForSeconds(0.1F);
        transform.position = startingPos;
        gameObject.SetActive(false);           
    }

    public void PauseGame()
    {
        savedVelocity = rb.velocity;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        isMoving = false;
    }

    public void UnPauseGame()
    {
        rb.isKinematic = false;
        rb.velocity = savedVelocity;
        isMoving = true;


    }
}
