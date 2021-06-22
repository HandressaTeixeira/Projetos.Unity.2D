using UnityEngine;

public class Gem : MonoBehaviour
{
    public AnimationCurve curve;
    public bool inverted;

    /// <summary>
    /// Posição do diamante (Gem)
    /// </summary>
    private Vector3 gemPossition;

    // Start is called before the first frame update
    void Start()
    {
        //Configuração inicial do movimento da gema
        //No eixo x (horizontal) o diamante não vai se mexer, deve se mexer somente no eixo y (para cima)
        //Para o eixo y informar a velocidade do movimento da gema e a distancia da gema do ponto inicial para o topo.
        curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.8f, 0.2f));
        curve.preWrapMode = WrapMode.PingPong; //Tipo do movimento que a gema vai realizar
        curve.postWrapMode = WrapMode.PingPong;

        gemPossition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Movimento da gema
        if (inverted)
            transform.position = new Vector3
                (gemPossition.x, gemPossition.y - curve.Evaluate(Time.time), gemPossition.z);
        else
            transform.position = new Vector3
                (gemPossition.x, gemPossition.y + curve.Evaluate(Time.time), gemPossition.z);

    }

}
