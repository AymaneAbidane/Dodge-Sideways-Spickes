using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private enum Scene { MainMenu, Gameplay }

    [SerializeField] private Scene scene;

    [Header("Main Menu"), ShowIf("scene", Scene.MainMenu)]
    [SerializeField, ShowIf("scene", Scene.MainMenu)] private Button playButton;
    [SerializeField, ShowIf("scene", Scene.MainMenu)] private Button quittButton;

    [Header("GamePlay"), ShowIf("scene", Scene.Gameplay)]
    [SerializeField, ShowIf("scene", Scene.Gameplay), SceneObjectsOnly] private Player player;
    [SerializeField, ShowIf("scene", Scene.Gameplay), SceneObjectsOnly] private Button replayButton;
    [SerializeField, ShowIf("scene", Scene.Gameplay), SceneObjectsOnly] private Button backToMainMenuButton;
    [SerializeField, ShowIf("scene", Scene.Gameplay), SceneObjectsOnly] private RectTransform menuPanel;
    [SerializeField, ShowIf("scene", Scene.Gameplay), SceneObjectsOnly] private TextMeshProUGUI textMenue;

    private bool isPaused;

    private void Awake()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
        Time.timeScale = 1f;
        if (scene == Scene.MainMenu)
        {
            //sub to event will be triggerd if the player touch a spike then he lossed and he need to restart
            HandleMainMenuButtonsUi();
        }
        else if (scene == Scene.Gameplay)
        {
            HandleGamePlayButtonsUi();
        }
        if (player != null)
        {
            player.onPlayerTouchASpickeOrDeathCollider += Player_onPlayerTouchASpickeOrDeathCollider;
        }
    }


    private void OnDestroy()
    {
        if (scene == Scene.MainMenu)
        {
            HandleMainMenuButtonsUi(false);
        }
        else if (scene == Scene.Gameplay)
        {
            HandleGamePlayButtonsUi(false);
        }

        if (player != null)
        {
            player.onPlayerTouchASpickeOrDeathCollider -= Player_onPlayerTouchASpickeOrDeathCollider;
        }
    }

    private void Player_onPlayerTouchASpickeOrDeathCollider(object sender, EventArgs e)
    {
        DisplayMenu(true, "GAME OVER");
    }

    #region GamePlay Scene Ui Manager
    private void HandleGamePlayButtonsUi(bool sub = true)
    {
        if (sub == true)
        {
            AddListnerToButton(replayButton, () => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
            AddListnerToButton(backToMainMenuButton, () => { SceneManager.LoadScene("MainMenuScene"); });
        }
        else
        {
            replayButton.onClick.RemoveAllListeners();
            backToMainMenuButton.onClick.RemoveAllListeners();
        }
    }


    #endregion


    #region MainMenu Scene Ui Manager
    private void HandleMainMenuButtonsUi(bool sub = true)
    {
        if (sub == true)
        {
            AddListnerToButton(playButton, () => { SceneManager.LoadScene("GamePlayScene"); });
            AddListnerToButton(quittButton, () => { Application.Quit(); });
        }
        else
        {
            playButton.onClick.RemoveAllListeners();
            quittButton.onClick.RemoveAllListeners();
        }
    }
    #endregion


    private void AddListnerToButton(Button btn, Action action)
    {
        btn.onClick.AddListener(() =>
        {
            action();
        });
    }
    private void DisplayMenu(bool display, string menuTitle = null)
    {
        menuPanel.gameObject.SetActive(display);
        textMenue.text = menuTitle;
    }
}
