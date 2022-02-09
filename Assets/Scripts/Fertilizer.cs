using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilizer : MonoBehaviour
{
    public SoilFormation MySoilFormationRef;
    public float mineralsReserve;
    public float radius;
    public Transform transformm;
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
