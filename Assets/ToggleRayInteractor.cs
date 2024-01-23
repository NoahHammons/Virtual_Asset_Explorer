using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleRayInteractor : MonoBehaviour
{
    public XRBaseInteractor leftInteractor;
    public XRBaseInteractor rightInteractor;
    private bool interactorsEnabled = false;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetButtonDown("B Button")) {//Button B
        interactorsEnabled = !interactorsEnabled;
        leftInteractor.enabled = interactorsEnabled;
        rightInteractor.enabled = interactorsEnabled;
    }
    }
}
