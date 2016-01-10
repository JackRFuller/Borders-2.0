using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

    private WaveManager wmScript;

    [Header("Spawn Items")]
    public GameObject[] borders = new GameObject[3];
    public GameObject[] shapes = new GameObject[3];
    public int numberofBordersToSpawn;
    public int numberOfShapesToSpawn;
    [HideInInspector] public List<GameObject> pooledBorders = new List<GameObject>();
    [HideInInspector] public List<GameObject> pooledShapes = new List<GameObject>();
    public Transform borderHolder;
    public Transform shapeHolder;

	// Use this for initialization
	void Start () {

        wmScript = transform.GetChild(0).GetComponent<WaveManager>();

        PoolBorders();

        PoolShapes();	
	}

    void PoolBorders()
    {
        for(int i = 0; i < borders.Length; i++)
        {
            for(int j = 0; j < numberofBordersToSpawn; j++)
            {
                GameObject border = (GameObject)Instantiate(borders[i]);
                border.transform.parent = borderHolder;
                border.SetActive(false);
                pooledBorders.Add(border);
            }
        }
    }

    void PoolShapes()
    {
        for (int i = 0; i < shapes.Length; i++)
        {
            for (int j = 0; j < numberOfShapesToSpawn; j++)
            {
                GameObject shape = (GameObject)Instantiate(shapes[i]);
                shape.transform.parent = shapeHolder;
                shape.SetActive(false);
                pooledShapes.Add(shape);
            }
        }



        wmScript.StartWaves();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
