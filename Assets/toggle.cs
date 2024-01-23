using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class toggle : MonoBehaviour
{

    bool isValid = false;
    bool canPress = true;

    public GameObject belt;
    public GameObject _360;
    public GameObject audio;
    public int count = 0;
    private InputDevice targetDevice;
    public InputDeviceCharacteristics controllerChrateristics;
    public int limit = 2;

    // Start is called before the first frame update
    void Start()
    {
        getDevices();
    }
    
    void increment()
    {
        count ++;
        if(count > limit)
        {
            count = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isValid)
        {
            getDevices();
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool press))
        {
            
            if (canPress && press)
            {
                canPress = false;
                if(count == 0)
                {
                    belt.SetActive(true);
                    _360.SetActive(false);
                    audio.SetActive(false);
                }
                else if (count == 1)
                {
                    _360.SetActive(true);
                    belt.SetActive(false);
                    audio.SetActive(false);
                }
                else if (count == 2)
                {
                    audio.SetActive(true);
                    belt.SetActive(false);
                    _360.SetActive(false);
                }
                increment();


            }
            if (!press)
            {
                canPress = true;
            }
        }
    }

    void getDevices()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);

        InputDevices.GetDevicesWithCharacteristics(controllerChrateristics, devices);

        if (devices.Count > 0)
        {
            isValid = true;
            foreach (var item in devices)
            {
                Debug.Log(item.name + ". " + item.characteristics);
            }
            targetDevice = devices[0];

        }
    }
}
