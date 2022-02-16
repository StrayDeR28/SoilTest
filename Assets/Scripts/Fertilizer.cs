using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilizer : MonoBehaviour
{
    public SoilFormation MySoilFormationRef;
    public float mineralsReserve;
    public float radius;

    public Fertilizer(float newmineralsReserve, float newradius)
    {
        mineralsReserve = newmineralsReserve;
        radius = newradius;
    }
    private void Destoyer()//поменять для оптимизации вызов функции: либо вставить её в Plant либо таймер свой
    {
        if (mineralsReserve < 0.25)
        {
            Destroy(gameObject, .5f);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        Destoyer();
    }
}
