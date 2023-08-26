using MelonLoader;
using Steamworks;

namespace SuzyCube_AchievementFix
{
    public class AchievementFix: MelonMod
    {
        private const string AchievementAllStarsSpecialWorld = "allStarsSpecialWorld";
        
        private const int WorldBonusIndex = 5;
        private const int MaxStarCount = 33; // 11 levels, 3 stars each.
        
        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            // Check for it in the level select scene.
            if (sceneName != "LevelSelect") return;

            // Don't do anything if below the required star count.
            if (LouisGlobalVariables.starsPerWorld[WorldBonusIndex] < MaxStarCount) return;
            
            SteamUserStats.RequestCurrentStats();
            SteamUserStats.GetAchievement(AchievementAllStarsSpecialWorld, out var isAchieved);
            
            // Already unlocked.
            if(isAchieved) return;
            
            // Trigger the achievement.
            SteamUserStats.SetAchievement(AchievementAllStarsSpecialWorld);
            SteamUserStats.StoreStats();
        }
    }
}