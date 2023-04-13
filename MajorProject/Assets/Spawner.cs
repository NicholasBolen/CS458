using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // gameobject that will be spawned
    public GameObject[] blockArray = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnBlock", 2, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnBlock()
    {
        GameObject spawnedBlock = Instantiate(blockArray[Random.Range(0, blockArray.Length)], gameObject.transform.position, Quaternion.identity);

        Rigidbody rb = spawnedBlock.GetComponent<Rigidbody>();

        rb.velocity = new Vector3(0, 0, -1);
    }
}
