using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decay : MonoBehaviour
{
    //private Collider m_ObjectCollider;
    [SerializeField] private GameObject MySoilFormationRef;
    [SerializeField] private LayerMask SoilLayer;//для SoilFormationRef
    [SerializeField] private float SFRadius;//для SoilFormationRef
    private Vector3 SFCenter;//для SoilFormationRef
    private int SFFlag = 0;

    [SerializeField] private float timeRemaining;
    private void OnDrawGizmosSelected()//отрисовка OverlapSphere для GetMySoilFormationRef()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(SFCenter, SFRadius);
    }
    private void GetMySoilFormationRef()
    {
        Collider[] hitColliders = Physics.OverlapSphere(SFCenter, SFRadius, SoilLayer);
        foreach (Collider collider in hitColliders)
        {
            GameObject iterObjectHit = collider.gameObject;
            if (iterObjectHit != null)
            {
                if (iterObjectHit.GetComponent<SoilFormation>() != null)
                {
                    MySoilFormationRef = iterObjectHit;//вариант с GameObject, не с об. класса SoilFormation
                    Destroy(GetComponent<Rigidbody>());//пока использовал для теста - работает
                    gameObject.GetComponent<SphereCollider>().isTrigger = true;//убираю коллайдер
                    SFFlag = 1;//пока ограничился одним, затем можно будет добавить логику для обновления привязки к слою земли.
                    gameObject.AddComponent<Fertilizer>();//пока здесь, потом реализовать через такймер
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SFCenter = transform.position;//для отрисовки сферы каста, затем убрать
        if (SFFlag == 0)//потом можно будет добавить смену этого флага
        {
            GetMySoilFormationRef();
        }
        //if (SFFlag != 0)
        //{
          //  if (timeRemaining > 0)//таймер для получения растением удобрений
          //  {
          //      timeRemaining -= Time.deltaTime;
          //  }
          //  else
          //  {
          //      gameObject.AddComponent<Fertilizer>();
          //  }
        //}
        
    }
}
