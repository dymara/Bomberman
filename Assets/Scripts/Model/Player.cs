using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Position;

namespace Assets.Scripts.Model
{
    public class Player : AbstractPlayer
    {
        private GameObject playerAvatar;

        public CameraPositionListener cameraPositionListener { set; get; }

        private int _bombs;
        public int bombs {
            get { return _bombs; }
            set
            {
                    _bombs = value;
                    GameManager.instance.OnPlayerBombsChanged(value);
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

        public Player(int lives) : base(lives) { }

        void Awake()
        {
            this.playerAvatar = GameObject.Find("PlayerAvatar");
            DontDestroyOnLoad(gameObject);  // Sets this to not be destroyed when reloading scene
        }

        public void PrepareForNextLevel()
        {
            StartCoroutine(PrepareForNextLevelCoroutine());
        }

        private IEnumerator PrepareForNextLevelCoroutine()
        {
            yield return new WaitForSeconds(3); // [dymara] Hack for disabling exit events untill scene fade out animation finishes
            this.exitReached = false;
        }

        /***********************************************************************/

        void OnControllerColliderHit(ControllerColliderHit hit)
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
                hit.gameObject.GetComponent<AbstractFinding>().PickUp(this);
            }
        }

        protected override void OnLivesChanged(int newValue)
        {
            GameManager.instance.OnPlayerLivesChanged(newValue);
        }

        public override void OnExplode()
        {
            if (GameManager.instance.CanPlayerBeKilled())
            {
                base.OnExplode();
            }
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
            GameManager.instance.GetFPSController().DisableMoving();
            if (wait)
            {
                yield return new WaitForSeconds(1);
            }

            GameManager.instance.EndCurrentLevel(false);
        }

        private IEnumerator OnExitReached()
        {
            exitReached = true;
            LevelManager levelManager = GameObject.Find(Constants.LEVEL_MANAGER_NAME).GetComponent<LevelManager>();
            GameManager.instance.OnLevelCleared(levelManager.GetCountdownValue());

            int currentLevel = GameManager.instance.GetCurrentLevelNumber();
            Debug.Log(DateTime.Now + " Player has reached level " + currentLevel + " maze exit!");

            GameManager.instance.GetFPSController().DisableMoving();
            GameManager.instance.EndCurrentLevel(true);

            yield return null;
        }

        new void Update()
        {
            Quaternion avatarRotation = playerAvatar.transform.rotation;
            avatarRotation.y = (transform.rotation.y + 180) % 360;
            playerAvatar.transform.rotation = avatarRotation;

            if (cameraPositionListener != null)
            {
                cameraPositionListener.OnPostionChanged(transform.position);
            }

            base.Update();
        }
    }
}
