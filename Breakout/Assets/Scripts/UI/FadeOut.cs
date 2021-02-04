using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.instance.Play("");
    }
}
