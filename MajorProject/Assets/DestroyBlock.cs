using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DestroyBlock : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource source;
    public TMP_Text textbox;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("block") && GetComponent<Renderer>().sharedMaterial == collision.gameObject.GetComponent<Renderer>().sharedMaterial)
        {
            source.PlayOneShot(clip);
            Destroy(collision.gameObject);
            textbox.text = (int.Parse(textbox.text) + 1).ToString();

        }
    }
}
