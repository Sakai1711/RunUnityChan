using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block2Script3 : MonoBehaviour
{
    float duration;
    Vector3 pos1 = new Vector3(55.5f, -0.5f, 0f);
    //Vector3 pos1 = new Vector3(1f, 0f, 0f);
    Vector3 pos2 = new Vector3(0f, 10f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        duration = 0;
    }

    // Update is called once per frame
    void Update()
    {
        duration += Time.deltaTime;

        if(duration % 5 > 2 && duration % 5 < 4){
            transform.position = pos1;
        }else{
            transform.position = pos2;
        }
    }
}
