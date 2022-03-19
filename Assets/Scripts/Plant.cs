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
    private DepletionCreator Dpc;//указание на класс 

    public float consumptionModifier;//надо менять в зависмости от растения. Решается разными префабами
    public float connection;
    public Vector3 rootsCenter;//для rootsSystem
    public float rootsRadius;//для rootsSystem
    public LayerMask rootsLayerMask;//для rootsSystem
    public LayerMask depletionLayerMask;


    List<Fertilizer> F = new List<Fertilizer>();//создаем cписок пересекающихся с растением X fertilizers 
    List<float> Fc = new List<float>();//создаем список connection-ов для растения X
    List<float> C = new List<float>();//создаем список объёма потребления для растения X
    List<float> A = new List<float>();//создаем список значений доступных минералов
    List<Depletion> DPL = new List<Depletion>();//лист для Depletion (по логике деплишн всегда один и лист не нужен,  но пока так)

    int iEll = 0;//итератор для метода CreatingLists - общий, дабы считать листы корректно
    public float timeRemaining = 10; //время для питания растения
    [SerializeField] private int firstDepletionSphere = 1;//флаг для пункта 4.3

    [SerializeField] private GameObject MySoilFormationRef;//для SoilFormationRef
    [SerializeField] private LayerMask SoilLayer;//для SoilFormationRef
    private int SFFlag = 0;//для SoilFormationRef

    private void OnDrawGizmosSelected()//отрисовка OverlapSphere для rootsSystem
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rootsCenter, rootsRadius);
    }
    private void GetMySoilFormationRef()
    {
        Collider[] hitColliders = Physics.OverlapSphere(rootsCenter, rootsRadius, SoilLayer);
        foreach (Collider collider in hitColliders)
        {
            GameObject iterObjectHit = collider.gameObject;
            if (iterObjectHit != null)
            {
                if (iterObjectHit.GetComponent<SoilFormation>() != null)
                {
                    MySoilFormationRef = iterObjectHit;//вариант с GameObject, не с об. класса SoilFormation
                    //Destroy(GetComponent<Rigidbody>());//пока использовал для теста - работает
                    SFFlag = 1;//пока ограничился одним, затем можно будет добавить логику для обновления привязки к слою земли.
                }
            }
        }
    }

    private void CheckForFertilizers()//метод, служащий для поиска fertilizers (в нужный промежуток времени)
    {
        iEll = 0;
        F.Clear(); // the same
        Collider[] hitColliders = Physics.OverlapSphere(rootsCenter, rootsRadius, rootsLayerMask); //массив коллайдеров, пересёкщихся с OverlapSphere. По-идее это переменная rootsSystem
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
            FertilizingAlgorithm(); //то же самое как в деплешоне
        }
    }

    // балансировка потребления из разных источников
    public void CreatingLists(Fertilizer x)//алгоритм заполнения списков удобрений (fertilizers)
    {
        //connection = Mathf.Sqrt(Mathf.Pow((rootsCenter.x - x.transform.position.x), 2) + Mathf.Pow((rootsCenter.y - x.transform.position.y), 2) + Mathf.Pow((rootsCenter.z - x.transform.position.z), 2)) - x.radius - rootsRadius;
        connection = Vector3.Distance(rootsCenter, x.transform.position);

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
        // Ограничить consumption сверху
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
                if (F[i].mineralsReserve == 0)//когда запас иссякнет, изменим A[i] на ноль.
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

        // если поле с деплишном пустое - создаем новый деплишн, иначе работает с тем который есть.
        else if (summ < mineralsConsumptionPerHour)//Шаг 4, пункт 3. Для единственности Depletion для каждого Plant добавить флаг проверки (1,0)
        {
            if (firstDepletionSphere == 1)//КОСТЫЛЬ - сначаласоздаём первый деплишн один раз
            {
                Dpc = GameObject.Find("DepletionHolder").GetComponent<DepletionCreator>();// попробовать перенсти в корень класса
                Dpc.CreateDepletionSphere(rootsCenter, rootsRadius);
                firstDepletionSphere = 0;
            } 
            Collider[] hitColliders = Physics.OverlapSphere(rootsCenter, rootsRadius, depletionLayerMask);// оверлап сфера для поиска деплишн
            foreach (var iter in hitColliders)
            {
                GameObject iterObjectHit = iter.gameObject;
                if (iterObjectHit != null)
                {
                    if (iterObjectHit.GetComponent<Depletion>() != null)//выбираем только те объекты, которые имеют данный класс
                    {
                        DPL.Clear();//очищаем лист, т.к. у одного растения - один деплишн в момент времени. НАДО БУДЕТ УБРАТЬ ЛИСТЫ, хотя работает и так
                        print("Count when finding a dep " + DPL.Count);//проверка
                        DPL.Add(iterObjectHit.GetComponent<Depletion>());//внесли очередной деп.
                    }
                }
                else//ЭТО ПРОДОЛЖЕНИЕ КОСТЫЛЯ - при исчезновении деплишна - создаём новый и еще раз создаем сферу
                {
                    Dpc = GameObject.Find("DepletionHolder").GetComponent<DepletionCreator>();// попробовать перенсти в корень класса
                    Dpc.CreateDepletionSphere(rootsCenter, rootsRadius);
                    Collider[] hitColliders2 = Physics.OverlapSphere(rootsCenter, rootsRadius, depletionLayerMask);// оверлап сфера для поиска деплишн
                    foreach (var iter2 in hitColliders2)
                    {
                        GameObject iterObjectHit2 = iter2.gameObject;
                        if (iterObjectHit2 != null)
                        {
                            DPL.Clear();//очищаем лист, т.к. у одного растения - один деплишн в момент времени. НАДО БУДЕТ УБРАТЬ ЛИСТЫ, хотя работает и так
                            //print("Count when finding a dep " + DPL.Count);//проверка
                            DPL.Add(iterObjectHit2.GetComponent<Depletion>());//внесли очередной деп.
                            print("Ver 2" + DPL.Count);
                        }
                    }
                    
                }
            }

            // Здесь много нужно переделать. 
            int i = 0;
            foreach (Depletion itr in DPL)//Вычисления для шага 4.3
            {
                if (DPL[i].lackMaximum >= mineralsConsumptionPerHour + DPL[i].mineralsLack)
                {
                    minerals += mineralsConsumptionPerHour;
                    print("Podschet v plante"+minerals);
                    if ((mineralsConsumptionPerHour - summ) <= DPL[i].lackMaximum)
                    {
                        DPL[i].mineralsLack += (mineralsConsumptionPerHour - summ);
                    }
                    else
                    {
                        DPL[i].mineralsLack += DPL[i].lackMaximum;
                    }
                }
                else
                {
                    print("mineralsCPH+mineralsLack is more then Maximum ");
                }
                i += 1;
            }
            int j = 0;
            foreach (Fertilizer iter in F)
            {
                F[j].mineralsReserve -= A[j];//
                if (F[j].mineralsReserve == 0)//когда запас иссякнет, изменим A[i] на ноль.
                {
                    A[j] = F[j].mineralsReserve;
                }
                j += 1;
            }
            summ = 0;//обнулим сумму, пересчитаем её заново
            foreach (float a in A)//обновляем сумму
            {
                summ += a;
                print("new summ " + summ);
            }
        }
        A.Clear();
        Fc.Clear();
        C.Clear();
    }
    // Update is called once per frame
    void Update()
    {
        rootsCenter = transform.position;//в будущем убрать т.к. корни неподвижны, пока для теста
        if (SFFlag == 0)//потом можно будет добавить
        {
            GetMySoilFormationRef();
        }
        if (timeRemaining > 0)//таймер для получения растением удобрений
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            CheckForFertilizers(); // Лучше названание вроде ProccedFertilizerAlgoritm
            timeRemaining = 10;
        }
    }
}