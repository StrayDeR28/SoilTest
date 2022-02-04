using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float minerals;
    //public float water;
    public float mineralsConsumptionPerHour;
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

    List<Fertilizer> F = new List<Fertilizer>();//создаем cписок пересекающихся с растением X fertilizers 
    List<float> Fc = new List<float>();//создаем список connection-ов для растения X
    List<float> C = new List<float>();//создаем список объёма потребления для растения X
    List<float> A = new List<float>();//создаем список значений доступных минералов

    int iEll=0;//итератор для метода FertilizngAlgorithm
    void Start()
    {
        
    }
    private void OnDrawGizmosSelected()//отрисовка сферы
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rootsCenter, rootsRadius);
    }
    private void FertilizingAlgorithm(Fertilizer x)
    {
        connection = Mathf.Sqrt(Mathf.Pow((rootsCenter.x - x.transform.position.x), 2) + Mathf.Pow((rootsCenter.y - x.transform.position.y), 2) + Mathf.Pow((rootsCenter.z - x.transform.position.z), 2)) - x.radius - rootsRadius;//раньше было fert
        if (connection < 0)
        {
            connection = Mathf.Abs(connection);
        }
        else
        {
            connection = 0;
        }
        Fc.Add(connection);//заполняем список connection
        print("connection " + Fc[iEll]);//аналогично как и для print F
        C.Add(connection * consumptionModifier);//создаем список объёма потребления для растения X
        print("multiplied connection " + C[iEll]);//аналогично как и для print F
        float reserve = x.mineralsReserve;//было fert
        if (connection * consumptionModifier >= reserve)//смотрим уже на элемент из листа C
        {
            A.Add(reserve);
        }
        else
        {
            A.Add(connection * consumptionModifier);
        }
        print("A " + A[iEll]);//аналогично как и для print F
        float summ = 0;
        foreach (float a in A)// тут избавиться от повторного вывода не получилось
        {
            summ += a;//находим сумму всех a из A
            print("summ " + summ);
        }
        if ((summ > mineralsConsumptionPerHour) & (summ < 2 * mineralsConsumptionPerHour))//Шаг 4, пункт 1
        {
            minerals += summ;//шаг 5
            int i = 0;
            foreach (Fertilizer iter in F)
            {
                F[i].mineralsReserve -= A[i];//должно работать, но при удалении фертилайзера со сцены скорее всего всё полетит. Изменения происходят только в листе, не на самом объекте
                if (F[i].mineralsReserve == 0)//когда запас иссякнет, изменим A[i] на ноль. Потом решить это удаленим фертилайзера
                {
                    A[i] = F[i].mineralsReserve;
                } 
                //F[i].GetComponent<Fertilizer>().mineralsReserve -= A[i];
                i += 1;
            }
            summ = 0;//обнулим сумму, пересчитаем её заново
            foreach (float a in A)//обновляем сумму
            {
                summ += a;//находим сумму всех a из A
                print("new summ " + summ);
            }
        }
        else if (summ > 2 * mineralsConsumptionPerHour)
        {
            int i = 0;
            int j = 0;
            foreach (float a in A)//шаг 5
            {
                A[i] = a / (summ / (2 * mineralsConsumptionPerHour));//должно работать, но при удалении фертилайзера со сцены скорее всего всё полетит
                i += 1;
            }
            summ = 0;//обнулим сумму, пересчитаем её заново
            foreach (float a in A)// тут избавиться от повторного вывода не получилось. Известно, что по формуле сумма будет равна 2*minCPH, но посчитаем на всякий
            {
                summ += a;//находим сумму всех a из A
                print("medium summ " + summ);
            }
            minerals += summ;//шаг 5
            foreach (Fertilizer iter in F)
            {
                F[j].mineralsReserve -= A[j];//должно работать, но при удалении фертилайзера со сцены скорее всего всё полетит
                if (F[j].mineralsReserve == 0)//когда запас иссякнет, изменим A[i] на ноль. Потом решить это удаленим фертилайзера
                {
                    A[j] = F[j].mineralsReserve;
                }
                j += 1;
            }
            foreach (float a in A)//обновляем сумму
            {
                summ += a;//находим сумму всех a из A
                print("new summ " + summ);
            }
        }
        else if (summ < mineralsConsumptionPerHour)
        {
            print("пока ничего, тут шаг 4 пункт 3");
        }
    }
    // Update is called once per frame
    void Update()
    {
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
                        F.Add(iterObjectHit.GetComponent<Fertilizer>());//внесли очередной ферт.
                        print(F[iEll].mineralsReserve + " " + F[iEll].radius);//вывод не всех элементов списка. Будет работать правильно пока не будут удалены элементы списка. В будущем вохможно изменить на foreach
                        //while (iterObjectHit.GetComponent<Fertilizer>().mineralsReserve > 0)
                        //{
                        //    iterObjectHit.GetComponent<Fertilizer>().mineralsReserve += -2;
                        //}
                        iterObjectHit.GetComponent<Fertilizer>().flagofusage = 1;//надо подумать, как это ограничить. Если убрать, бесконечно вписываем в лист 
                        print("Count " + F.Count);
                        FertilizingAlgorithm(F[iEll]);
                        iEll += 1;//итератор для метода сверху, значения совпадают с порядком эл. в списке
                    }
                }
            }
        }
    }
}
