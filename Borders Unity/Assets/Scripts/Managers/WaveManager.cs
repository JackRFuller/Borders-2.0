using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {

    private SpawnManager smScript;
    private LevelManager lmScript;

    [Header("Wave Attributes")]
    public int currentWave;
    public enum waveType
    {
        shape,
        border,
    }
    public waveType currentWaveType;
    public GameObject waveHolder;
    public int numberOfWavesToSpawn;
    private List<GameObject> pooledWaveHolders = new List<GameObject>();
    public Vector3[] spawnPositions = new Vector3[3];
    public float waveSpawnRate;

    [Header("Pick Up Attributes")]
    public int healthPickUpRate;
    public int starPickUpRate;

    [Header("Item Attributes")]
    public Color[] objectColours = new Color[3];

	// Use this for initialization
	void Start () {

        InitialiseData();
	
	}

    void InitialiseData()
    {
        smScript = transform.parent.GetComponent<SpawnManager>();
        GameObject _LevelManager = GameObject.Find("LevelManager");
        lmScript = _LevelManager.GetComponent<LevelManager>();


        PoolWaves();
    }

    void PoolWaves()
    {
        for(int i = 0; i < numberOfWavesToSpawn; i++)
        {
            GameObject _waveHolder = (GameObject)Instantiate(waveHolder);
            _waveHolder.SetActive(false);
            pooledWaveHolders.Add(_waveHolder);
        }
    }

    public void StartWaves()
    {
        InvokeRepeating("SetupWave", 1, 1);
    }

    public void SetupWave()
    {
        GameObject _waveHolder = GetWaveHolder();
        _waveHolder.SetActive(true);

        switch (currentWaveType)
        {
            case (waveType.shape):
                SpawnInShape(_waveHolder);
                break;
            case (waveType.border):
                SpawnInBorder(_waveHolder);
                break;
        }
    }

    GameObject GetWaveHolder()
    {
        GameObject _waveHolder = null;

        for(int i =0; i < pooledWaveHolders.Count; i++)
        {
            if (!pooledWaveHolders[i].activeInHierarchy)
            {
                _waveHolder = pooledWaveHolders[i];
                return _waveHolder;
                
            }
        }

        return _waveHolder;
    }

    void SpawnInBorder(GameObject _wave)
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            int _shapeID = Random.Range(0, smScript.borders.Length);
            string _shape = GetShapeTag(_shapeID);

            for (int j = 0; j < smScript.pooledBorders.Count; j++)
            {
                if (smScript.pooledBorders[j].tag == _shape)
                {
                    if (!smScript.pooledBorders[j].activeInHierarchy)
                    {
                        GameObject _chosenShape = smScript.pooledBorders[j];

                        _chosenShape.GetComponent<SpriteRenderer>().color = ObjectColour();

                        _chosenShape.transform.parent = _wave.transform;
                        _chosenShape.transform.localPosition = spawnPositions[i];

                        bool hasHeart = false;
                        bool hasStar = false;                       


                        if(lmScript.numOfLives < 3)
                        {
                            hasHeart = isHealthPickUpActive();
                        }

                        if (!hasHeart)
                        {
                            hasStar = isStarPickUpActive();
                        }

                        _chosenShape.SetActive(true);

                        if (hasHeart)
                        {
                            ActivatePickUp(_chosenShape.transform.GetChild(0).gameObject);
                        }

                        if (hasStar)
                        {
                            ActivatePickUp(_chosenShape.transform.GetChild(1).gameObject);
                        }

                        currentWaveType = waveType.shape;
                        break;
                    }
                }
            }
        }
    }

    void ActivatePickUp(GameObject _pickUp)
    {
       
        _pickUp.GetComponent<CircleCollider2D>().enabled = true;

        GameObject _sprite = _pickUp.transform.GetChild(0).gameObject;

        _sprite.GetComponent<SpriteRenderer>().enabled = true;
        _sprite.GetComponent<Animation>().enabled = true;
        _sprite.GetComponent<Animation>().Play();
    }

    bool isHealthPickUpActive()
    {
        bool _healthPickUpActive = false;

        int _healthPickUp = Random.Range(0, healthPickUpRate);

        if (_healthPickUp == 1)
        {
            _healthPickUpActive = true;
        }

        return _healthPickUpActive;
    }

    bool isStarPickUpActive()
    {
        bool _starPickUpActive = false;

        int _starPickUp = Random.Range(0, starPickUpRate);
        Debug.Log(_starPickUp);

        if (_starPickUp == 1)
        {
            _starPickUpActive = true;
        }

        return _starPickUpActive;
    }

    void SpawnInShape(GameObject _wave)
    {
        for(int i = 0; i < spawnPositions.Length; i++)
        {
            int _shapeID = Random.Range(0, smScript.shapes.Length);
            string _shape = GetShapeTag(_shapeID);

            for(int j = 0; j < smScript.pooledShapes.Count; j++)
            {
               if(smScript.pooledShapes[j].tag == _shape)
                {
                    if (!smScript.pooledShapes[j].activeInHierarchy)
                    {
                        GameObject _chosenShape = smScript.pooledShapes[j];

                        _chosenShape.GetComponent<SpriteRenderer>().enabled = true;
                        _chosenShape.GetComponent<SpriteRenderer>().color = ObjectColour();

                        _chosenShape.transform.parent = _wave.transform;
                        _chosenShape.transform.localPosition = spawnPositions[i];

                        _chosenShape.SetActive(true);
                        currentWaveType = waveType.border;
                        break;
                    }
                }
            }
        }
    }

    string GetShapeTag(int i)
    {
        string _shapeTag = "";

        switch (i)
        {
            case (0):
                _shapeTag = "Circle";
                break;
            case(1):
                _shapeTag = "Square";
                break;
            case (2):
                _shapeTag = "Hexagon";
                break;
        }

        return _shapeTag;
    }

    Color ObjectColour()
    {
        Color newShapeColour = Color.white;
        int colorID = Random.Range(0, objectColours.Length);
        newShapeColour = objectColours[colorID];
        return newShapeColour;
    }

    public void PauseGame()
    {
        CancelInvoke("SetupWave");

        for(int i = 0; i < pooledWaveHolders.Count; i++)
        {
            if (pooledWaveHolders[i].activeInHierarchy)
            {
                GameObject _currentWave = pooledWaveHolders[i];
                _currentWave.GetComponent<WaveMovement>().PauseGame();
            }
        }
    }

    public void UnPauseGame()
    {
        InvokeRepeating("SetupWave", 1, 1);

        for (int i = 0; i < pooledWaveHolders.Count; i++)
        {
            if (pooledWaveHolders[i].activeInHierarchy)
            {
                GameObject _currentWave = pooledWaveHolders[i];
                _currentWave.GetComponent<WaveMovement>().UnPauseGame();
            }
        }
    }


}
