using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneAudio : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(Routine());
    }

    IEnumerator Routine()
    {
        AudioManager.instance.Play("Open");
        yield return new WaitForSeconds(1f);
        AudioManager.instance.Play("GameMusic");
    }
}
