using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepletionCreator : MonoBehaviour
{
    //public float radiusOfDep;
    //public float centerOfDep;
    public Transform DepSphere;
    private Depletion Dp;
    public void CreateDepletionSphere(Vector3 newCenterOfDep, float newRadiusOfDep)
    {
        Dp = GameObject.Find("DepletionSphere").GetComponent<Depletion>();
        Dp.gameObject.GetComponent<SphereCollider>().radius = newRadiusOfDep;//всё становится проблемой из-за этого. т.к. у нас везде find будут меняться все depsph на сцене, надо это фиксить
        print(Dp.gameObject.GetComponent<SphereCollider>().radius);
        Instantiate(DepSphere, newCenterOfDep, Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
}