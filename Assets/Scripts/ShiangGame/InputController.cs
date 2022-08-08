
using System;
using UnityEngine;

namespace Shiang
{
    #region state manager
    public class InputStateManager : StateManager
    {
        InputController _controller;
        GameModeState _gameMode;
        UiModeState _uiMode;

        public InputStateManager()
            : base()
        {
        }

        public override void InitStates()
        {
            // the casting here is safe
            if (_controller == null)
                _controller = ((InputController)Owner);

            _gameMode = new GameModeState();
            _uiMode = new UiModeState();

            _gameMode.OnStateTick += _controller.GameMode;
            _uiMode.OnStateTick += _controller.UiMode;
        }

        public override void InitTransitions()
        {
            SM.AddTransiton(_gameMode, _uiMode,
                () => _controller.Mode == InputController.InputMode.Ui);
            SM.AddTransiton(_uiMode, _gameMode,
                () => _controller.Mode == InputController.InputMode.Game);
        }

        public override void SetInitialState() => SM.ChangeState(_gameMode);
    }
    #endregion

    [DefaultExecutionOrder(-100)]
    public class InputController : MonoBehaviour, IGameEntity
    {
        public enum InputMode { Game, Ui }

        private InputStateManager _stateMgr;

        public InputMode Mode { get; set; }

        public float ChangeX { get; private set; }

        public float ChangeY { get; private set; }

        public bool Interact { get; private set; }

        public bool Zoom { get; private set; }

        public bool UseWeapon { get; private set; }

        public bool UseAbility { get; private set; }

        public bool Exit { get; private set; }

        public bool OpenStatMenu { get; private set; } // TODO

        public static event Action OnExitFromUIMode;

        private void Awake()
        {
            _stateMgr = Utils.CreateStateManager<InputStateManager, InputController>(this);
            Mode = InputMode.Game;
            UiSceneLoader.OnUISceneLoad += () => Mode = InputMode.Ui;
            UiSceneLoader.OnUISceneUnload += () => Mode = InputMode.Game;
        }

        public void Idle() { }

        public void GameMode()
        {
            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");

            ChangeX = dx != 0f ? Mathf.Sign(dx) : 0f;
            ChangeY = dy != 0f ? Mathf.Sign(dy) : 0f;

            Interact = Input.GetKeyDown(KeyCode.I);
            Zoom = Input.GetKeyDown(KeyCode.Z);

            UseWeapon = Input.GetKeyDown(KeyCode.Space);
            UseAbility = Input.GetKeyDown(KeyCode.LeftControl);

            Exit = Input.GetKeyDown(KeyCode.Escape);
            OpenStatMenu = Input.GetKeyDown(KeyCode.Alpha1);

            if (OpenStatMenu) Mode = InputMode.Ui;

            if (Exit) GameController.QuitGame(); // TODO
        }

        public void UiMode()
        {
            // pause game
            Time.timeScale = 0f;

            Exit = Input.GetKeyDown(KeyCode.Escape);

            if (Exit)
            {
                Mode = InputMode.Game;
                Time.timeScale = 1f;
                OnExitFromUIMode?.Invoke();
            }
        }

        private void Update() => _stateMgr.Tick();
    }
}