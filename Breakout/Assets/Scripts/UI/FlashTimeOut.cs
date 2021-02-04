using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashTimeOut : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(TimeOut());
    }

    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(1.5f);
        //gameObject.GetComponent<Canvas>().sortingOrder = -20;
        Destroy(this.gameObject);
    }
}
