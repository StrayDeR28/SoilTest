using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decay : MonoBehaviour
{
    [SerializeField] private GameObject MySoilFormationRef;
    [SerializeField] private GameObject FertilizerPrefab;
    Coroutine dt;//для IEnumerator DecayTime
    
    private void SpawnFetilizer()
    {
        Vector3 position = transform.position;
        Instantiate(FertilizerPrefab, position, Quaternion.identity);
        Destroy(gameObject);
    }
    IEnumerator DecayTime(float timer)//Время гниения яблока
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            print(timer);
            yield return null;
        }
        SpawnFetilizer();
        yield return null;
    }
    private void OnCollisionEnter(Collision col)//Изменения получения MySoilFormationRef. При падении на землю запускается корутин
    {
        if (col.collider.GetComponent<SoilFormation>() != null)
        {
            MySoilFormationRef = col.gameObject;
            dt = StartCoroutine(DecayTime(3));
        }
    }
    private void OnCollisionExit(Collision col)//При подборе яблока, время гниения сбрасывается
    {
        if (col.collider.GetComponent<SoilFormation>() != null)
        {
            MySoilFormationRef = null;
            StopCoroutine(dt);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
