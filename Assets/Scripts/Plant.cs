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
    private DepletionCreator Dpc;//указание на класс 

    public float consumptionModifier;//надо менять в зависмости от растения. Решается разными префабами
    public float connection;
    public Vector3 rootsCenter;//для rootsSystem
    public float rootsRadius;//для rootsSystem
    public LayerMask rootsLayerMask;//для rootsSystem

    List<Fertilizer> F = new List<Fertilizer>();//создаем cписок пересекающихся с растением X fertilizers 
    List<float> Fc = new List<float>();//создаем список connection-ов для растения X
    List<float> C = new List<float>();//создаем список объёма потребления для растения X
    List<float> A = new List<float>();//создаем список значений доступных минералов

    int iEll = 0;//итератор для метода CreatingLists - общий, дабы считать листы корректно
    public float timeRemaining = 10; //время для питания растения
    void Start()
    {

    }
    private void OnDrawGizmosSelected()//отрисовка OverlapSphere для rootsSystem
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rootsCenter, rootsRadius);
    }
    private void CheckForFertilizers()//метод, служащий для поиска fertilizers (в нужный промежуток времени)
    {
        iEll = 0;
        F.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(rootsCenter, rootsRadius, rootsLayerMask);//массив коллайдеров, пересёкщихся с OverlapSphere. По-идее это переменная rootsSystem
        foreach (var iter in hitColliders)
        {
            GameObject iterObjectHit = iter.gameObject;
            if (iterObjectHit != null)
            {
                if (iterObjectHit.GetComponent<Fertilizer>() != null)//выбираем только те объекты, которые имеют данный класс
                {
                    F.Add(iterObjectHit.GetComponent<Fertilizer>());//внесли очередной ферт.
                }
            }
        }
        if (F.Count > 0)
        {
            for (int i = 0; i < F.Count; i++)//от сюда формируем список пересекщихся fert в данный промежуток времени (mineralsCPH)
            {
                print(F[i].mineralsReserve + " " + F[i].radius);//вывод значений fertilizers 
                print("Count " + F.Count);
                CreatingLists(F[i]);//создаем List-ы данных для фертов
                iEll += 1;//итератор для метода CreatingLists, значения совпадают с порядком эл. в списке
            }
        }
        else
        {
            FertilizingAlgorithm();
        }
    }
    public void CreatingLists(Fertilizer x)//алгоритм заполнения списков удобрений (fertilizers)
    {
        connection = Mathf.Sqrt(Mathf.Pow((rootsCenter.x - x.transform.position.x), 2) + Mathf.Pow((rootsCenter.y - x.transform.position.y), 2) + Mathf.Pow((rootsCenter.z - x.transform.position.z), 2)) - x.radius - rootsRadius;
        if (connection < 0)
        {
            connection = Mathf.Abs(connection);
        }
        else
        {
            connection = 0;
        }
        Fc.Add(connection);//заполняем список connection
        print("connection " + Fc[iEll]);//проверка, удалить в фин. версии
        C.Add(connection * consumptionModifier);//создаем список объёма потребления для растения X
        print("multiplied connection " + C[iEll]);//проверка, удалить в фин. версии
        float reserve = x.mineralsReserve;
        if (connection * consumptionModifier >= reserve)//смотрим уже на элемент из листа C
        {
            A.Add(reserve);
        }
        else
        {
            A.Add(connection * consumptionModifier);//совпадает с значением из C, можно переписать
        }
        print("A " + A[iEll]);//проверка, удалить в фин. версии
        if (A.Count == F.Count)//когда заполним наши List-ы данных для всех пересеченных fert., вызовём алгоритм, считающий потребление minerals и тд
        {
            FertilizingAlgorithm();
        }
    }
    private void FertilizingAlgorithm()//алгоритм потребления растения удобрений (mierals)
    {
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
                F[i].mineralsReserve -= A[i];//
                if (F[i].mineralsReserve == 0)//когда запас иссякнет, изменим A[i] на ноль. Потом добавить и удаление фертилайзера
                {
                    A[i] = F[i].mineralsReserve;
                } 
                i += 1;
            }
            summ = 0;//обнулим сумму, пересчитаем её заново
            foreach (float a in A)//обновляем сумму
            {
                summ += a;
                print("new summ " + summ);
            }
        }
        else if (summ > 2 * mineralsConsumptionPerHour)//Шаг 4, пункт 2
        {
            int j = 0;
            for (int i= 0; i < A.Count; i++)//перезаполняем элементы листа A
            {
                A[i] = A[i] / (summ / (2 * mineralsConsumptionPerHour));
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
                F[j].mineralsReserve -= A[j];
                if (F[j].mineralsReserve == 0)
                {
                    A[j] = F[j].mineralsReserve;
                }
                j += 1;
            }
            summ = 0;
        }
        else if (summ < mineralsConsumptionPerHour)//Шаг 4, пункт 3. Для единственности Depletion для каждого Plant добавить флаг проверки (1,0)
        {
            Dpc = GameObject.Find("DepletionHolder").GetComponent<DepletionCreator>();// попробовать перенсти в корень класса
            Dpc.CreateDepletionSphere( rootsCenter, rootsRadius);
        }
        A.Clear();
        Fc.Clear();
        C.Clear();
    }
    // Update is called once per frame
    void Update()
    {
        rootsCenter = transform.position;//в будущем убрать т.к. корни неподвижны, пока для теста
        if (timeRemaining > 0)//таймер для получения растением удобрений
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            CheckForFertilizers();
            timeRemaining = 10;
        }
    }
}