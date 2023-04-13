using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTest : MonoBehaviour
{
    private Vector3 prev;
    private Vector3 offset = new Vector3();
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
            if (OVRInput.Get(OVRInput.RawAxis1D.Any) > 0)
            {
                item.transform.position = this.transform.position + offset;
            }
            else
            {
                Vector3 velocity = (this.transform.position - prev) / Time.deltaTime;                
                item.GetComponent<Rigidbody>().isKinematic = false;
                item.attachedRigidbody.velocity = velocity * 2;

                item = null;
                offset = new Vector3();
            }
        }
        prev = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OVRInput.Get(OVRInput.Axis1D.Any) > 0 && other.CompareTag("grabbable"))
        {
            item = other;
            offset = item.transform.position - this.transform.position;
            item.attachedRigidbody.isKinematic = true;
        }
    }
}
