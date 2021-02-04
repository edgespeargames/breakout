using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddlePowerUp : MonoBehaviour
{
    private Vector3 origScale;
    private Vector3 biggerScale;
    private Vector3 smallerScale;

    // Start is called before the first frame update
    void Start()
    {
        origScale  = this.transform.localScale;
        biggerScale = new Vector3(1.5f, 1f, 1f);
        smallerScale = new Vector3(0.5f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("make it big");
            this.transform.localScale = Vector3.Scale(origScale, biggerScale);
        }
        else if(Input.GetMouseButtonUp(1))
        {
            Debug.Log("make it normal");
            this.transform.localScale = origScale;
        }
        

    }
}
