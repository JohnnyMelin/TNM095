using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caustics : MonoBehaviour
{

    private Projector projector;
    public MovieTexture mt;

    // Start is called before the first frame update
    void Start()
    {
        projector = GetComponent<Projector>();
        projector.material.SetTexture("_ShadowTex", mt);
        mt.loop = true;
        mt.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
