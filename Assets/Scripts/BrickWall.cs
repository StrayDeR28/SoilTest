using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWall : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject Brick;
    public Transform Brick;

    void Start()
    {
        for (int i = 0; i < 10; i++)
            Instantiate(Brick, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
    }
 // Update is called once per frame
    void Update()
    {
        
    }
}
