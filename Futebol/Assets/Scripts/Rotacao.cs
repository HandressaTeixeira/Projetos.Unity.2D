using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotacao : MonoBehaviour
{

    /// <summary>
    /// Posição Inicial
    /// </summary>
    [SerializeField] private Transform posStart;

    /// <summary>
    /// Imagem da Seta
    /// </summary>
    [SerializeField] private Image setaImg;

    //Angulo
    public float zRotate;

    public bool liberaRot = false;
    public bool liberaTiro = false;

    void Start()
    {
        PosicionaSeta();
        PosicionaBola();
    }

    void Update()
    {
        //InputDeRotacao();
        MouseRotacao();
        RotacaoSeta();
        LimitaRotacao();
    }

    void PosicionaSeta()
    {
        setaImg.rectTransform.position = posStart.position;
    }

    void PosicionaBola()
    {
        this.gameObject.transform.position = posStart.position;
    }

    void RotacaoSeta()
    {
        setaImg.rectTransform.eulerAngles = new Vector3(0, 0, zRotate);
    }

    /// <summary>
    /// Eventos de movimentação da seta pelo teclado
    /// </summary>
    void InputDeRotacao()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            zRotate += 2.5f;

        if (Input.GetKey(KeyCode.DownArrow))
            zRotate -= 2.5f;

    }

    /// <summary>
    /// Eventos de movimentação da seta pelo mouse
    /// </summary>
    void MouseRotacao()
    {
        if (liberaRot)
        {
            //float moveX = Input.GetAxis("Mouse X");
            float moveY = Input.GetAxis("Mouse Y");


            if (zRotate < 90)
                if (moveY > 0)
                    zRotate += 2.5f;

            if (zRotate > 0)
                if (moveY < 0)
                    zRotate -= 2.5f;

            //if (zRotate > 0)
            //    if (moveX > 0)
            //        zRotate -= 2.5f;
        }
    }

    void LimitaRotacao()
    {
        if (zRotate >= 90)
            zRotate = 90;

        if (zRotate <= 0)
            zRotate = 0;
    }

    private void OnMouseDown()
    {
        liberaRot = true;
    }

    private void OnMouseUp()
    {
        liberaRot = false;
        liberaTiro = true;
    }
}
