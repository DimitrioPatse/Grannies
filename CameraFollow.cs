using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core {

    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] Vector3 distanceFromPlayer;

        GameObject player;
        void Awake()
        {
            Application.targetFrameRate = 60;
        }

        // Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = player.transform.position + distanceFromPlayer;
        }
    }
}