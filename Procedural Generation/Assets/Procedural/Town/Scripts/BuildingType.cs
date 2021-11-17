using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkayns.Procedural.Town {

    [Serializable]
    public class BuildingType {
        [SerializeField]
        private GameObject [] prefabs;
        public int sizeRequired;
        public int quantity;
        public int quantityAlreadyPlaced;

        public GameObject GetPrefab() {
            quantityAlreadyPlaced++;
            if (prefabs.Length > 1) {
                var radom = UnityEngine.Random.Range (0, prefabs.Length);
                return prefabs [radom];
            }
            return prefabs [0];
        } // GetPrefab

        public bool IsBuildingAvailable () {
            return quantityAlreadyPlaced < quantity;
        } // IsBuildingAvailable

        public void Reset () {
            quantityAlreadyPlaced = 0;
        } // Reset

    } // Class BuildingType

} // Namespace Procedural Town