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
        }

        private void InstallDependencyInjection()
        {
            DI.CreateGameContainer();
            
            DI.Game.Register<ILeaderboardStorage, LeaderboardStorage>();
            
            DI.Game.Register<ITargetDownMover>(downStepper);
            
            ballSpawner.Inject(downStepper);
            DI.Game.Register<IInputController>(ballSpawner);
            
            bottomWall.Inject(downStepper, ballSpawner);
            DI.Game.Register<BottomWall>(bottomWall);

            DI.Game.Register<TargetController>(targetController);
            DI.Game.Register<GameLostTrigger>(gameLostTrigger);

            DI.Game.Register<ScoreMultiplicatorPopup>(multipicatorPopup);
            var leaderboardStorage = DI.Game.Resolve<ILeaderboardStorage>();
            leaderboardPopup.Inject(leaderboardStorage);
            DI.Game.Register<LeaderboardPopup>(leaderboardPopup);
            
            DI.Game.Register<SessionEndSequencer>();

            var sessionManager = this.GetComponent<SessionManager>();
            var sequencer = DI.Game.Resolve<SessionEndSequencer>();
            sessionManager.Inject(targetController, gameLostTrigger, sequencer);
            DI.Game.Register<SessionManager>(sessionManager);
        }

        private void OnDestroy()
        {
            DI.DisposeOfGameContainer();
        }
    }
}