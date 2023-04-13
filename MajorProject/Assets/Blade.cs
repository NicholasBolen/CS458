using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blade : MonoBehaviour
{
    private GameObject sTip, sBase;
    private Vector3 enterPos1, enterPos2, exitPos;
    public TMP_Text textbox;
    
    [SerializeField]
    [Tooltip("The amount of force applied to each side of a slice")]
    private float _forceAppliedToCut = 3f;


    // Start is called before the first frame update
    void Start()
    {
        sTip = this.transform.Find("Tip").gameObject;
        sBase = this.transform.Find("Base").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("sliceable"))
        {
            enterPos1 = sBase.transform.position;
            enterPos2 = sTip.transform.position;
            other.gameObject.layer = LayerMask.NameToLayer("Cutting");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("sliceable"))
        {
            exitPos = sTip.transform.position;

            //Create a triangle between the tip and base so that we can get the normal
            Vector3 side1 = exitPos - enterPos1;
            Vector3 side2 = exitPos - enterPos2;

            //Get the point perpendicular to the triangle above which is the normal
            //https://docs.unity3d.com/Manual/ComputingNormalPerpendicularVector.html
            Vector3 normal = Vector3.Cross(side1, side2).normalized;

            //Transform the normal so that it is aligned with the object we are slicing's transform.
            Vector3 transformedNormal = ((Vector3)(other.gameObject.transform.localToWorldMatrix.transpose * normal)).normalized;

            //Get the enter position relative to the object we're cutting's local transform
            Vector3 transformedStartingPoint = other.gameObject.transform.InverseTransformPoint(enterPos1);

            Plane plane = new Plane();

            plane.SetNormalAndPosition(
                    transformedNormal,
                    transformedStartingPoint);

            var direction = Vector3.Dot(Vector3.up, transformedNormal);

            //Flip the plane so that we always know which side the positive mesh is on
            if (direction < 0)
            {
                plane = plane.flipped;
            }

            GameObject[] slices = Slicer.Slice(plane, other.gameObject);
            Destroy(other.gameObject);

            Rigidbody rigidbody = slices[1].GetComponent<Rigidbody>();
            Vector3 newNormal = transformedNormal + Vector3.up * _forceAppliedToCut;
            rigidbody.AddForce(newNormal, ForceMode.Impulse);

            textbox.text = (int.Parse(textbox.text) + 1).ToString();
        }
    }
}
