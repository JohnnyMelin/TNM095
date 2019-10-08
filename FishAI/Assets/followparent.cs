using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followparent : MonoBehaviour
{
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = parent.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = parent.transform.position;
    }
}
