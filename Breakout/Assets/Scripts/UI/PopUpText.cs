using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    public Text text;
    public void SetText(string value)
    {
        text.text = value;
    }

    void Awake()
    {
        StartCoroutine(TimeOut());
    }

    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
