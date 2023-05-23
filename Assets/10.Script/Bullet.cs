using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor") {
            Destroy(gameObject, 3);
        }
        else if (collision.gameObject.tag == "wall"){
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {   if(this.gameObject.layer != 8){
        Destroy(this.gameObject, 2);
    }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
