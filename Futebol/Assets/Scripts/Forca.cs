using UnityEngine;
using UnityEngine.UI;

public class Forca : MonoBehaviour
{
    private Rigidbody2D bola;
    private float force = 1000f;
    private Rotacao rot;
    public Image seta2Img;

    void Start()
    {
        bola = GetComponent<Rigidbody2D>();
        rot = GetComponent<Rotacao>();
    }

    void Update()
    {
        ControlaForca();
        AplicaForca();
    }

    void AplicaForca()
    {
        //Coseno com conversão de graus para radiano
        float x = force * Mathf.Cos(rot.zRotate * Mathf.Deg2Rad);

        //Seno com conversão de graus para radiano
        float y = force * Mathf.Sin(rot.zRotate * Mathf.Deg2Rad);

        //if (Input.GetKeyUp(KeyCode.Space))
        if (rot.liberaTiro)
            bola.AddForce(new Vector2(x, y));

    }

    void ControlaForca()
    {
        if(rot.liberaRot)
        {
            float moveX = Input.GetAxis("Mouse X");

            //Esquerda
            if(moveX < 0)
            {
                seta2Img.fillAmount += 1 * Time.deltaTime;
                force = seta2Img.fillAmount * 1000;
            }

            //Direita
            if (moveX > 0)
            {
                seta2Img.fillAmount -= 1 * Time.deltaTime;
                force = seta2Img.fillAmount * 1000;
            }
        }
    }
}
