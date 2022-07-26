using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.Utils.Stats
{
    public class StatMap : MonoBehaviour
    {
        [System.Serializable]
        public class StatEntry
        {
            public string name;
            public int value;
        }

        public StatEntry[] _allStats;
        public Dictionary<string, int> allStats = new Dictionary<string, int>();

        void Awake()
        {
            InitStatMap();
        }

        private void InitStatMap()
        {
            foreach (StatEntry stat in _allStats)
            {
                allStats[stat.name] = stat.value;
            }
        }
    }
}
