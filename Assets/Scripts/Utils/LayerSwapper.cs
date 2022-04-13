using System;
using System.CodeDom;
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
            var tags = FindObjectsOfType<MultiTag>();
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
            try
            {
                swapObjects.Sort(SortByPos);
            }
            catch (Exception)
            {
                // if something dies or is removed
            }

            float z = 0;
            List<GameObject> toSwapTemp = new List<GameObject>(swapObjects);
            foreach (GameObject toSwap in toSwapTemp)
            {
                if (toSwap == null)
                {
                    swapObjects.Remove(toSwap);
                }
                else
                {
                    toSwap.transform.position = new Vector3(toSwap.transform.position.x, toSwap.transform.position.y, z);
                    z += 0.5f;
                }
            }
        }

        public int SortByPos(GameObject obj1, GameObject obj2)
        {
            return obj1.transform.position.y.CompareTo(obj2.transform.position.y);
        }
    }
}
