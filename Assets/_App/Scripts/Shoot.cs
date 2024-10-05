using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public GameObject missile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;

                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                
                bool hitOnTouch = Physics.Raycast(ray, out hit);

                if(hitOnTouch)
                {
                    Debug.Log("Touch On Hit: " + hit.transform.name);
                    
                    missile.GetComponent<TurretAI>().currentTarget = hit.transform.gameObject;
                    missile.GetComponent<TurretAI>().Shoot(hit.transform.gameObject);
                    
                }
                else {
                    Debug.Log("Nothing was hit");
                }

            }
        }
    }
}
