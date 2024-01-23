using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using System.Linq;

public class PreviousDirectory : MonoBehaviour
{
    public GameObject rootDirectory;
    public List<GameObject> previousDirectory = new List<GameObject>();
    public GameObject currentDirectory;

    public bool leftController = false;
    public bool rightController = false;

    public XRNode inputSourceLeft;
    public XRNode inputSourceRight;

    public float time;



    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice deviceLeft = InputDevices.GetDeviceAtXRNode(inputSourceLeft);
        if (deviceLeft.TryGetFeatureValue(CommonUsages.trigger, out float triggerValueLeft) && triggerValueLeft > 0.9f)
        {
            if (leftController)
            {
                leftController = false;
               goToPreviousDirectory();
            }
        }

        InputDevice deviceRight = InputDevices.GetDeviceAtXRNode(inputSourceRight);
        if (deviceRight.TryGetFeatureValue(CommonUsages.trigger, out float triggerValueRight) && triggerValueRight > 0.9f)
        {
            if (rightController)
            {
                rightController = false;
                goToPreviousDirectory();
            }
        }


    }

    public void goToPreviousDirectory()
    {
        if (Time.time < (time +1))
        {
            return;
        }

        if (previousDirectory.Count > 0)
        {
            previousDirectory.First().SetActive(true);     
            if (currentDirectory != null)
            {
                Debug.Log("current directory ");
                Debug.Log(currentDirectory.name);
                currentDirectory.SetActive(false);
            }

        currentDirectory = previousDirectory.First();
        previousDirectory.RemoveAt(0);
        } else {
            currentDirectory.SetActive(false);
            rootDirectory.SetActive(true);
        }
        time = Time.time;
    }



 void OnTriggerEnter(Collider other)
    {
        if (other.name == "Left hand")
        {
            leftController = true;
        }
        if (other.name == "Right hand")
        {
            rightController = true;
        }
    }

     void OnTriggerExit(Collider other)
    {
        if (other.name == "Left hand")
        {
            leftController = false;
        }
        if (other.name == "Right hand")
        {
            rightController = false;
        }
    }
}
