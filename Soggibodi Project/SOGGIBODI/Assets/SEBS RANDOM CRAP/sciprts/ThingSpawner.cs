using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingSpawner : MonoBehaviour
{
    [SerializeField]Vector3 spawnCentre;
    [SerializeField]Vector3 spawnBoxSize; 

    [SerializeField]float spawnTimer;
    [SerializeField]float spawnTimerMax;

    [SerializeField] List<GameObject> spawnTable = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnTimer <= 0){
            SpawnThing(spawnTable[Random.Range(0, spawnTable.Count)]);
        }
        else{
            spawnTimer -= Time.deltaTime;
        }
    }

    void SpawnThing(GameObject _thing)
    {
        float x = Random.Range(-spawnBoxSize.x / 2, spawnBoxSize.x / 2);
        float y = spawnCentre.y;
        float z = Random.Range(-spawnBoxSize.z / 2, spawnBoxSize.z / 2);

        Instantiate(_thing, new Vector3(x,y,z), Quaternion.identity);
        spawnTimer = spawnTimerMax + (spawnTimer * Random.Range(-0.2f, 0.2f));

        Debug.Log("spawned a " + _thing.name);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnCentre, spawnBoxSize);
    }
}
