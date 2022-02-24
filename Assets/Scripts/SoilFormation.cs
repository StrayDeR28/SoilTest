using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilFormation : MonoBehaviour
{
    //public GameObject SoilPrefab;
    [SerializeField] private SoilFormation x;
    [SerializeField] private Vector3 soilCenter;
    [SerializeField] private float soilRadius;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnDrawGizmosSelected()//отрисовка OverlapSphere для rootsSystem
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(soilCenter, soilRadius);
    }
    public SoilFormation GetSoilFormationRef(GameObject gameObjectе)
    {
        Collider[] hitColliders = Physics.OverlapSphere(soilCenter, soilRadius);
        foreach (Collider collider in hitColliders)
        {
            GameObject iterObjectHit = collider.gameObject;
            if (iterObjectHit != null)
            {
                if (iterObjectHit == gameObjectе)
                {
                    return x;
                }
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        soilCenter = transform.position;
    }
}
