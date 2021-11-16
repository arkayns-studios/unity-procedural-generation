using UnityEngine;

namespace Arkayns.P.PS {

    [CreateAssetMenu(menuName = "Procedural City/Rule")]
    public class Rule : ScriptableObject {

        public string letter;
        [SerializeField]
        private string [] results = null;

        public string GetResults () {
            return results [0];
        } // GetResults

    } // Class Rule

} // PS