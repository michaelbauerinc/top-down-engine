using System.Collections;
using System.Collections.Generic;
using Core.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Core.Utils
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        VisualElement characterSelectUi;

        private Dictionary<int, List<VisualElement>> selectScreenMappings = new Dictionary<int, List<VisualElement>>();

        private List<MainMenuController> players = new List<MainMenuController>();

        public GameObject[] playableCharacters;

        private int selectedCharacter;


        void Awake()
        {

            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

        }
        // Start is called before the first frame update
        void Start()
        {
            SceneManager.activeSceneChanged += OnLevelFinishedLoading;
            InitCharSelect();
            Application.targetFrameRate = 60;
        }

        // Update is called once per frame
        void Update()
        {
        }

        void InitCharSelect()
        {
            players.Clear();
            players.Add(GameObject.FindGameObjectsWithTag("Player1")[0].GetComponent<MainMenuController>());

            characterSelectUi = GameObject.Find("CharacterSelectScreen").GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Root");
            IEnumerable<VisualElement> rows = characterSelectUi.Q<VisualElement>("CharacterContainer").Children();
            int i = 0;
            foreach (VisualElement row in rows)
            {
                List<VisualElement> rowToMap = new List<VisualElement>();
                IEnumerable<VisualElement> boxes = row.Children();
                foreach (VisualElement box in boxes)
                {
                    rowToMap.Add(box);
                }
                selectScreenMappings[i] = rowToMap;
                i++;
            }
        }

        void FixedUpdate()
        {
            if (SceneManager.GetActiveScene().name == "char_sel")
            {
                HandlePlayerSelections();

            }
        }

        void HandlePlayerSelections()
        {
            var sel = players[0].selectedCharacter;
            foreach (KeyValuePair<int, List<VisualElement>> entry in selectScreenMappings)
            {
                foreach (var box in entry.Value)
                {
                    box.RemoveFromClassList("slot-selected");
                }
                // do something with entry.Value or entry.Key
            }
            selectScreenMappings[sel.Item1][sel.Item2].AddToClassList("slot-selected");
            foreach (MainMenuController player in players)
            {
                if (player.confirm)
                {
                    LaunchGame(sel.Item2);
                }
            }
        }

        void LaunchGame(int character)
        {
            selectedCharacter = character;
            SceneManager.LoadScene("Sandbox");
        }

        void RestartLayerSwapper()
        {
            Destroy(gameObject.GetComponent<LayerSwapper>());
            gameObject.AddComponent<LayerSwapper>();
        }

        void OnLevelFinishedLoading(Scene previousScene, Scene newScene)
        {
            if (newScene.name == "sandbox")
            {
                Instantiate(playableCharacters[selectedCharacter], new Vector3(-10, 5, 0), Quaternion.identity);
            }
            else if (newScene.name == "char_sel")
            {
                InitCharSelect();
            }

            RestartLayerSwapper();
        }

        void InitSandbox(int character)
        {
        }
    }
}

