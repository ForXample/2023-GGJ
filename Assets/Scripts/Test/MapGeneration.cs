using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;
using Random = UnityEngine.Random;

public class MapGeneration : MonoBehaviour
{

    // --------- Public Properties
    public GameObject PlayerObjectReference;
    public GameObject MapBackgroundReference;
    public GameObject WinZoneObjReference;
    // The array used to store prefab 
    public GameObject[] PrefabList;
    // For every x unit to generate the next map blocks, which result in the density of blocks 
    [Range(2, 50)]
    public float DistanceStepForGeneratingMapBlocks = 3;
    // For every x unit to generate the next map blocks, which result in the density of blocks 
    [Range(20, 50)]
    public float MapBlockGenerationDistanceInAdvance = 20;
    // How many Map Block it will try to generate each time 
    [Range(10, 500)]
    public int MapSize;
    [Range(0, 2)]
    public float MapAscendingSlope;
    // How many Map Block it will try to generate each time 
    [Range(1, 2)]
    public float MapDensityScale;
    [Range(1, 10)]
    public float ScrambaRangeOfMapBlock;
    [Range(1, 10)]

    [Header("Deadzone Follow")]
    public float DeadZoneUpdateFrequency;

    // --------- Private Properties
    private Transform PlayerCurrentTransform;
    private Vector2 CurrentPlayerWorldPos;
    private Vector2 InitialPlayerWorldPos;
    private Vector2 LastUpdatePlayerWorldPos;
    private float InitialPlayerDeadZoneDiff;
    private Vector2 MaxPrefabBoundsInList;


    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // --------- Functions
    void Start()
    {
        if (PrefabList != null)
        {
            //find out the maximum bounds between difference blocks 
            Vector2 maxPrefabBounds = Vector2.zero;
            foreach(GameObject prefab in PrefabList)
            {
                Vector2 currentBounds = new Vector2(GetBounds(prefab).size.x, GetBounds(prefab).size.x);
                if(currentBounds.sqrMagnitude > maxPrefabBounds.sqrMagnitude)
                    maxPrefabBounds = currentBounds;
            }
            MaxPrefabBoundsInList = maxPrefabBounds;
        }
        else
        { 
            Debug.LogError("Cannot Generate Map since no prefab");
        }

        if (PlayerObjectReference != null)
        {
            UpdatePlayerInfo();
            InitialPlayerWorldPos = CurrentPlayerWorldPos;
            InitialPlayerDeadZoneDiff = Vector2.Distance(InitialPlayerWorldPos, (Vector2) transform.position);
        }
        else
        {
            Debug.LogError("Cannot Generate Map since no Player Object Reference");
        }

        // Generate Map Block
        for (float i = CurrentPlayerWorldPos.x; i < MapSize; i += DistanceStepForGeneratingMapBlocks)
        {
            GenerateMapBlocksOnPosition(CurrentMapGenerationLocation(i));
        }

        //Generate WinZone in the end of the map
        if(WinZoneObjReference != null)
            Instantiate(WinZoneObjReference, CurrentMapGenerationLocation(MapSize), Quaternion.identity);

        //Generate Map Background
        if(MapBackgroundReference != null)
        {
            MapBackgroundReference.gameObject.transform.localScale =  Vector3.one * MapSize/2;
            MapBackgroundReference.gameObject.transform.position =  new Vector3(MapSize / 2, MapSize / 2, 50);
            Instantiate(MapBackgroundReference);
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerInfo();

        //Move the game object to update the dead zone
        Vector2 DeltaPlayerWorldPos = CurrentPlayerWorldPos - LastUpdatePlayerWorldPos;
        if(DeltaPlayerWorldPos.x > DeadZoneUpdateFrequency &&      //Can only move towards right && moving down
           DeltaPlayerWorldPos.y > 0)
        {
            LastUpdatePlayerWorldPos = CurrentPlayerWorldPos;
            Vector2 newDeadzoneLocation = CurrentPlayerWorldPos + new Vector2(-1, -1) * InitialPlayerDeadZoneDiff;
            this.transform.position = newDeadzoneLocation;
        }
    }

    void UpdatePlayerInfo()
    {
        PlayerCurrentTransform = PlayerObjectReference.transform;
        CurrentPlayerWorldPos = new Vector2(PlayerCurrentTransform.position.x,
                                            PlayerCurrentTransform.position.y);
    }

    Mesh GenerateBackgroundMesh()
    {
        Mesh backgroundMesh = new Mesh();
        
        float mapWidth = MapSize * 1.1f;
        float mapHight = MapAscendingSlope * MapSize * 1.1f;
        Vector3 meshStartPos = PlayerCurrentTransform.position + Vector3.one * -20;
        float depth = -5;


        backgroundMesh.vertices = new Vector3[] {
            new Vector3(meshStartPos.x, meshStartPos.y, depth),
            new Vector3(0, meshStartPos.y + mapHight,depth),
            new Vector3(meshStartPos.x + mapWidth, meshStartPos.y + mapHight, depth),
            new Vector3(meshStartPos.x + mapWidth, 0 ,depth)
        };

        backgroundMesh.uv = new Vector2[] { new Vector2(0, 0), 
                                            new Vector2(0, 1), 
                                            new Vector2(1, 1), 
                                            new Vector2(1, 0) };

        backgroundMesh.triangles = new int[] { 0, 2, 1, 0, 3, 2 };

        return backgroundMesh;
    }

    //Based on Univariate Quadratic Equation to find the generate map block location 
    Vector2 CurrentMapGenerationLocation(float inX)
    {
        Vector2 returnLocation = Vector2.zero;

        if(PlayerCurrentTransform != null)
        {
            float xValue = inX + MapBlockGenerationDistanceInAdvance;

            float yValue = MapAscendingSlope * inX;         //Equation for map generation line

            returnLocation = new Vector2(xValue, yValue);
        }

        return returnLocation;
    }

    void GenerateMapBlocksOnPosition(Vector2 GenPos)
    {
        if (PrefabList.Length != 0)
        {
            int MapDensity = 4;
            for(int i = 0; i < MapDensity; i++)
            {
                //hard code 
                Vector2 currentGenPos = GenPos;

                switch (i)
                {
                    case 0:         // Right
                        currentGenPos += Vector2.right * MaxPrefabBoundsInList.magnitude * MapDensityScale * Random.Range(-ScrambaRangeOfMapBlock, ScrambaRangeOfMapBlock);
                        break;
                    case 1:         // Up
                        currentGenPos += Vector2.up * MaxPrefabBoundsInList.magnitude * MapDensityScale * Random.Range(-ScrambaRangeOfMapBlock, ScrambaRangeOfMapBlock);
                        break;
                    case 2:         // left
                        currentGenPos += Vector2.left * MaxPrefabBoundsInList.magnitude * Random.Range(-ScrambaRangeOfMapBlock, ScrambaRangeOfMapBlock); 
                        break;
                    case 3:         // Current Location
                        break;
                    default:
                        break;
                }

                int currentPrefabIdx = Random.Range(0, PrefabList.Length);
                GameObject currentGenMapBlock = PrefabList[currentPrefabIdx];
                currentGenMapBlock.isStatic = true;

                Instantiate(currentGenMapBlock, currentGenPos, Quaternion.identity);
            }

        }
    }

    List<Vector2> UniformSampleCircleWithRadius(Vector2 inCenter, float radious)
    {
        List<Vector2> samplePoints = new List<Vector2>();

        float theta = Random.Range(0, radious * 2 * Mathf.PI);
        float r = Mathf.Sqrt(Random.Range(0, Mathf.Pow(radious, 2)));

        return samplePoints;
    }

    Bounds GetBounds(GameObject inObject)
    {
        Bounds bounds;
        BoxCollider2D childBox;
        bounds = GetBoxCollider2DBounds(inObject);
        if (bounds.extents.x == 0)
        {
            bounds = new Bounds(inObject.transform.position, Vector3.zero);
            foreach (Transform child in inObject.transform)
            {
                childBox = child.GetComponent<BoxCollider2D>();
                if (childBox)
                {
                    bounds.Encapsulate(childBox.bounds);
                }
                else
                {
                    bounds.Encapsulate(GetBounds(child.gameObject));
                }
            }
        }
        return bounds;
    }
    Bounds GetBoxCollider2DBounds(GameObject inObject)
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        BoxCollider2D boxCollider = inObject.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            return boxCollider.bounds;
        }
        return bounds;
    }
}
