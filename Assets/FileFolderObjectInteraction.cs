using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class FileFolderObjectInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 initialPosition;
    public Vector3 initialOrientation;

    private Vector3 pos;
    private Quaternion rot;


    private bool hasBeenThrown = false;

    public GameObject garbageCan;
    public GameObject openWithClipboard1;
    public GameObject openWithClipboard2;

    public GameObject previewObject;
    public bool previewObjectRotating = false;

    public XRBaseInteractor leftInteractor;
    public XRBaseInteractor rightInteractor;

    public float time;
    private float timePassed;
    private bool grabbed = false;

    bool leftGrab = false;
    bool rightGrab = false;
    bool isValid = false;

    private InputDevice targetDevice;
    public InputDeviceCharacteristics controllerChrateristics;

    AudioSource m_MyAudioSource;

    //Play the music
    bool m_Play;
    //Detect when you use the toggle, ensures music isn’t played multiple times
    bool m_ToggleChange;
    // Start is called before the first frame update
    void Start()
    {
        //Fetch the AudioSource from the GameObject
        m_MyAudioSource = GetComponent<AudioSource>();
        //Ensure the toggle is set to true for the music to play at start-up
        
        time = Time.time;
        timePassed = 0;
        pos = transform.position;
        rot = transform.rotation;
        getDevices();
    }

    private Vector3 scaleChange = new Vector3(.1f, .1f, .1f);
    // Update is called once per frame
    void Update()
    {
        
        if (m_MyAudioSource != null)
        {
            if ((grabbed && !m_Play) || (!grabbed && m_Play))
            {
                m_Play = !m_Play;
                m_ToggleChange = true;
            }
            if (m_Play == true && m_ToggleChange == true)
            {
                //Play the audio you attach to the AudioSource component
                m_MyAudioSource.Play();
                //Ensure audio doesn’t play more than once
                m_ToggleChange = false;
            }
            if (m_Play == false && m_ToggleChange == true)
            {
                //Stop the audio
                m_MyAudioSource.Stop();
                //Ensure audio doesn’t play more than once
                m_ToggleChange = false;
            }
        }
        if (!isValid)
        {
            getDevices();
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {

            if ((leftGrab && rightGrab) && (gripValue > 0.1))
            {
                if (previewObject.tag == "asset")
                {
                    previewObject.transform.localScale += scaleChange;
                }

                

            }

            if ((leftGrab || rightGrab) && !grabbed)
            {
                grabbed = true;
                timePassed = Time.time;
            }
            if ((!leftGrab && !rightGrab) && (Time.time - timePassed) > 5)
            {
                transform.position = pos;
                transform.rotation = rot;
                grabbed = false;
            }



        }
        if(previewObject != null)
        {
            if (previewObject.tag == "asset")
            {
                previewObject.transform.rotation = transform.rotation;
                previewObject.transform.rotation *= Quaternion.Euler(new Vector3(0, -90, 0));
            }
        }
        
            
        if (hasBeenThrown)
        {
                if (GetComponent<Collider>().bounds.Intersects(garbageCan.GetComponent<Collider>().bounds))
                {
                    Destroy(gameObject);
                }
                if (GetComponent<Collider>().bounds.Intersects(openWithClipboard1.GetComponent<Collider>().bounds) || GetComponent<Collider>().bounds.Intersects(openWithClipboard2.GetComponent<Collider>().bounds))
                {
                    // open with clipboard
                }
                if(gameObject != null ){ 
                    resetAfterThrownIfNoCollisions();
                }
        }        

        if(previewObject != null)
        {
            if (previewObjectRotating)
            {
                previewObject.transform.Rotate(0, 1, 0);
            }
        }
    }

    public void setInitialPosition()
    {
        initialPosition = transform.position;
        initialOrientation = transform.eulerAngles;
    }

    public void showPreview()
    {
        // slowly rotate around in a circle 
        previewObject.SetActive(true);
    }

    public void hidePreview()
    {
        previewObject.SetActive(false);
    }

    public void resetAfterThrownIfNoCollisions()
    {
            if ((time +3)  < Time.time && GetComponent<Collider>().bounds.Intersects(garbageCan.GetComponent<Collider>().bounds) == false
              && GetComponent<Collider>().bounds.Intersects(openWithClipboard1.GetComponent<Collider>().bounds) == false 
              && GetComponent<Collider>().bounds.Intersects(openWithClipboard2.GetComponent<Collider>().bounds) == false)
                {
                    resetPosition();
                    hasBeenThrown = false;
                }
    }

    public void setHasBeenThrown()
    {
        hasBeenThrown = true;
        time = Time.time;
    }


    public void resetPosition()
    {

        if (initialPosition != null)
        {
            transform.position = initialPosition;
        }
        if (initialOrientation != null)
        {
            transform.eulerAngles = initialOrientation;
        }

    }

    // listen for collisions
    void OnCollisionEnter(Collision collision)
    {
        if (hasBeenThrown){
            if (collision.gameObject.name == "GarbageCan")
            {
                Destroy(gameObject);
            }
            if (collision.gameObject.name == "OpenWithClipboard1" || collision.gameObject.name == "OpenWithClipboard2")
            {
                // open with clipboard
            }
        }
    }

   
    private void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.name == "Left hand")
        {
            leftGrab = true;
            
        }
        if (other.gameObject.name == "Right hand")
        {
            rightGrab = true;
           
        }

        if (other.gameObject.name == "Left hand" || other.gameObject.name == "Right hand")
        {  
            leftInteractor.enabled = false;
            rightInteractor.enabled = false;
            if (previewObject != null)
            {
                showPreview();
            }
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.name == "Left hand")
        {
            leftGrab = false;
            
        }
        if (other.gameObject.name == "Right hand")
        {
            rightGrab = false;
            
        }
        if ((!leftGrab && !rightGrab))
        {
            leftInteractor.enabled = true;
            rightInteractor.enabled = true;
            if (previewObject != null)
            {
                hidePreview();
                if (previewObject.tag == "asset")
                {
                    previewObject.transform.localScale = new Vector3(1f, 1f, 1f);
                }
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
