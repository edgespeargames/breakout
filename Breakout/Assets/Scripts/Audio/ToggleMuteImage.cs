using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMuteImage : MonoBehaviour
{
    [SerializeField] private Sprite muteOff;
    [SerializeField] private Sprite muteOn;

    public void OnToggleMute(bool mute)
    {
        if (mute)
            GetComponent<Image>().sprite = muteOn;
        else
            GetComponent<Image>().sprite = muteOff;
    }

}
