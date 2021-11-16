using System.Collections.Generic;
using UnityEngine;

namespace Arkayns.P.PT {

    public class SimpleVisualizer : MonoBehaviour {

        public LSystemGenerator lSystem;
        private List<Vector3> positions = new List<Vector3> ();
        public GameObject prefab;
        public Material lineMaterial;

        private int lenght = 8;
        private float angle = 90;

        public int Lenght {
            get => (lenght > 0) ? lenght : 1;
            set => lenght = value;
        } // Lenght

        private void Start () {
            var sequence = lSystem.GenerateSentence ();
            VisualizeSequence (sequence);
        } // Start

        private void VisualizeSequence (string sequence) {
            Stack<AgentParameter> savePoints = new Stack<AgentParameter> ();
            var currentPosition = Vector3.zero;

            Vector3 direction = Vector3.forward;
            Vector3 tempPosition = Vector3.zero;

            positions.Add (currentPosition);

            foreach (var letter in sequence) {
                EncondingLetter enconding = (EncondingLetter)letter;
                switch (enconding) {
                    case EncondingLetter.save:
                    savePoints.Push (new AgentParameter {
                        position = currentPosition,
                        direction = direction,
                        lenght = Lenght
                    });
                    break;
                    case EncondingLetter.load:
                    if (savePoints.Count > 0) {
                        var agentParameter = savePoints.Pop ();
                        currentPosition = agentParameter.position;
                        direction = agentParameter.direction;
                        Lenght = agentParameter.lenght;
                    } else {
                        throw new System.Exception ("Don't have saved point in our stack!");
                    }
                    break;
                    case EncondingLetter.draw:
                    tempPosition = currentPosition;
                    currentPosition += direction * lenght;
                    DrawLine (tempPosition, currentPosition, Color.red);
                    Lenght -= 2;
                    positions.Add (currentPosition);
                    break;
                    case EncondingLetter.turnR:
                    direction = Quaternion.AngleAxis (angle, Vector3.up) * direction;
                    break;
                    case EncondingLetter.turnL:
                    direction = Quaternion.AngleAxis (-angle, Vector3.up) * direction;
                    break;
                }
            }

            foreach (var position in positions) 
                Instantiate (prefab, position, Quaternion.identity);

        } // VisualizeSequence

        private void DrawLine (Vector3 start, Vector3 end, Color color) {
            GameObject line = new GameObject ("line");
            line.transform.position = start;
            var lineRenderer = line.AddComponent<LineRenderer> ();
            lineRenderer.material = lineMaterial;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition (0, start);
            lineRenderer.SetPosition (1, end);
        } // DrawLine

        public enum EncondingLetter {
            unknown = '1',
            save = '[',
            load = ']',
            draw = 'F',
            turnR = '+',
            turnL = '-'
        } // EncondingLetter

    } // Class SimpleVisualizer

} // Namespace PT