using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using System.Linq;


public class DirectoryActions : MonoBehaviour
{
    public GameObject childDirectory;
    public GameObject thisDirectory;
    public GameObject previousDirectoryManager;
    
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
                if (Time.time <(time + 2)){
                    return;
                }
                time = Time.time;
                goToChildDirectory();
                leftController = false;
            }
        }

        InputDevice deviceRight = InputDevices.GetDeviceAtXRNode(inputSourceRight);
                if (deviceRight.TryGetFeatureValue(CommonUsages.trigger, out float triggerValueRight) && triggerValueRight > 0.9f)
        {
            if (rightController)
            {
                // Debug.Log("right controller");
                // Debug.Log(childDirectory.name);
                Debug.Log("right controller");

               if (Time.time <(time + 2)){
                    return;
                }
                time = Time.time;
                goToChildDirectory();
                rightController = false;
            }
        }


       
    }

    public void goToChildDirectory()
    {
        if (thisDirectory.activeSelf)
        {
            Debug.Log("go to child directory");
           //moveThisDirectoryDown();
           showChildDirectory();  
           hideThisDirectory();
           previousDirectoryManager.GetComponent<PreviousDirectory>().currentDirectory = childDirectory;
            previousDirectoryManager.GetComponent<PreviousDirectory>().previousDirectory.Add(thisDirectory);
        }
    }

    // public void moveThisDirectoryDown()
    // {
    //     Debug.Log("activeDirectory");
    //     Debug.Log(thisDirectory.activeSelf);
    //     if (thisDirectory.activeSelf)
    //     {
    //         thisDirectory.transform.position = new Vector3(thisDirectory.transform.position.x, thisDirectory.transform.position.y - 0.1f, thisDirectory.transform.position.z);
    //     }
    // }

    public void hideThisDirectory(){
        if (thisDirectory.activeSelf)
        {
            thisDirectory.SetActive(false);
        }
    }

    public void showChildDirectory(){
        if (thisDirectory.activeSelf)
        {
            childDirectory.SetActive(true);
        }
    }

    // public void moveThisDirectoryUp()
    // {
    //     if (thisDirectory.activeSelf)
    //     {
    //         thisDirectory.transform.position = new Vector3(thisDirectory.transform.position.x, thisDirectory.transform.position.y + 0.1f, thisDirectory.transform.position.z);
    //     }
    // }


void OnTriggerEnter(Collider other)
    {
        if (other.name == "Left hand")
        {
            leftController = true;
        }
        if (other.name == "Right hand" )
        {
            rightController = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
        Debug.Log(other.name);
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


