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
            set { _bombs = value; GameManager.instance.OnPlayerBombsChanged(value); }
        }

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

        private bool wait;

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
            if (!exitReached && hit.gameObject.tag.Equals(Constants.EXIT_TAG))
            {
                OnExitReached();
            }
        }

        protected override void Kill()
        {
            controller.DisableMoving();
            wait = true;
            StartCoroutine(KillCoroutine());
        }

        protected override void EndGame()
        {
            controller.DisableMoving();
            wait = true;
            StartCoroutine(EndGameCoroutine());
        }

        private IEnumerator KillCoroutine()
        {
            // TODO - change to proper implementation
            if (wait)
            {
                wait = false;
                yield return new WaitForSeconds(1);
            }
            EditorUtility.DisplayDialog("Bomberman3D", "You are dead.", "Repeat level");
            Debug.Log(DateTime.Now + " Repeat level");
            controller.EnableMoving();
            GameManager.instance.SwitchGameState(GameState.GAMEPLAY);
        }

        private IEnumerator EndGameCoroutine()
        {
            // TODO - change to proper implementation
            if (wait)
            {
                wait = false;
                yield return new WaitForSeconds(1);
            }
            EditorUtility.DisplayDialog("Bomberman3D", ":(:(:( You lose.", "Exit to main menu");
            Debug.Log(DateTime.Now + " Exit to main menu");
            Application.Quit();
        }

        private void OnExitReached()
        {
            // TODO - change to proper implementation
            exitReached = true;
            Debug.Log(DateTime.Now + " Maze exit reached");
            Boolean answer = EditorUtility.DisplayDialog("Bomberman3D", "Congratulations! You win!", "Next level", "Exit");
            if (answer)
            {
                Debug.Log(DateTime.Now + " Loading next level");
                GameManager.instance.SwitchGameState(GameState.GAMEPLAY);
            }
            else {
                Debug.Log("EXIT GAME");
                Application.Quit();
                exitReached = false;
            }
        }
    }
}
