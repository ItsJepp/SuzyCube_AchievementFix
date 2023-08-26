using System.Collections.Generic;
using System.Collections.ObjectModel;
using MelonLoader;
using UnityEngine;
using Steamworks;
using UnityEngine.SceneManagement;

namespace SuzyCube_AchievementFix
{
    public class AchievementFix: MelonMod
    {
        private readonly Dictionary<string, bool> _achievements = new Dictionary<string, bool>();

        private void OnUserStatsReceived(UserStatsReceived_t pCallback)
        {
            // Achievements already retrieved.
            if (_achievements.Count > 0) return;
            
            if (pCallback.m_eResult != EResult.k_EResultOK)
            {
                LoggerInstance.Msg("Achievements could not be retrieved.");
                return;
            }
            
            var allAchievements = Achievement.All;
            foreach (var ach in allAchievements)
            {
                SteamUserStats.GetAchievement(ach, out var isUnlocked);
                _achievements[ach] = isUnlocked;
            }
            
            LoggerInstance.Msg("Achievements retrieved!");
        }
        
        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName == "TitleScreen")
            {
                Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
                SteamUserStats.RequestCurrentStats();
                
                MelonEvents.OnGUI.Subscribe(SaveSlotButton, 100);
            }
            else
            {
                // Hide the button.
                MelonEvents.OnGUI.Unsubscribe(SaveSlotButton);

                if (sceneName == "LevelSelect")
                {
                    LoggerInstance.Msg(LouisGlobalVariables.starsPerWorld[5]);
                    if (LouisGlobalVariables.starsPerWorld[5] >= 33)
                        LoggerInstance.Msg("Ye have completied the achievement haha xD");
                    else
                        LoggerInstance.Msg("Not done yet");
                }

                //NoodleJavaScriptRelay.m_Instance.JSRelayUnlockAchievement("allStarsSpecialWorld", null);
            }
        }

        private void SaveSlotButton()
        {
            if (!GUI.Button(new Rect(10, Screen.height - 60, 300, 50), "Create Save From Achievements"))
                return;
            
            var saveSlot = SaveSlotManager.GetFirstAvailableIndex();
            if (saveSlot == -1)
            {
                LoggerInstance.Msg("No save slot available.");
                return;
            }
            
            LoggerInstance.Msg("Using save slot " + saveSlot);
            SaveSlotManager.CreateSaveFromAchievements(saveSlot, _achievements);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public static class Achievement
    {
        public const string CompleteLevel1 = "completeLevel1";

        public const string CompleteWorld1 = "completeWorld1";
        public const string CompleteWorld2 = "completeWorld2";
        public const string CompleteWorld3 = "completeWorld3";
        public const string CompleteWorld4 = "completeWorld4";
        public const string CompleteWorld5 = "completeWorld5";
        
        public const string AllStarsWorld1 = "allStarsWorld1";
        public const string AllStarsWorld2 = "allStarsWorld2";
        public const string AllStarsWorld3 = "allStarsWorld3";
        public const string AllStarsWorld4 = "allStarsWorld4";
        public const string AllStarsWorld5 = "allStarsWorld5";
        
        public const string AllStarsSpecialWorld = "allStarsSpecialWorld";
        
        public const string NineLives = "nineLives";
        public const string OneHundredLives = "oneHundredLives";
        public const string Sprinter = "sprinter";
        public const string MudWrestling = "mudWrestling";
        public const string FourTorches = "fourTorches";

        public static readonly ReadOnlyCollection<string> All = new ReadOnlyCollection<string>(
            new[]
            {
                CompleteLevel1,
                CompleteWorld1, CompleteWorld2, CompleteWorld3, CompleteWorld4, CompleteWorld5,
                AllStarsWorld1, AllStarsWorld2, AllStarsWorld3, AllStarsWorld4, AllStarsWorld5, AllStarsSpecialWorld,
                NineLives, OneHundredLives, Sprinter, MudWrestling, FourTorches
            }
        );
    }
}