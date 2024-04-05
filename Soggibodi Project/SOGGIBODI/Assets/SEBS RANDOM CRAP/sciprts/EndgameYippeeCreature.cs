using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameYippeeCreature : MonoBehaviour
{
    [SerializeField] Vector3 spinSpeed;

    [SerializeField] GameObject bloodVFXPrefab, confettiPrefab;

    [SerializeField] AudioSource popNoise;
    [SerializeField] MeshRenderer[] meshes = new MeshRenderer[2];

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Pop", 15);
        Invoke("Quit", 18);
    }

    void Update()
    {
        Vector3 rotationDelta = spinSpeed * Time.deltaTime;
        transform.eulerAngles += rotationDelta;
    }

    // Update is called once per frame
    void Pop()
    {
        // Disable Mesh Renderer
        foreach(MeshRenderer m in meshes){
            m.enabled = false;
        }
        // Make pop popNoise
        popNoise.Play();
        // instantiate hella blood
        confettiPrefab.SetActive(false);
        Instantiate(bloodVFXPrefab, transform.position, Quaternion.identity);
    }

    void Quit()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
