using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Pointer : MonoBehaviour
{
    public LineRenderer Line;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // update the pointer
        Line.SetPosition(0, transform.position);
        Line.SetPosition(1, transform.position + (transform.forward * 20));

        // generate a raycast
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit);

        // check if it hit anything
        if (hit.collider && hit.collider.CompareTag("button"))
        {
            Button button = hit.collider.gameObject.GetComponent<Button>();
            button.Select();
            Line.SetPosition(1, hit.point);

            if (OVRInput.GetDown(OVRInput.Button.Any))
            {
                ExecuteEvents.Execute(button.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
