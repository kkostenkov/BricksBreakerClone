using UnityEngine;

namespace BrickBreaker
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField]
        private TargetController targetController;
        [SerializeField]
        private BallSpawner ballSpawner;
        [SerializeField]
        private GameLostTrigger gameLostTrigger;
        [SerializeField]
        private ScoreMultiplicatorPopup multipicatorPopup;
        [SerializeField]
        private LeaderboardPopup leaderboardPopup;
        
        private void Awake()
        {
            InstallDependencyInjection();
            // var buildingCursor = DI.Game.Resolve<IBuildingCursor>() as BuildCursorComponent;
        }

        private void InstallDependencyInjection()
        {
            DI.CreateGameContainer();
            DI.Game.Register<TargetController>(targetController);
            DI.Game.Register<BallSpawner>(ballSpawner);
            DI.Game.Register<GameLostTrigger>(gameLostTrigger);
            
            DI.Game.Register<ScoreMultiplicatorPopup>(multipicatorPopup);
            DI.Game.Register<LeaderboardPopup>(leaderboardPopup);

            var sessionManager = this.GetComponent<SessionManager>();
            sessionManager.Inject(ballSpawner, targetController, gameLostTrigger, multipicatorPopup, leaderboardPopup);
            DI.Game.Register<SessionManager>(sessionManager);

        }

        private void OnDestroy()
        {
            DI.DisposeOfGameContainer();
        }
    }
}