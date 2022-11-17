using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspetosManager : MonoBehaviour
{

    private SliderJoint2D espeto;
    private JointMotor2D motor;

    void Start()
    {
        espeto = GetComponent<SliderJoint2D>();
        motor = espeto.motor;
    }

    void Update()
    {
        if(espeto.limitState == JointLimitState2D.UpperLimit)
        {
            //Para que a velocidade negativa do espeto fique aleatória
            motor.motorSpeed = Random.Range(-1, -5);
            espeto.motor = motor;
        }

        if (espeto.limitState == JointLimitState2D.LowerLimit)
        {
            //Para que a velocidade positiva do espeto fique aleatória
            motor.motorSpeed = Random.Range(1, 5);
            espeto.motor = motor;
        }
    }
}
