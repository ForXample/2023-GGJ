using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class HightAndRotationAlign : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject RotateObjectRef;
    public GameObject PlayerRef;
    [Range(0, 360)]
    public float RotateDegreePerUnit;
    [Range(0.1f, 100)]
    public float AccendSpeed;
    [Range(0, 500)]
    public float AccendMaxHight;
    [Range(-50, 500)]
    public float AccendMinHight;

    Transform CurrentPlayerTransform;
    Vector2 CurrentPlayer2DWorldPos;
    Vector2 CurrentPlayer2DWorldDiff;
    Vector2 LastPlayer2DWorldUpdatePos;
    Vector2 DeltaPlayer2DWorldDiff;
    Vector2 Player2DWorldInitPos;
    Vector3 RotationCenterPoint;
    Vector3 ObjectInitalPos;


    void Start()
    {
        RotationCenterPoint = transform.position;
        GetPlayerUpdate();
        Player2DWorldInitPos = CurrentPlayer2DWorldPos;
        ObjectInitalPos = RotateObjectRef.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerUpdate();  
        RotationCenterPoint = transform.position;

        //if (CurrentPlayer2DWorldDiff.sqrMagnitude > 1)
        //{
        LastPlayer2DWorldUpdatePos = CurrentPlayer2DWorldPos;
            
        //Rotation
        float RotateAngle = DeltaPlayer2DWorldDiff.x * RotateDegreePerUnit;
        RotateObjectRef.GetComponent<Transform>().RotateAround(RotationCenterPoint, Vector3.up, RotateAngle * Time.deltaTime);

        //climb
        float newHeight = CurrentPlayer2DWorldDiff.y;
        if(newHeight > AccendMinHight && newHeight < AccendMaxHight)
            RotateObjectRef.transform.position = ObjectInitalPos + new Vector3(0, newHeight * AccendSpeed, 0);
        //}
    }

    void GetPlayerUpdate()
    {
        CurrentPlayerTransform = PlayerRef.GetComponent<Transform>();
        CurrentPlayer2DWorldPos = CurrentPlayerTransform.transform.position;
        CurrentPlayer2DWorldDiff = CurrentPlayer2DWorldPos - Player2DWorldInitPos;
        DeltaPlayer2DWorldDiff = CurrentPlayer2DWorldPos - LastPlayer2DWorldUpdatePos;
    }

    void MyRotateAround(Vector3 center, Vector3 axis, float angle)
    {
        Vector3 pos = RotateObjectRef.transform.position;
        Quaternion rot = Quaternion.AngleAxis(angle, axis);
        Vector3 dir = pos - center; //计算从圆心指向摄像头的朝向向量
        dir = rot * dir;            //旋转此向量
        RotateObjectRef.transform.position = center + dir;//移动摄像机位置
        var myrot = RotateObjectRef.transform.rotation;
        //transform.rotation *= Quaternion.Inverse(myrot) * rot *myrot;//设置角度另一种方法
        RotateObjectRef.transform.rotation = rot * myrot; //设置角度
    }
}
