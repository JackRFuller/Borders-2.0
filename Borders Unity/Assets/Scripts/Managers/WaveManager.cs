using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {

    private SpawnManager smScript;

    [Header("Wave Attributes")]
    public int currentWave;
    public enum waveType
    {
        shape,
        border,
    }
    public waveType currentWaveType;
    public GameObject waveHolder;
    public Vector3[] spawnPositions = new Vector3[3];
    public float waveSpawnRate;

    [Header("Item Attributes")]
    public Color[] objectColours = new Color[3];

	// Use this for initialization
	void Start () {

        InitialiseData();
	
	}

    void InitialiseData()
    {
        smScript = transform.parent.GetComponent<SpawnManager>();
    }

    
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartWaves()
    {
        InvokeRepeating("SetupWave", 3, 3);
    }

    public void SetupWave()
    {
        GameObject _waveHolder = (GameObject)Instantiate(waveHolder);

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

                        _chosenShape.SetActive(true);
                        currentWaveType = waveType.shape;
                        break;
                    }
                }
            }
        }
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


}
