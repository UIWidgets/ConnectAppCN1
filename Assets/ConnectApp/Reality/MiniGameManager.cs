using System.Collections.Generic;
using ConnectApp.Reality;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour {
    [Header("Game Status")] public bool duringGame = false;

    public bool isPause = false;
    public bool isOver = false;

    public int score;

    [Header("Game Settings")] public float timerCountdown;

    public float timePerRound = 60f;

    [Header("Object Settings")] public Text timerText;

    public Text scoreText;

    public GameObject startText;
    public GameObject pauseIcon;
    public GameObject replayIcon;
    public GameObject overlayCanvas;

    public UnityChanController unityChan;

    [Header("Camera Settings")] public Camera mainCamera;

    public Transform defaultCameraAnchor;
    public Transform miniGameCameraAnchor;
    public float defalutCameraFOV = 90f;
    public float miniGameCameraFOV = 60f;
    public float duration = 2f;
    float timer = 0f;
    bool towardDefault = true;

    [Header("Coin Spawn Settings")] public float timePerCoin = 10f;

    public GameObject coinPrefab;
    public List<Transform> coinSpawnPoints = new List<Transform>();
    Transform lastCoinSpawnPoint;

    int m_DelayFrames = -1;

    public void TriggerSwitchCamera(int? delayFrames = null) {
        if (delayFrames != null) {
            this.m_DelayFrames = (int) delayFrames;
            return;
        }

        this.towardDefault = !this.towardDefault;
        if (this.towardDefault) {
            this.mainCamera.transform.parent = this.defaultCameraAnchor;
        }
        else {
            RealityManager.instance.useGyro = false;
            this.mainCamera.transform.parent = this.miniGameCameraAnchor;
            this.duringGame = true;
        }
    }

    public void AddTime() {
        this.timerCountdown += this.timePerCoin;
        this.score++;

        int randomIndex = 0;
        do {
            randomIndex = Random.Range(0, this.coinSpawnPoints.Count);
        } while (this.coinSpawnPoints[randomIndex] == this.lastCoinSpawnPoint);

        this.lastCoinSpawnPoint = this.coinSpawnPoints[randomIndex];
        Instantiate(this.coinPrefab, this.lastCoinSpawnPoint.position, Quaternion.identity, this.lastCoinSpawnPoint);
    }

    public void InitGame() {
        if (!this.isOver) {
            return;
        }

        Debuger.Log("Init Game");
        this.startText.SetActive(true);
        this.pauseIcon.SetActive(false);
        this.replayIcon.SetActive(false);
        this.scoreText.gameObject.SetActive(false);
        this.overlayCanvas.SetActive(false);

        this.isOver = true;
        this.isPause = true;
    }

    public void StartGame() {
        if (!this.isOver) {
            if (this.isPause) {
                this.ResumeGame();
            }

            return;
        }

        Debuger.Log("Start Game");

        this.overlayCanvas.SetActive(true);
        this.startText.SetActive(false);
        this.pauseIcon.SetActive(false);
        this.replayIcon.SetActive(false);
        this.scoreText.gameObject.SetActive(true);

        this.timerCountdown = this.timePerRound;
        this.score = 0;
        this.isOver = false;
        this.isPause = false;
        this.unityChan.enableToMove = true;

        this.TriggerSwitchCamera();
        return;
    }

    public void PauseGame() {
        Debuger.Log("Pause Game");
        this.overlayCanvas.SetActive(false);
        this.pauseIcon.SetActive(true);

        this.isPause = true;
        this.unityChan.enableToMove = false;
        this.scoreText.text = $"TIME : {(int) this.timerCountdown}\nSCORE : {this.score}";

        this.unityChan.isMoving = false;

        RealityManager.instance.viewer.ResetGyro();

        this.TriggerSwitchCamera(10);
    }

    public void ResumeGame() {
        Debuger.Log("Resume Game");

        this.overlayCanvas.SetActive(true);
        this.pauseIcon.SetActive(false);

        this.isPause = false;
        this.unityChan.enableToMove = true;

        this.TriggerSwitchCamera();
    }

    public void OverGame() {
        Debuger.Log("Game Over");
        this.timerCountdown = 0;
        this.isPause = true;
        this.isOver = true;
        this.scoreText.text = $"FINAL SCORE\n{this.score}";
        this.replayIcon.SetActive(true);
        this.unityChan.enableToMove = false;
        this.unityChan.isMoving = false;
        this.overlayCanvas.SetActive(false);

        RealityManager.instance.viewer.ResetGyro();

        this.TriggerSwitchCamera(10);
    }

    void ProcessCameraSwitch() {
        var ratio = this.timer / this.duration;
        this.mainCamera.transform.position = (1 - ratio) * this.defaultCameraAnchor.position +
                                             ratio * this.miniGameCameraAnchor.position;
        this.mainCamera.transform.rotation = Quaternion.Lerp(this.defaultCameraAnchor.rotation,
            this.miniGameCameraAnchor.rotation, ratio);
        this.mainCamera.fieldOfView = (1 - ratio) * this.defalutCameraFOV + ratio * this.miniGameCameraFOV;
    }

    void Update() {
        if (this.m_DelayFrames >= 0) {
            this.m_DelayFrames--;
        }

        if (this.m_DelayFrames == 0) {
            this.TriggerSwitchCamera();
        }

        var timeDelta = Time.deltaTime;
        // Camera Switch
        if (this.towardDefault) {
            if (this.timer > 0) {
                this.timer -= timeDelta;
                if (this.timer <= 0) {
                    this.timer = 0;
                    this.duringGame = false;
                    RealityManager.instance.useGyro = true;
                }

                this.ProcessCameraSwitch();
            }
        }

        if (!this.towardDefault) {
            if (this.timer < this.duration) {
                this.timer += timeDelta;
                if (this.timer >= this.duration) {
                    this.timer = this.duration;
                }

                this.ProcessCameraSwitch();
            }
        }

        if (this.isOver) {
            return;
        }

        if (!this.isPause) {
            this.timerCountdown -= timeDelta;
            this.timerText.text = $"Time : {(int) this.timerCountdown}";
            this.scoreText.text = $"SCORE\n{this.score}";
        }

        if (this.timerCountdown <= 0) {
            this.OverGame();
        }
    }
}