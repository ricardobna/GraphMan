using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComidaScript : MonoBehaviour
{
    //public bool visivel = true;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(true);
    }

    void comer()
    {
        this.gameObject.SetActive(false);
    }
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
