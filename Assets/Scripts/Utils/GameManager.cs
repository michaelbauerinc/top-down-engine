using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Utils
{
    public class GameManager : MonoBehaviour
    {

        public GameObject gorilla;
        public GameObject cat;

        void Awake()
        {
            Instantiate(gorilla, new Vector3(-10, 5, 0), Quaternion.identity);
        }
        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 60;
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}

