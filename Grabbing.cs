using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbing : MonoBehaviour
{
    private Vector3 prev;
    private Collider item;

    // Start is called before the first frame update
    void Start()
    {
        prev = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (item)
        {
            if (item.transform.parent == null)
            {
                item = null;
            }
            else if (OVRInput.Get(OVRInput.RawAxis1D.Any) == 0)
            {
                Vector3 velocity = (this.transform.position - prev) / Time.deltaTime;                
                item.GetComponent<Rigidbody>().isKinematic = false;
                item.attachedRigidbody.velocity = velocity * 2;
                item.transform.parent = null;

                item = null;
            }
        }
        prev = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OVRInput.Get(OVRInput.Axis1D.Any) > 0 && item == null && (other.CompareTag("grabbable") || other.CompareTag("sliceable")))
        {
            item = other;
            item.transform.parent = this.transform;
            
            //offset = item.transform.position - this.transform.position;
            item.attachedRigidbody.isKinematic = true;
        }
    }
}
