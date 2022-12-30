using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderBusController : MonoBehaviour
{
    [SerializeField] private string BusName = "Master";
    private Slider _slider;
    public void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void SetBusVolume()
    {
        AudioManager.Instance.SetBusVolume(BusName, _slider.value);
    }
}
