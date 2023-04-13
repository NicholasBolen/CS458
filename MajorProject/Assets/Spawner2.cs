using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner2 : MonoBehaviour
{
    // gameobject that will be spawned
    public GameObject[] blockArray = new GameObject[2];
    public Material[] matArray = new Material[7];

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObject", 2, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObject()
    {
        Invoke("ActuallySpawnObject", Random.Range(0, 1.5f));
    }

    void ActuallySpawnObject()
    {
        GameObject spawnedBlock = Instantiate(blockArray[Random.Range(0, blockArray.Length)], gameObject.transform.position, Quaternion.identity);
        spawnedBlock.GetComponent<Renderer>().material = matArray[Random.Range(0, matArray.Length)];
        Rigidbody rb = spawnedBlock.GetComponent<Rigidbody>();

        rb.velocity = new Vector3(0, Random.Range(3, 6), 1);
    }
}
