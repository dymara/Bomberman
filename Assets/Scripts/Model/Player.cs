using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Model
{
    public class Player : AbstractPlayer
    {
        private long score;

        private Boolean exitReached = false;

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

            Exit exit = hit.gameObject.GetComponent<Exit>();
            if (!exitReached && exit != null)
            {
                OnExitReached();
            }
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
