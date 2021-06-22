using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    /// <summary>
    /// Obter a posi��o dos fundos
    /// Os fundos s�o o c�u, arvores com o ambiente e as pedras
    /// </summary>
    public Transform[] bgs;

    /// <summary>
    /// Lista de velocidades para cada fundo
    /// </summary>
    public float[] parallaxVel;

    /// <summary>
    /// Movimento suave
    /// </summary>
    public float smooth;

    /// <summary>
    /// Posi��o da camera
    /// </summary>
    public Transform cam;

    private Vector3 previewCam;

    void Start()
    {
        previewCam = cam.position;
    }

    void Update()
    {
        //Percorre a quantidade de backgrounds/fundos
        for (int i = 0; i < bgs.Length; i++)
        {
            //Calculo de posi��o
            //Somente posi��o x porque os fundos se movem somente na horizontal
            float parallax = (previewCam.x - cam.position.x) * parallaxVel[i];
            float targetPosX = bgs[i].position.x - parallax;
            Vector3 targetPos = new Vector3(targetPosX, bgs[i].position.y, bgs[i].position.z);

            //Reposiciona fundo com movimento suave a cada segundo
            bgs[i].position = Vector3.Lerp(bgs[i].position, targetPos, smooth * Time.deltaTime);
        }

        //Atualiza a posi��o da camera porque ela se move com o personagem
        previewCam = cam.position;
    }
}
