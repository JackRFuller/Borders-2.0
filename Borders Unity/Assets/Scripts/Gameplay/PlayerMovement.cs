using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

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


	// Use this for initialization
	void Start () {

        InitialValues();
	
	}

    void InitialValues()
    {
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
        if (!isMoving)
        {
            switch (_movingDirection)
            {
                case ("Right"):
                    if(currentMovementPoint < movementPoints.Length -1)
                    {
                        currentMovementPoint++;
                        SetMovementPoint();
                    }
                    break;
                case ("Left"):
                    if(currentMovementPoint > 0)
                    {
                        currentMovementPoint--;
                        SetMovementPoint();
                    }
                    break;
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
        }
        else
        {
            CheckAgainstBorder(other.gameObject);
        }

        

        Color _shapeColor = other.gameObject.GetComponent<SpriteRenderer>().color;



        
    }

    void CheckAgainstBorder(GameObject borderHit)
    {
        Color _shapeColor = borderHit.GetComponent<SpriteRenderer>().color;

        if ((borderHit.gameObject.layer == gameObject.layer) || (_shapeColor == currentColor))
        {
            Debug.Log("Alive");
        }
        else
        {
            Debug.Log("Dead");
        }
    }

    void ChangeShape(GameObject newShape)
    {
        SpriteRenderer _newSprite = newShape.GetComponent<SpriteRenderer>();
        sprite.sprite = _newSprite.sprite;
        sprite.color = _newSprite.color;
        currentColor = sprite.color;

        string _newLayer = newShape.GetComponent<Collider2D>().tag;

        gameObject.layer = LayerMask.NameToLayer(_newLayer);        
        newShape.SetActive(false);
    }
}
