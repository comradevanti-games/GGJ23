using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace TeamShrimp.GGJ23
{
    public class GameManager : MonoBehaviour
    {
        public UnityEvent onGameStarted;
        public UnityEvent onRoundStarted;
        
        private Team currentTeam = Team.Red;

        private void Start()
        {
            IEnumerator WaitAndStart()
            {
                yield return new WaitForSeconds(2); // Wait for map to generate
                StartGame();
            }

            StartCoroutine(WaitAndStart());
        }

        private void StartGame()
        {
            Debug.Log("Start game");
            onGameStarted.Invoke();

            if (Blackboard.IsHost)
                StartRound();
        }

        private void StartRound()
        {
            Debug.Log($"Round started for {currentTeam}");
            onRoundStarted.Invoke();
        }
        
    }
}