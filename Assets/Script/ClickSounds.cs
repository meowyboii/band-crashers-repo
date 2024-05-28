using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSounds : MonoBehaviour
{
    public void PlayButtonClickSound()
    {
        FindObjectOfType<AudioManager>().Play("ClickSound");
    }
}