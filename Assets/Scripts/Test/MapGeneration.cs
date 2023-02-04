using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGeneration : MonoBehaviour
{
    // --------- Public Properties
    public GameObject PlayerObjectReference;
    // The array used to store prefab 
    public GameObject[] PrefabList;
    // For every x unit to generate the next map blocks, which result in the density of blocks 
    [Range(2, 50)]
    public float DistanceStepForGeneratingMapBlocks = 3;
    // For every x unit to generate the next map blocks, which result in the density of blocks 
    [Range(20, 50)]
    public float MapBlockGenerationDistanceInAdvance = 20;
    // How many Map Block it will try to generate each time 
    [Range(500, 2000)]
    public int MapSize;
    // How many Map Block it will try to generate each time 
    [Range(1, 2)]
    public float MapDensityScale;
    [Range(1, 10)]
    public float ScrambaRangeOfMapBlock;

    // --------- Private Properties
    private Transform PlayerCurrentTransform;
    private Vector2 CurrentPlayerWorldPos;
    private float PlayerLatestXPosition;
    private Vector2 MaxPrefabBoundsInList;


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

        if(PlayerObjectReference != null)
        {
            PlayerCurrentTransform = PlayerObjectReference.transform;
            PlayerLatestXPosition = PlayerCurrentTransform.position.x;
            CurrentPlayerWorldPos = new Vector2(PlayerCurrentTransform.position.x,
                                                PlayerCurrentTransform.position.y);
        }
        else
        {
            Debug.LogError("Cannot Generate Map since no Player Object Reference");
        }

        for (float i = CurrentPlayerWorldPos.x; i < MapSize; i += DistanceStepForGeneratingMapBlocks)
        {
            GenerateMapBlocksOnPosition(CurrentMapGenerationLocation(i));
        }


    }

    // Update is called once per frame
    void Update()
    {
        //PlayerCurrentTransform = PlayerObjectReference.transform;
        //CurrentPlayerWorldPos = new Vector2(PlayerCurrentTransform.position.x,
        //                                    PlayerCurrentTransform.position.y);

        ////Map blocks will only be generated when the step limit is exceeded
        //if (PlayerCurrentTransform.position.x - PlayerLatestXPosition > DistanceStepForGeneratingMapBlocks)
        //{
        //    PlayerLatestXPosition = PlayerCurrentTransform.position.x;
        //    Vector2 domainPos = CurrentMapGenerationLocation();

        //    float randomBaseX = Mathf.Abs(ScrambaRangeOfMapBlock.x);
        //    float randomBaseY = Mathf.Abs(ScrambaRangeOfMapBlock.y);
        //    GenerateMapBlocksOnPosition(new Vector2( domainPos.x + Random.Range(-randomBaseX, randomBaseX),
        //                                             domainPos.y + Random.Range(-randomBaseY, randomBaseY)));
        //}
    }

    //Based on Univariate Quadratic Equation to find the generate map block location 
    Vector2 CurrentMapGenerationLocation(float inX)
    {
        Vector2 returnLocation = Vector2.zero;

        if(PlayerCurrentTransform != null)
        {
            float xValue = inX + MapBlockGenerationDistanceInAdvance;

            float yValue = 0.7f * inX;         //Equation for map generation line

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
