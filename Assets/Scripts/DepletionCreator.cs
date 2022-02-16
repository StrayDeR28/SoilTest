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
        Instantiate(DepSphere, newCenterOfDep, Quaternion.identity);
        Dp = GameObject.Find("DepletionSphere").GetComponent<Depletion>();
        Dp.SetParameters(newRadiusOfDep);
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