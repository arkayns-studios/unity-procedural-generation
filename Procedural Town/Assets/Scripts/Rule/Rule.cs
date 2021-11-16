using UnityEngine;

namespace Arkayns.P.PT {

    [CreateAssetMenu(menuName = "Procedural City/Rule")]
    public class Rule : ScriptableObject {

        public string letter;
        [SerializeField]
        private string [] results = null;
        [SerializeField]
        private bool isRandom;

        public string GetResult () {
            int index = (isRandom) ? Random.Range (0, results.Length) : 0;
            return results [index];
        } // GetResult

    } // Class Rule

} // Namespace PT