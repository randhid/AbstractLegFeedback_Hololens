using UnityEngine;
using System;

public class UDPResponse : MonoBehaviour
{
    public Transform pointPref;

    public float scalefactor_x;
    public float scalefactor_y;
    public float scalefactor_z;
    Transform[] points;

    Vector3 position;
    Vector3 scale;
 
    double hipflexion_r;
    double kneeflexion_r;

    public float thighlength = 0.2f;
    public float shanklength = 0.2f;

    public Material mat;

    public float width = 0.02f;

    void Start()
    {
        scale = new Vector3(scalefactor_x, scalefactor_y, scalefactor_z);
        points = new Transform[3];
        position.y = 0f;
        position.z = 0f;
        
        for(int i =0; i< 3; i ++)
        {
            Transform point = Instantiate(pointPref);
            position.x = 0f;
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform);
            points[i] = point;
        }


        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = mat;
        lineRenderer.widthMultiplier = width;
        lineRenderer.positionCount = 3;
        lineRenderer.receiveShadows = false;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    void Update()
    {
        Transform point0 = points[0];
        Vector3 position0 = point0.localPosition;
        position0.x = 0f;
        position0.y = 0f;
        position0.z = 0f;
        point0.localPosition = position0;
        point0.localScale = scale ;
        
        Transform point1 = points[1];
        Vector3 position1 = point1.localPosition;
        position1.x = thighlength * Mathf.Cos((float)hipflexion_r);+
        position1.y = thighlength * Mathf.Sin((float)hipflexion_r);
        position1.z = 0f;
        point1.localPosition = position1;
        point1.localScale = scale;


        Transform point2 = points[2];
        Vector3 position2 = point2.localPosition;
        position2.x = thighlength * Mathf.Cos((float)hipflexion_r) + shanklength * Mathf.Cos((float)(hipflexion_r + kneeflexion_r));
        position2.y = thighlength * Mathf.Sin((float)hipflexion_r) + shanklength * Mathf.Sin((float)(hipflexion_r + kneeflexion_r));
        position2.z = 0f;
        point2.localPosition = position2;
        point2.localScale = scale;


        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, point0.transform.position);
        lineRenderer.SetPosition(1, point1.transform.position);
        lineRenderer.SetPosition(2, point2.transform.position);

    
    }

    public void ResponseToUDPPacket(string fromIP, string fromPort, byte[] data)
    {
        hipflexion_r = BitConverter.ToDouble(data, 0) - (Mathf.PI / 2);
        //double hipadduction_r = BitConverter.ToDouble(data, 8);
        //double hiprotation_r = BitConverter.ToDouble(data, 16);
        kneeflexion_r = BitConverter.ToDouble(data, 24);


    }
}