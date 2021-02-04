using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(Routine());
    }

    IEnumerator Routine()
    {
        AudioManager.instance.Play("Open");
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.Play("MenuMusic");
    }
}
