using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depletion : MonoBehaviour
{
    [SerializeField] private GameObject MySoilFormationRef;
    [SerializeField] public float mineralsLack;
    [SerializeField] public float lackMaximum; //будет висеть на объекте на сцене, с которого копировать будем

    [SerializeField] private LayerMask SoilLayer; //для SoilFormationRef
    [SerializeField] private float SFRadius; //для SoilFormationRef
    private Vector3 SFCenter; //для SoilFormationRef
    private int SFFlag = 0;

    List<Fertilizer> F = new List<Fertilizer>(); //создаем cписок пересекающихся с Depletion X fertilizers
    List<float> A = new List<float>(); //создаем список значений доступных минералов. Неправильные названия. Сделай их говорящими, а не А, F
    [SerializeField] private LayerMask FertilizerLayer;
    [SerializeField] private float timeRemaining = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void DepRegenarition() 
    {
        //Добавить зависимость регенерации от объема нехватки.
        mineralsLack -= 2;
        if (mineralsLack <= 0.25)
        {
            Destroyer();
        }
    }

    private void CheckForFertilizers()//Алгоритм регенерации DepletionSphere из Fertilizer-ов. Пока работает не плавно - регенериует на максимально возможное числи minerals
    {
        F = new List<Fertilizer>();
        Collider[] hitColliders = Physics.OverlapSphere(SFCenter, SFRadius, FertilizerLayer);
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
            for (int i = 0; i < F.Count; i++)//отсюда формируем список пересекщихся fert в данный промежуток времени
            {
                CreatingLists(F[i]);//создаем List-ы данных для фертов
            }
        }
        else
        {
            FertilizingAlgorithm(); // не ясно зачем это тут
        }
    }

    public void CreatingLists(Fertilizer x)//алгоритм заполнения списков удобрений (fertilizers)
    {
        if (mineralsLack >= x.mineralsReserve)
        {
            A.Add(x.mineralsReserve);
        }
        else
        {
            A.Add(mineralsLack); // совпадает с значением из C, можно переписать
        }
        if (A.Count == F.Count) // когда заполним наши List-ы данных для всех пересеченных fert., вызовём алгоритм, считающий потребление minerals и тд
        {
            FertilizingAlgorithm(); // Вызывать не тут ^ Лишняя проверка
        }
    }

    // не учтён конкшен
    // равномерно распределять нагрузку по потреблению удобрений
    private void FertilizingAlgorithm() // алгоритм потребления Depletion удобрений (mierals)
    {
        int i = 0;
        foreach (Fertilizer iter in F)
        {
            if (A[i] - mineralsLack < 0) // простая логика для поочередного использования удобрений
            {
                mineralsLack -= A[i];
                F[i].mineralsReserve -= A[i];
                if (F[i].mineralsReserve == 0)//когда запас иссякнет, изменим A[i] на ноль.
                {
                    A[i] = F[i].mineralsReserve;
                }
            }
            else
            {
                
                F[i].mineralsReserve -= mineralsLack;
                A[i] -= mineralsLack;
                mineralsLack = 0;
                //break
            }
            i += 1;
        }
        if (mineralsLack <= 0.25)//удаление Depletion при восстановлении запаса минералов
        {
            Destroyer();
        }
        A.Clear();
    }

    public void Destroyer()//поменять для оптимизации вызов функции: либо вставить её в Plant либо таймер свой // Бесполезная функция) лишние строчки
    {
        Destroy(gameObject, .5f);
    }

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
                    //Destroy(GetComponent<Rigidbody>());//пока использовал для теста - работает
                    SFFlag = 1;//пока ограничился одним, затем можно будет добавить логику для обновления привязки к слою земли.
                }
            }
        }
    }

    //public void SetSoilFormation(GameObject SF)//сделать как сеттер для конструктора деплишн
    //{
    //    MySoilFormationRef = SF;
    //}


    // Update is called once per frame
    void Update()
    {
        SFCenter = transform.position;//для отрисовки сферы каста, затем убрать
        if (SFFlag == 0)//потом можно будет добавить
        {
            GetMySoilFormationRef(); // Лучше убарть
        }
        if (SFFlag != 0)//Запускает таймер только при соприкосновении с SoilFormation, это позволяет логично настроить отдельное время для каждого SoilFormation
        {
            if (timeRemaining > 0)//таймер для получения деплишном удобрений
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                DepRegenarition();
                CheckForFertilizers();
                timeRemaining = 10;
            }
        }
    }
}
