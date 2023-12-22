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
        [SerializeField]
        private BottomWall bottomWall;

        [SerializeField]
        private TargetDownStepper downStepper;

        private void Awake()
        {
            InstallDependencyInjection();
            // var buildingCursor = DI.Game.Resolve<IBuildingCursor>() as BuildCursorComponent;
        }

        private void InstallDependencyInjection()
        {
            DI.CreateGameContainer();
            DI.Game.Register<ITargetDownMover>(downStepper);
            
            ballSpawner.Inject(downStepper);
            DI.Game.Register<BallSpawner>(ballSpawner);
            
            bottomWall.Inject(downStepper, ballSpawner);
            DI.Game.Register<BottomWall>(bottomWall);

            DI.Game.Register<TargetController>(targetController);
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