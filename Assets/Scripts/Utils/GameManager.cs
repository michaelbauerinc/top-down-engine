using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Core.Utils
{
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 60;
        }

        // Update is called once per frame
        void Update()
        {
            // if (Input.GetKeyDown("r"))
            // {
            //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // }
        }
    }
}

