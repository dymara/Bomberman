using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.Model
{
    public class Player : AbstractPlayer
    {
        private int _bombs;


        public int bombs {
            get { return _bombs; }
            set
            {
                if (value <= Constants.MAX_BOMBS_COUNT) { 
                    _bombs = value;
                    GameManager.instance.OnPlayerBombsChanged(value);
            }
        }
        }

        public int maximumBombsCount { get; set; }

        private int _bombRange;
        public int bombRange {
            get { return _bombRange; }
            set { _bombRange = value;  GameManager.instance.OnPlayerBombRangeChanged(value); }
        }

        private float _speed;
        public float speed {
            get { return _speed; }
            set { _speed = value; GameManager.instance.OnPlayerSpeedChanged(value); }
        }

        private bool _remoteDetonationBonus;
        public bool remoteDetonationBonus {
            get { return _remoteDetonationBonus; }
            set { _remoteDetonationBonus = value; GameManager.instance.OnPlayerRemoteDetonationBonusChanged(value); }
        }

        private long _score;
        public long score {
            get { return _score; }
            set { _score = value; GameManager.instance.OnPlayerScoreChanged(value); }
        }

        private bool exitReached = false;

        private bool wait = false;

        private FirstPersonController controller;

        public Player(int lives) : base(lives)
        {

        }

        void Awake()
        {
            this.controller = gameObject.GetComponent<FirstPersonController>();
            DontDestroyOnLoad(gameObject);  // Sets this to not be destroyed when reloading scene
        }

        /***********************************************************************/

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.tag.Equals(Constants.EXIT_TAG))
            {
                if (!exitReached)
                {
                    StartCoroutine(OnExitReached());
                }
            }
            else if (hit.gameObject.tag.Equals(Constants.FINDING_TAG))
            {
                hit.gameObject.GetComponent<Finding>().PickUp(this);
            }
        }

        protected override void OnLivesChanged(int newValue)
        {
            GameManager.instance.OnPlayerLivesChanged(newValue);
        }

        public void TriggerKill()
        {
            remainingLives--;
            wait = false;
            StartCoroutine(KillCoroutine());
        }

        protected override void Kill()
        {
            wait = true;
            Debug.Log(DateTime.Now + " Player has blown himself up!");
            StartCoroutine(KillCoroutine());
        }

        private IEnumerator KillCoroutine()
        {
            controller.DisableMoving();
            if (wait)
            {
                yield return new WaitForSeconds(1);
            }

            if (remainingLives > 0)
            {
                EditorUtility.DisplayDialog("Bomberman 3D", "Unfortunately, you are dead...", "Repeat level");
                controller.EnableMoving();
                GameManager.instance.RestartLevel();
            }
            else
            {
                EditorUtility.DisplayDialog("Bomberman 3D", "GAME OVER!", "Return to main menu");
                GameManager.instance.SwitchGameState(GameState.MAIN_MENU);
                controller.EnableMoving();
            }
        }

        private IEnumerator OnExitReached()
        {
            exitReached = true;
            LevelManager levelManager = GameObject.Find(Constants.LEVEL_MANAGER_NAME).GetComponent<LevelManager>();
            GameManager.instance.OnLevelCleared(levelManager.GetCountdownValue());

            int currentLevel = GameManager.instance.GetCurrentLevelNumber();
            Debug.Log(DateTime.Now + " Player has reached level " + currentLevel + " maze exit!");

            bool answer = EditorUtility.DisplayDialog("Bomberman 3D", "Congratulations! Level " + currentLevel  + " cleared!", "Go to the next level", "Return to main menu");
            if (answer) {
                GameManager.instance.AdvanceToNextLevel();
            }
            else
            {
                GameManager.instance.SwitchGameState(GameState.MAIN_MENU);
            }
            yield return new WaitForSeconds(3); // [dymara] Hack for disabling exit events untill scene fade out animation finishes.
            exitReached = false;
        }
    }
}
