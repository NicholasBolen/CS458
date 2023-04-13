using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbing : MonoBehaviour
{
    private Vector3 prev;
    private Collider item;
    private OVRInput.RawAxis1D handTrigger;
    public string lr;

    // Start is called before the first frame update
    void Start()
    {
        prev = this.transform.position;
        if (lr == "right")
            handTrigger = OVRInput.RawAxis1D.RHandTrigger;
        if (lr == "left")
            handTrigger = OVRInput.RawAxis1D.LHandTrigger;
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
            else if (OVRInput.Get(handTrigger) == 0)
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
        if (OVRInput.Get(handTrigger) > 0 && item == null && (other.CompareTag("grabbable") || other.CompareTag("sliceable")))
        {
            item = other;
            item.transform.parent = this.transform;
            
            //offset = item.transform.position - this.transform.position;
            item.attachedRigidbody.isKinematic = true;
        }
    }
}
