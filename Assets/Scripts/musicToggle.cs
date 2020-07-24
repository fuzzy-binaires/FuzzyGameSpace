using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class musicToggle : MonoBehaviour
{

    Toggle thisToggle;

    void Start()
    {

        thisToggle = GetComponent<Toggle>();

        thisToggle.onValueChanged.AddListener(delegate {
            toogleMusicPlaying(thisToggle);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void toogleMusicPlaying(Toggle change)
    {
        AudioController.switchAmbientMusic(change.isOn);
    }
}
