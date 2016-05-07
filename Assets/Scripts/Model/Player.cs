using Assets.Scripts.Board;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;


namespace Assets.Scripts.Model
{
    public class Player : AbstractPlayer
    {
        private long score;

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
    }
}
