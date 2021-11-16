using UnityEngine;

namespace Arkayns.P.PT {

    [CreateAssetMenu(menuName = "Procedural City/Rule")]
    public class Rule : ScriptableObject {

        public string letter;
        [SerializeField]
        private string [] results = null;

        public string GetResult () {
            return results [0];
        } // GetResult

    } // Class Rule

} // Namespace PT