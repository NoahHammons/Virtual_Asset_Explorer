using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class Folder_system : MonoBehaviour
{

    private InputDevice targetDevice;
    public InputDeviceCharacteristics controllerChrateristics;
    private bool inFolder = false;
    bool isValid = false;
    // Start is called before the first frame update
    void Start()
    {
        getDevices();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isValid)
        {
            getDevices();
        }
        else
        {
            
            if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                Debug.Log("true");
                if (gripValue < 0.1 && inFolder)
                {
                    Debug.Log("infolder");
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Folder")
        {
            
            inFolder = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
         if (other.tag == "Folder")
        {
            
            inFolder = false;
        }
    }

    void getDevices()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);

        InputDevices.GetDevicesWithCharacteristics(controllerChrateristics, devices);
        Debug.Log(devices.Count);
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
