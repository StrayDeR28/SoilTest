using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCaster : MonoBehaviour
{
    public GameObject currentHitObject;

    public float sphereRadius;
    public float maxDistance;
    public LayerMask layerMask;

    public Vector3 origin;
    public Vector3 direction;
    public float currentHitDistance;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnDrawGizmosSelected()//отрисовка сферы
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }

    // Update is called once per frame
    void Update()
    {
        origin = transform.position;//по-идее можно убрать, будет статичной
        //direction = transform.forward;
        RaycastHit hit;
        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))//ласт параметр вероятно только у камеры, надо прочекать что это QuerryTriggerInteraction.UseGlobal
        {
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
        }
        else
        {
            currentHitObject = null;
            currentHitDistance = maxDistance;
        }
    }
}
