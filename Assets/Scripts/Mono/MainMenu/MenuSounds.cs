using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour
{
    public AudioSource source;

    public void Click()
    {
        source.Play();
    }
}
