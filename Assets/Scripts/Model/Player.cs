using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.Model
{
    public class Player : AbstractPlayer
    {
        public int bombs { get; set; }

        public int bombRange { get; set; }

        public float speed { get; set; }

        public bool remoteDetonationBonus { get; set; }

        public long score { get; set; }

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
