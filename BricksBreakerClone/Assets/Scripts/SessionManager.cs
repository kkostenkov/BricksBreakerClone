using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrickBreaker
{
    public class SessionManager : MonoBehaviour
    {
        [SerializeField]
        private TargetController targetController;

        [SerializeField]
        private GameLostTrigger gameLostTrigger;

        private void Awake()
        {
            this.targetController.AllTargetsDestroyed += OnAllTargetsDestroyed;
            this.gameLostTrigger.TargetReachedGameLostTrigger += OnGameLost;
        }

        private void OnDestroy()
        {
            this.targetController.AllTargetsDestroyed -= OnAllTargetsDestroyed;
            this.gameLostTrigger.TargetReachedGameLostTrigger -= OnGameLost;
        }

        private void OnAllTargetsDestroyed()
        {
            EndSession();
        }

        private void OnGameLost()
        {
            EndSession();
        }

        private void EndSession()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}