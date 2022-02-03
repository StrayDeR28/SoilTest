using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    //public float minerals;
    //public float water;
    //public float mineralsPerHour;
    //public float waterPerHour;
    //public float mineralsConsumptionRate;
    //public float waterConsumptionRate;
    //public float mineralsPerStage;
    //public Depletion depletion;

    public float consumptionModifier;//надо менять в зависмости от растения. Решается разными префабами
    public float connection;
    public Vector3 rootsCenter;//для rootsSystem
    public float rootsRadius;//для rootsSystem
    public LayerMask rootsLayerMask;//для rootsSystem

    void Start()
    {
       
    }
    private void OnDrawGizmosSelected()//отрисовка сферы
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rootsCenter, rootsRadius);
    }
    // Update is called once per frame
    void Update()
    {
        List<Fertilizer> F = new List<Fertilizer>();//создаем cписок пересекающихся с растением X fertilizers 
        rootsCenter = transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(rootsCenter, rootsRadius, rootsLayerMask);//массив пересеченных какашек. По-идее это переменная rootsSystem
        foreach (var iter in hitColliders)
        {
            GameObject iterObjectHit = iter.gameObject;
            if (iterObjectHit != null)
            {
                if (iterObjectHit.GetComponent<Fertilizer>() != null)//выбираем только те объекты, которые имеют данный класс
                {
                    if (iterObjectHit.GetComponent<Fertilizer>().flagofusage == 0)//проверка на единственность использования объекта
                    {
                        //Fertilizer fertilizer = new Fertilizer(iterObjectHit.GetComponent<Fertilizer >().mineralsReserve, iterObjectHit.GetComponent<Fertilizer>().radius);//вариант с конструктором, хм
                        F.Add(iterObjectHit.GetComponent<Fertilizer>());//внесли очередной ферт. Тут должна быть new!!!! Каким-то образом, иначе лист перезаписывается
                        //F.Add(new Fertilizer(iterObjectHit.GetComponent<Fertilizer>().mineralsReserve, iterObjectHit.GetComponent<Fertilizer>().radius));
                        foreach (Fertilizer fert in F)//проверка на заполнение списка
                        {
                           print(fert.mineralsReserve + " " + fert.radius);
                        }
                        //while (iterObjectHit.GetComponent<Fertilizer>().mineralsReserve > 0)
                        //{
                        //    iterObjectHit.GetComponent<Fertilizer>().mineralsReserve += -2;
                        //}
                        iterObjectHit.GetComponent<Fertilizer>().flagofusage = 1;
                        print (F.Count);
                    }
                }
            }
        }
        /*
        List<float> Fc = new List<float>();//создаем список connection-ов для растения X
        List<float> C = new List<float>();//создаем список объёма потребления для растения X
        List<float> A = new List<float>();//создаем список значений доступных минералов
        foreach (Fertilizer fert in F)
        {
            connection = Mathf.Sqrt(Mathf.Pow((rootsCenter.x - fert.transform.position.x), 2) + Mathf.Pow((rootsCenter.y - fert.transform.position.y), 2) + Mathf.Pow((rootsCenter.z - fert.transform.position.z), 2)) - fert.radius - rootsRadius;
            if (connection < 0)
            {
                connection = Mathf.Abs(connection);
            }
            else
            {
                connection = 0;
            }
            Fc.Add(connection);//заполняем список connection
            foreach (float fc in Fc)//проверка на заполнение списка
            {
               print(fc);
            }
            C.Add((connection * consumptionModifier));//создаем список объёма потребления для растения X
            foreach (float c in C)//проверка на заполнение списка
            {
                print(c);
            }
        }
        foreach (float iter in C)//заполняем лист A - список значений доступных минералов
        {
            foreach (Fertilizer fert in F)
            {
                float reserve = fert.mineralsReserve;
                if (connection >= reserve)
                {
                    A.Add(reserve);
                }
                else
                {
                    A.Add(connection);
                }
            }
            foreach (float a in A)//проверка на заполнение списка
            {
                print("A "+a);
            }
        }
        float summ=0;
        foreach (float a in A)
        {
            summ += a;
            print("summ " + summ);
        }
        //print ("summ"+summ);
    */}
}
