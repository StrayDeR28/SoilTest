using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depletion : MonoBehaviour
{
    [SerializeField] SoilFormation MySoilFormationRef;
    [SerializeField] public float mineralsLack;
    [SerializeField] public float lackMaximum;//будет висеть на объекте на сцене, с которого копировать будем
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Destroyer()//поменять для оптимизации вызов функции: либо вставить её в Plant либо таймер свой
    {
        Destroy(gameObject, .5f);
    }
    // Update is called once per frame
    void Update()
    {
        //Destoyer();
    }
}
