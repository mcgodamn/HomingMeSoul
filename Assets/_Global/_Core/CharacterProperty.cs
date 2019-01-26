using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AngerStudio.HomingMeSoul.Game
{
    public class CharacterProperty : MonoBehaviour
    {
        public GameObject collideLocation;
        public Vector3 ForwardVector;
        public Vector3 RotatePoint;
        public bool Ready = true;
        public FloatReference Stamina;

        public void PlayerMove()
        {
            transform.position = transform.position + ForwardVector * Time.deltaTime;
        }


    }
}