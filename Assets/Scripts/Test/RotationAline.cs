using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAline : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject RotateObjectRef;
    public GameObject PlayerRef;
    [Range(0, 360)]
    public float RotateDegreePerUnit;
    [Range(0, 500)]
    public float AccendMaxHight;

    Transform CurrentPlayerTransform;
    Vector2 CurrentPlayer2DWorldPos;
    Vector2 CurrentPlayer2DWorldDiff;
    Vector2 LastPlayer2DWorldUpdatePos;
    Vector2 DeltaPlayer2DWorldDiff;
    Vector2 Player2DWorldInitPos;
    Vector2 RotationCenterPoint;


    void Start()
    {
        RotationCenterPoint = transform.position;
        GetPlayerUpdate();
        Player2DWorldInitPos = CurrentPlayer2DWorldPos;

    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerUpdate();
        RotationCenterPoint = transform.position;

        if (CurrentPlayer2DWorldDiff.sqrMagnitude > 1)
        {
            LastPlayer2DWorldUpdatePos = CurrentPlayer2DWorldPos;
            //Vector3 newHightPos = this.GetComponent<Transform>().position + new Vector3(0, , 0);
            //float RotateAngle = DeltaPlayer2DWorldDiff.x * RotateDegreePerUnit;
            //RotateObjectRef.GetComponent<Transform>().RotateAround(RotationCenterPoint, Vector3.up, RotateAngle * Time.deltaTime);
        }
        float RotateAngle = 1 * RotateDegreePerUnit;
        RotateObjectRef.GetComponent<Transform>().RotateAround(RotationCenterPoint, Vector3.up, RotateAngle * Time.deltaTime);
    }

    void GetPlayerUpdate()
    {
        CurrentPlayerTransform = PlayerRef.GetComponent<Transform>();
        CurrentPlayer2DWorldPos = CurrentPlayerTransform.transform.position;
        CurrentPlayer2DWorldDiff = CurrentPlayer2DWorldPos - Player2DWorldInitPos;
        DeltaPlayer2DWorldDiff = CurrentPlayer2DWorldPos - LastPlayer2DWorldUpdatePos;
    }
}
