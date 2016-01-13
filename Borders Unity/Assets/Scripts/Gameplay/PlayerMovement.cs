using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private LevelManager lmScript;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    [Header("Movement Points")]
    public Vector3[] movementPoints = new Vector3[3];
    public int currentMovementPoint = 1;

    private bool isMoving;

    private Vector3 targetPosition;
    private Vector3 directionalVector;
    private Vector3 desiredVelocity;
    private float lastSqrMag;

    private Color currentColor;

    private Vector3 savedVelocity;
    private bool isPaused;


	// Use this for initialization
	void Start () {

        InitialValues();
	
	}

    void InitialValues()
    {
        GameObject levelMan = GameObject.Find("LevelManager");
        lmScript = levelMan.GetComponent<LevelManager>();

        rb = GetComponent<Rigidbody2D>();
        transform.position = movementPoints[currentMovementPoint];

        sprite = GetComponent<SpriteRenderer>();
        currentColor = sprite.color;
    }
	
	// Update is called once per frame
	void Update () {

        MovePlayer();

	
	}

    void FixedUpdate()
    {
        rb.velocity = desiredVelocity;
    }

    

    public void DetermineDirection(string _movingDirection)
    {
        if (!isPaused)
        {
            if (!isMoving)
            {
                switch (_movingDirection)
                {
                    case ("Right"):
                        if (currentMovementPoint < movementPoints.Length - 1)
                        {
                            currentMovementPoint++;
                            SetMovementPoint();
                        }
                        break;
                    case ("Left"):
                        if (currentMovementPoint > 0)
                        {
                            currentMovementPoint--;
                            SetMovementPoint();
                        }
                        break;
                }
            }
        }
    }

    void SetMovementPoint()
    {
        targetPosition = movementPoints[currentMovementPoint];
        directionalVector = (targetPosition - transform.position).normalized * 10;
        lastSqrMag = Mathf.Infinity;
        desiredVelocity = directionalVector;

        isMoving = true;

    }

    void MovePlayer()
    {
        if (isMoving)
        {
            float sqrMag = (targetPosition - transform.position).sqrMagnitude;

            if(sqrMag > lastSqrMag)
            {
                desiredVelocity = Vector3.zero;
                transform.position = targetPosition;
                isMoving = false;
            }

            lastSqrMag = sqrMag;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        string _layerName = LayerMask.LayerToName(other.gameObject.layer);

        if(_layerName == "Shape")
        {
            ChangeShape(other.gameObject);
            Color _shapeColor = other.gameObject.GetComponent<SpriteRenderer>().color;
        }
        else
        {
            if(other.tag != "Health" && other.tag != "Point")
            {
                CheckAgainstBorder(other.gameObject);
            }
            
        }

        if(other.tag == "Health")
        {
            lmScript.AddLives();
            bool _destroyingPickUp = false;
            TurnOffPickUp(other.gameObject, _destroyingPickUp);
        }

        if (other.tag == "Point")
        {
            Debug.Log("Hit");
            lmScript.AddPoints();
            bool _destroyingPickUp = false;
            TurnOffPickUp(other.gameObject, _destroyingPickUp);
        }
    }

    void CheckAgainstBorder(GameObject borderHit)
    {
        Color _shapeColor = borderHit.GetComponent<SpriteRenderer>().color;
       
        if ((borderHit.layer != gameObject.layer) && (_shapeColor != currentColor))
        {           
            foreach(Transform child in borderHit.transform)
            {
                if (child.GetComponent<CircleCollider2D>().enabled)
                {
                    bool _destroyingPickUp = true;
                    TurnOffPickUp(child.gameObject, _destroyingPickUp);
                }
            }
           
            lmScript.LoseLives();
        }
    }

    void TurnOffPickUp(GameObject _pickUp, bool _destroyingPickUp)
    {
        _pickUp.GetComponent<CircleCollider2D>().enabled = false;
        GameObject _sprite = _pickUp.transform.GetChild(0).gameObject;

        _sprite.GetComponent<SpriteRenderer>().enabled = false;
        _sprite.GetComponent<Animation>().enabled = false;
        _sprite.GetComponent<Animation>().Play();

        if (!_destroyingPickUp)
        {
            StartCoroutine(TurnOnParticleEffects(_sprite.GetComponent<ParticleSystem>()));
        }
       
    }

    IEnumerator TurnOnParticleEffects(ParticleSystem _pickUpParticles)
    {
        _pickUpParticles.enableEmission = true;
        _pickUpParticles.Play();
        yield return new WaitForSeconds(1F);
        _pickUpParticles.enableEmission = false;
        _pickUpParticles.Stop();
    }

    void ChangeShape(GameObject newShape)
    {
        SpriteRenderer _newSprite = newShape.GetComponent<SpriteRenderer>();
        sprite.sprite = _newSprite.sprite;
        sprite.color = _newSprite.color;
        currentColor = sprite.color;

        string _newLayer = newShape.GetComponent<Collider2D>().tag;

        gameObject.layer = LayerMask.NameToLayer(_newLayer);
        _newSprite.enabled = false;
    }

    public void PauseGame()
    {
        savedVelocity = rb.velocity;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        isPaused = true;
    }

    public void UnPauseGame()
    {
        rb.isKinematic = false;
        rb.velocity = savedVelocity;
        isPaused = false;
        
    }
}
