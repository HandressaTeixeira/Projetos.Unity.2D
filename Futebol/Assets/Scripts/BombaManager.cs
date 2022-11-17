using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaManager : MonoBehaviour
{
    [SerializeField]
    private GameObject bomba;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bola"))
        {
            Instantiate(bomba, new Vector2(this.transform.position.x, this.transform.position.y), 
                Quaternion.identity);
        }
    }

}
