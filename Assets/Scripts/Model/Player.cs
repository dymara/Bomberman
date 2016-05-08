using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;


namespace Assets.Scripts.Model
{
    public class Player : AbstractPlayer
    {
        private const String EXIT_TAG = "Exit";

        private long score;

        private Boolean exitReached = false;

        private bool wait;


        public Player(String name, int lives) : base(lives)
        {
            this.name = name;
            this.score = 0;
        }

        public long GetScore()
        {
            return score;
        }

        public void AddToScoreLong(long bonus)
        {
            score += bonus;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!exitReached && hit.gameObject.tag.Equals(EXIT_TAG))
            {
                OnExitReached();
            }
        }


        protected override void Kill()
        {
            FirstPersonController controller = gameObject.GetComponent<FirstPersonController>();
            controller.DisableMoving();
            wait = true;
            StartCoroutine(KillCoroutine());
        }

        protected override void EndGame()
        {
            FirstPersonController controller = gameObject.GetComponent<FirstPersonController>();
            controller.DisableMoving();
            wait = true;
            StartCoroutine(EndGameCoroutine());
        }

        private IEnumerator KillCoroutine()
        {
            if (wait)
            {
                wait = false;
                yield return new WaitForSeconds(1);
            }
                EditorUtility.DisplayDialog("Bomberman3D", "You are dead.", "Repeat level");
                Debug.Log(DateTime.Now + " Repeat level");
                SceneManager.LoadScene("Main");
        }

        private IEnumerator EndGameCoroutine()
        {
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
            exitReached = true;
            Debug.Log(DateTime.Now + " Maze exit reached");
            Boolean answer = EditorUtility.DisplayDialog("Bomberman3D", "Congratulation!. You win.", "Next level", "Exit");
            if (answer)
            {
                Debug.Log(DateTime.Now + " Loading next level");
                SceneManager.LoadScene("Main");
            }
            else {
                Debug.Log("EXIT GAME");
                Application.Quit();
                exitReached = false;
            }
        }
    }
}
