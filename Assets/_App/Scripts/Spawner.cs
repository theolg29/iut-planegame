using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public static Spawner Instance { get; private set; }

    public GameObject RedPlane;
    public GameObject GreenPlane;
    public GameObject RainbowPlane;

    private Vector3 missilePosition;

    void Awake() 
    {
        
        if(Instance != null) {
            Destroy(Instance);
        }
        else
        {
            Instance = this;

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMissilePosition(Vector3 mPose)
    {
        this.missilePosition = missilePosition;
    }


    public void spawnPlane(GameObject plane)
    {
        // empty object to work with the transform
        GameObject planeSpawnPoint = new GameObject();


        // random height for the plane
        // assign a random value between (range) 0.5f, 1.5f
        float extraHeight = Random.Range(.5f, 1.5f);


        // random depth for the plane's Z axis
        // same, assign a random value between .5f, 1.5f
        float extraDepth = Random.Range(.5f, 1.5f);

        // plane position
        Vector3 planeSpawnPosition = new Vector3(.0f, this.missilePosition.y + extraHeight, this.missilePosition.z + extraDepth);

        // set the position of the spawn
        planeSpawnPoint.transform.position = planeSpawnPosition;

        // instantiate a plane at the spawn position
        Instantiate(plane, planeSpawnPoint.transform);

    }

    public void spawnRedPlane()
    {
        spawnPlane(this.RedPlane);
    }

    public void spawnGreenPlane()
    {
        spawnPlane(this.GreenPlane);
    }

    public void spawnRainbowPlane()
    {
        spawnPlane(this.RainbowPlane);
    }

}
