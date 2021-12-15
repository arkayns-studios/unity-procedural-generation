﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkayns.Procedural.Town {

    public class StructureHelper : MonoBehaviour {

        public BuildingType [] buildingTypes;
        public GameObject [] naturePrefabs;
        public bool randomNaturePlacement;
        [Range (0, 1)]
        public float randomNaturePlacementThreshold = 0.3f;
        public Dictionary<Vector3Int, GameObject> structuresDictionary = new Dictionary<Vector3Int, GameObject> ();
        public Dictionary<Vector3Int, GameObject> natureDictionary = new Dictionary<Vector3Int, GameObject> ();

        public void PlaceStructureAroundRoad (List<Vector3Int> roadPositions) {
            Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpaceAroundRoad (roadPositions);
            List<Vector3Int> blockedPositions = new List<Vector3Int> ();
            foreach (var freeSpot in freeEstateSpots) {
                if (blockedPositions.Contains (freeSpot.Key)) {
                    continue;
                }

                var rotation = Quaternion.identity;
                switch (freeSpot.Value) {
                    case Direction.Up:
                    rotation = Quaternion.Euler (0, 90, 0);
                    break;
                    case Direction.Down:
                    rotation = Quaternion.Euler (0, -90, 0);
                    break;
                    case Direction.Right:
                    rotation = Quaternion.Euler (0, 180, 0);
                    break;
                }

                for (int i = 0; i < buildingTypes.Length; i++) {
                    if (buildingTypes [i].quantity == -1) {
                        if (randomNaturePlacement) {
                            var random = UnityEngine.Random.value;
                            if (random < randomNaturePlacementThreshold) {
                                var nature = SpawnPrefab (naturePrefabs [UnityEngine.Random.Range(0, naturePrefabs.Length)], freeSpot.Key, rotation);
                                natureDictionary.Add (freeSpot.Key, nature);
                                break;
                            }
                        }

                        var building = SpawnPrefab (buildingTypes [i].GetPrefab (), freeSpot.Key, rotation);
                        structuresDictionary.Add (freeSpot.Key, building);
                        break;
                    }

                    if (buildingTypes [i].IsBuildingAvailable ()) {
                        if (buildingTypes [i].sizeRequired > 1) {
                            var halfSize = Mathf.FloorToInt (buildingTypes [i].sizeRequired / 2.0f);
                            List<Vector3Int> tempPositionsBlocked = new List<Vector3Int> ();

                            if (VerifyIfBuildingFits (halfSize, freeEstateSpots, freeSpot, blockedPositions, ref tempPositionsBlocked)) {
                                blockedPositions.AddRange (tempPositionsBlocked);
                                var building = SpawnPrefab (buildingTypes [i].GetPrefab (), freeSpot.Key, rotation);
                                structuresDictionary.Add (freeSpot.Key, building);

                                foreach (var pos in tempPositionsBlocked) {
                                    if (!structuresDictionary.ContainsKey (pos))
                                        structuresDictionary.Add (pos, building);
                                }

                                break;
                            }
                        } else {
                            var building = SpawnPrefab (buildingTypes [i].GetPrefab (), freeSpot.Key, rotation);
                            structuresDictionary.Add (freeSpot.Key, building);
                        }
                        break;
                    }
                }
            }

        } // PlaceStructureAroundRoad

        private bool VerifyIfBuildingFits (int halfSize, Dictionary<Vector3Int, Direction> freeEstateSpots, KeyValuePair<Vector3Int, Direction> freeSpot, List<Vector3Int> blockedPositions, ref List<Vector3Int> tempPositionsBlocked) {
            Vector3Int direction = Vector3Int.zero;
            direction = (freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up) ? Vector3Int.right : new Vector3Int (0, 0, 1);

            for (int i = 0; i <= halfSize; i++) {
                var pos1 = freeSpot.Key + direction * i;
                var pos2 = freeSpot.Key - direction * i;

                if (!freeEstateSpots.ContainsKey (pos1) || !freeEstateSpots.ContainsKey (pos2) || blockedPositions.Contains (pos1) || blockedPositions.Contains (pos2))
                    return false;

                tempPositionsBlocked.Add (pos1);
                tempPositionsBlocked.Add (pos2);
            }
            return true;
        } // VerifyIfBuildingFits

        private GameObject SpawnPrefab (GameObject prefab, Vector3Int position, Quaternion rotation) {
            var newStructure = Instantiate (prefab, position, rotation, transform);
            return newStructure;
        } // SpawnPrefab

        private Dictionary<Vector3Int, Direction> FindFreeSpaceAroundRoad (List<Vector3Int> roadPositions) {
            Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction> ();
            foreach (var position in roadPositions) {
                var neighbourDirections = PlacementHelper.FindNeighbour (position, roadPositions);
                foreach (Direction direction in Enum.GetValues (typeof (Direction))) {
                    if (neighbourDirections.Contains (direction) == false) {
                        var newPosition = position + PlacementHelper.GetOffsetFromDirection (direction);
                        if (freeSpaces.ContainsKey (newPosition))
                            continue;

                        freeSpaces.Add (newPosition, PlacementHelper.GetReverseDirection (direction));
                    }
                }
            }

            return freeSpaces;
        } // FindFreeSpaceAroundRoad

    } // Class StructureHelper

} // Namespace Procedural Town