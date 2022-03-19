using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepletionCreator : MonoBehaviour
{
    [SerializeField] private GameObject DepSpherePrefab;

    public void CreateDepletionSphere(Vector3 newCenterOfDep, float newRadiusOfDep) //newRadiusOfDep никуда не идёт. Напомню так же что сюда нужно сунуть логику с присвоением почвы SF
    {
        Instantiate(DepSpherePrefab, newCenterOfDep, Quaternion.identity);
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