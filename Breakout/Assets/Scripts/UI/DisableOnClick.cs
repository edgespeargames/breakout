using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableOnClick : MonoBehaviour
{
    [SerializeField] GameObject panel;
    private float delay = 10f;
    private float time = 0f;

    //Start with enabled; On click set delay to 10 and disable panel; fixed update we add 

    private void Start()
    {
        Disable();
        time = 7f;
    }

    private void FixedUpdate()
    {
        if(time >= delay)
        {
            Enable();
            time = delay;
        }
        else
        {
            time = time + Time.deltaTime;
        }

        if (Input.GetMouseButton(0))
        {
            time = 0f;
            Disable();
        }
    }

    private void Disable()
    {
        panel.SetActive(false);
    }

    private void Enable()
    {
        panel.SetActive(true);
    }
}
