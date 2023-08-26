using System.Collections.Generic;
using System.Collections.ObjectModel;
using MelonLoader;
using UnityEngine;
using Boo.Lang.Runtime;
using Steamworks;

namespace SuzyCube_AchievementFix
{
    public class AchievementFix: MelonMod
    {
        private Dictionary<string, bool> _achievements = new Dictionary<string, bool>();

        public override void OnInitializeMelon()
        {
            Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        }

        private void OnUserStatsReceived(UserStatsReceived_t pCallback)
        {
            if (pCallback.m_eResult != EResult.k_EResultOK)
            {
                LoggerInstance.Msg("Achievements could not be retrieved.");
                return;
            }

            if (_achievements.Count > 0)
            {
                LoggerInstance.Msg("Achievements have already been retrieved.");
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