using System.Collections;
using Myth.Core;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Myth.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("UI Documents")]
        [SerializeField] private UIDocument mainMenu;
        [SerializeField] private UIDocument optionsMenu;
        
        [Header("Audio")]
        [SerializeField] private RandomizeAudio hoverAudio;
        [SerializeField] private RandomizeAudio selectAudio;
        [SerializeField] private AudioSource startAudio;
        [SerializeField] private AudioSource exitAudio;

        private VisualElement _rootMain;
        private VisualElement _rootOptions;

        private float _quitDelay = 0f;
        private float _startDelay = 0f;
        [UxmlAttribute, CreateProperty] private string _currentVersion;

        private void OnEnable()
        {
            _quitDelay = exitAudio.clip.length;
            _startDelay = startAudio.clip.length;
            
            // Set Root Visual Elements
            _rootMain = mainMenu.rootVisualElement;
            _rootOptions = optionsMenu.rootVisualElement;
            _rootOptions.style.display = DisplayStyle.None;
            
            // Set Version Text
            _currentVersion = $"Version: {Application.version}";
            _rootMain.Q<Label>("Version").text = _currentVersion;
            
#region MAIN MENU SCREEN
            Button newWorldButton = _rootMain.Q<Button>("NewWorld");
            newWorldButton.clicked += OnNewWorldButtonClicked;
            newWorldButton.RegisterCallback<PointerEnterEvent>(OnPointerEnterEvent, TrickleDown.TrickleDown);
            
            Button loadWorldButton = _rootMain.Q<Button>("LoadWorld");
            loadWorldButton.clicked += OnLoadWorldButtonClicked;
            loadWorldButton.RegisterCallback<PointerEnterEvent>(OnPointerEnterEvent, TrickleDown.TrickleDown);
            
            Button multiplayerButton = _rootMain.Q<Button>("Multiplayer");
            //multiplayerButton.clicked += OnMultiplayerbuttonClicked;
            //multiplayerButton.RegisterCallback<PointerEnterEvent>(OnPointerEnterEvent, TrickleDown.TrickleDown);
            
            Button optionsButton = _rootMain.Q<Button>("Options");
            optionsButton.clicked += OnSettingsButtonClicked;
            optionsButton.RegisterCallback<PointerEnterEvent>(OnPointerEnterEvent, TrickleDown.TrickleDown);
            
            Button exitButton = _rootMain.Q<Button>("Exit");
            exitButton.clicked += OnExitButtonClicked;
            exitButton.RegisterCallback<PointerEnterEvent>(OnPointerEnterEvent, TrickleDown.TrickleDown);
#endregion
            
#region OPTIONS SCREEN
            Button optionsBackButton = _rootOptions.Q<Button>("Back");
            optionsBackButton.clicked += OnOptionsBackButtonClicked;
            optionsBackButton.RegisterCallback<PointerEnterEvent>(OnPointerEnterEvent, TrickleDown.TrickleDown);
            
            Button optionsGeneralButton = _rootOptions.Q<Button>("GeneralOptions");
            optionsGeneralButton.clicked += OnOptionsGeneralButtonClicked;
            optionsGeneralButton.RegisterCallback<PointerEnterEvent>(OnPointerEnterEvent, TrickleDown.TrickleDown);
            
            Button optionsGraphicsButton = _rootOptions.Q<Button>("GraphicOptions");
            optionsGraphicsButton.clicked += OnOptionsGraphicsButtonClicked;
            optionsGraphicsButton.RegisterCallback<PointerEnterEvent>(OnPointerEnterEvent, TrickleDown.TrickleDown);

            Button optionsAudioButton = _rootOptions.Q<Button>("AudioOptions");
            optionsAudioButton.clicked += OnOptionsAudioButtonClicked;
            optionsAudioButton.RegisterCallback<PointerEnterEvent>(OnPointerEnterEvent, TrickleDown.TrickleDown);
            
            Button optionsControlsButton = _rootOptions.Q<Button>("ControlOptions");
            optionsControlsButton.clicked += OnOptionsControlButtonClicked;
            optionsControlsButton.RegisterCallback<PointerEnterEvent>(OnPointerEnterEvent, TrickleDown.TrickleDown);
            
            Button optionsLanguageButton = _rootOptions.Q<Button>("LanguageOptions");
            //optionsLanguageButton.clicked += OnOptionsLanguageButtonClicked;
            //optionsLanguageButton.RegisterCallback<PointerEnterEvent>(OnPointerEnterEvent, TrickleDown.TrickleDown);
#endregion
        }

#region MAIN MENU FUNCTIONS
        private void OnNewWorldButtonClicked()
        {
            StartCoroutine(StartApplication());
        }

        private void OnLoadWorldButtonClicked()
        {
            selectAudio.PlayRandomAudio();
        }

        private void OnMultiplayerbuttonClicked()
        {
            selectAudio.PlayRandomAudio();
        }

        private void OnSettingsButtonClicked()
        {
            selectAudio.PlayRandomAudio();
            DisableAllScreens();
            _rootOptions.style.display = DisplayStyle.Flex;
        }

        private void OnExitButtonClicked()
        {
            exitAudio.Play();
            StartCoroutine(QuitApplication());
        }
#endregion
        
#region OPTIONS FUNCTIONS

        private void OnOptionsBackButtonClicked()
        {
            selectAudio.PlayRandomAudio();
            DisableAllScreens();
            _rootMain.style.display = DisplayStyle.Flex;
        }

        private void OnOptionsGeneralButtonClicked()
        {
            selectAudio.PlayRandomAudio();
        }
        
        private void OnOptionsGraphicsButtonClicked()
        {
            selectAudio.PlayRandomAudio();
        }
        
        private void OnOptionsAudioButtonClicked()
        {
            selectAudio.PlayRandomAudio();
        }
        
        private void OnOptionsControlButtonClicked()
        {
            selectAudio.PlayRandomAudio();
        }

        private void OnOptionsLanguageButtonClicked()
        {
            selectAudio.PlayRandomAudio();
        }
#endregion
        
#region HELPER FUNCTIONS
        private void DisableAllScreens()
        {
            _rootMain.style.display = DisplayStyle.None;
            _rootOptions.style.display = DisplayStyle.None;
        }

        private void OnPointerEnterEvent(PointerEnterEvent enterEvent)
        {
            hoverAudio.PlayRandomAudio();
        }

        IEnumerator QuitApplication()
        {
            if (_quitDelay > 0f)
                yield return new WaitForSeconds(_quitDelay);
            yield return new WaitForEndOfFrame();
            
            Application.Quit();
            
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #endif
        }
        
        IEnumerator StartApplication()
        {
            if (_startDelay > 0f)
                yield return new WaitForSeconds(_startDelay);
            yield return new WaitForEndOfFrame();

            SceneManager.LoadScene(1, LoadSceneMode.Single);
            
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
#endregion
    }
}