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
        private ScoreMultiplierPopup multipicatorPopup;
        [SerializeField]
        private LeaderboardPopup leaderboardPopup;
        [SerializeField]
        private BottomWall bottomWall;

        [SerializeField]
        private TargetDownStepper downStepper;

        private void Awake()
        {
            InstallDependencyInjection();
        }

        private void OnDestroy()
        {
            DI.DisposeOfGameContainer();
        }

        private void InstallDependencyInjection()
        {
            DI.CreateGameContainer();
            
            DI.Game.Register<IPlayerInfoProvider, PlayerInfo>();
            
            InstallLeaderboard();
            
            InstallGameSessionFieldEntities();

            InstallPopups();

            InstallSessionFlow();
        }

        private static void InstallLeaderboard()
        {
            DI.Game.Register<ILeaderboardStorage, LeaderboardStorage>();
            DI.Game.Register<ILeaderboardController, LeaderboardController>();
        }

        private void InstallGameSessionFieldEntities()
        {
            DI.Game.Register<ITargetDownMover>(this.downStepper);
            
            this.ballSpawner.Inject(this.downStepper);
            DI.Game.Register<IInputController>(this.ballSpawner);
            
            this.bottomWall.Inject(this.downStepper, this.ballSpawner);
            DI.Game.Register<BottomWall>(this.bottomWall);

            DI.Game.Register<TargetController>(this.targetController);
            DI.Game.Register<GameLostTrigger>(this.gameLostTrigger);
        }

        private void InstallPopups()
        {
            DI.Game.Register<ScoreMultiplierPopup>(this.multipicatorPopup);
            var leaderboardController = DI.Game.Resolve<ILeaderboardController>();
            var playerInfo = DI.Game.Resolve<IPlayerInfoProvider>();
            this.leaderboardPopup.Inject(leaderboardController, playerInfo);
            DI.Game.Register<LeaderboardPopup>(this.leaderboardPopup);
        }

        private void InstallSessionFlow()
        {
            DI.Game.Register<SessionEndSequencer>();

            var sessionManager = this.GetComponent<SessionManager>();
            var sequencer = DI.Game.Resolve<SessionEndSequencer>();
            sessionManager.Inject(this.targetController, this.gameLostTrigger, sequencer);
            DI.Game.Register<SessionManager>(sessionManager);
        }
    }
}