using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkayns.Procedural.Town {

    public class StructureHelper : MonoBehaviour {

        public BuildingType[] buildingTypes;
        public Dictionary<Vector3Int, GameObject> structuresDictionary = new Dictionary<Vector3Int, GameObject> ();

        public void PlaceStructureAroundRoad (List<Vector3Int> roadPositions) {
            Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpaceAroundRoad (roadPositions);
            foreach (var freeSpot in freeEstateSpots) {
                var rotation = Quaternion.identity;
                switch (freeSpot.Value) {
                    case Direction.Up:
                    rotation = Quaternion.Euler (0, 90, 0);
                    break;
                    case Direction.Down:
                    rotation = Quaternion.Euler (0,- 90, 0);
                    break;
                    case Direction.Right:
                    rotation = Quaternion.Euler (0, 180, 0);
                    break;
                }

                for (int i = 0; i < buildingTypes.Length; i++) {
                    if(buildingTypes[i].quantity == -1) {
                        var building = SpawnPrefab (buildingTypes [i].GetPrefab (), freeSpot.Key, rotation);
                        structuresDictionary.Add (freeSpot.Key, building);
                        break;
                    }

                    if (buildingTypes[i].IsBuildingAvailable()) {
                        if (buildingTypes[i].sizeRequired > 1) {

                        } else {
                            var building = SpawnPrefab (buildingTypes [i].GetPrefab (), freeSpot.Key, rotation);
                            structuresDictionary.Add (freeSpot.Key, building);
                        }
                        break;
                    }
                    
                }
            }
                
        } // PlaceStructureAroundRoad

        private GameObject SpawnPrefab (GameObject prefab, Vector3Int position, Quaternion rotation) {
            var newStructure = Instantiate (prefab, position, rotation, transform);
            return newStructure;
        } // SpawnPrefab

        private Dictionary<Vector3Int, Direction> FindFreeSpaceAroundRoad (List<Vector3Int> roadPositions) {
            Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction> ();
            foreach (var position in roadPositions) {
                var neighbourDirections = PlacementHelper.FindNeighbour (position, roadPositions);
                foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
                    if(neighbourDirections.Contains(direction) == false) {
                        var newPosition = position + PlacementHelper.GetOffsetFromDirection (direction);
                        if (freeSpaces.ContainsKey(newPosition)) 
                            continue;

                        freeSpaces.Add (newPosition, PlacementHelper.GetReverseDirection(direction));
                    }
                }
            }

            return freeSpaces;
        } // FindFreeSpaceAroundRoad

    } // Class StructureHelper

} // Namespace Procedural Town