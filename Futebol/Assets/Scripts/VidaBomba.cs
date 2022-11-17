using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaBomba : MonoBehaviour
{

    private GameObject bombaRep;

    void Start()
    {
        bombaRep = GameObject.Find("BarriuExplosivo");
    }

    void Update()
    {
        StartCoroutine(Vida());
    }

    IEnumerator Vida()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(bombaRep);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
       
        
    }
}
