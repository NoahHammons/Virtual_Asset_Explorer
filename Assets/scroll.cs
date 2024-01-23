using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroll : MonoBehaviour
{
    public GameObject[] items;
    public GameObject tray;
    private float offset = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        float totalOffset = ((tray.transform.localScale.x / 2) * -1) + (offset / 2);
        foreach (GameObject item in items)
        {
            item.transform.localPosition = new Vector3(totalOffset, item.transform.localPosition.y, item.transform.localPosition.z);
            totalOffset += offset;
        }
        BoxCollider box = GetComponent<BoxCollider>();
        box.size = new Vector3(items.Length * offset * 2f, box.size.y, box.size.z);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
