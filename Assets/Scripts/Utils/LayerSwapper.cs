using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Utils
{
    public class LayerSwapper : MonoBehaviour
    {
        public List<GameObject> swapObjects = new List<GameObject>();
        void Awake()
        {
            var tags = Object.FindObjectsOfType<MultiTag>();
            foreach (MultiTag tag in tags)
            {
                if (tag.HasTag("layer-swap-object"))
                {
                    swapObjects.Add(tag.gameObject);
                }
            }
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            swapObjects.Sort(SortByPos);
            float i = 0;
            foreach (GameObject toSwap in swapObjects)
            {
                toSwap.transform.position = new Vector3(toSwap.transform.position.x, toSwap.transform.position.y, i);
                // toSwap.transform.Find("shadow").gameObject.transform.position = new Vector3(toSwap.transform.Find("shadow").position.x, toSwap.transform.Find("shadow").position.y, i);
                i += 0.5f;
            }
        }


        static int SortByPos(GameObject obj1, GameObject obj2)
        {
            return obj1.transform.position.y.CompareTo(obj2.transform.position.y);
        }
    }
}
