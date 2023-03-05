using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider SliderObject;

    // Start is called before the first frame update
    void Start()
    {
        SliderObject.value = AudioListener.volume;   
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = SliderObject.value;
    }
}
