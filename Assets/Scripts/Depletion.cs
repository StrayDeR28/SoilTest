using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depletion : MonoBehaviour
{
    [SerializeField] SoilFormation MySoilFormationRef;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetParameters(float newRadius)//GameObject x
    {
        //gameObject.GetComponent<SphereCollider>().radius = newRadius;//всё становится проблемой из-за этого. т.к. у нас везде find будут меняться все depsph на сцене, надо это фиксить
        //print(gameObject.GetComponent<SphereCollider>().radius);
        print("got it");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
