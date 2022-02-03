using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilizer : MonoBehaviour
{
    public Plant connection;
    //SoilFormation: MySoilFormationRef
    public float mineralsReserve;
    public float radius;
    public float flagofusage = 0;//проверка на единственность использолвания объекта
    public Fertilizer(float newmineralsReserve, float newradius)
    {
        mineralsReserve = newmineralsReserve;
        radius = newradius;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {

    }
}
