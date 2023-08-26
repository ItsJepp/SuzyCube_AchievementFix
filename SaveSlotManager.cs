using System.Collections.Generic;
using Boo.Lang.Runtime;
using MelonLoader;

namespace SuzyCube_AchievementFix
{
    public static class SaveSlotManager
    {
        public static int GetFirstAvailableIndex()
        {
            for (var i = 0; i < 3; i++)
            {
                if (!PlayerPrefsX.GetBool($"slot{i}hasPlayed"))
                    return i;
            }

            return -1;
        }
        
        public static void CreateSaveFromAchievements(int saveSlot, Dictionary<string, bool> achievements)
        {
            LouisSaveSlotData.saveSlot = saveSlot;
            
            LouisSaveSlotData.SetBool("hasPlayed", true);

            // Regular levels.
            for (var world = 1; world <= 5; world++)
            {
                var worldCompleted = achievements[$"completeWorld{world}"];
                var worldStarred = achievements[$"allStarsWorld{world}"];
                
                for (var level = 1; level <= 6; level++)
                {
                    var levelDisplay = level.ToString();
                    switch (level)
                    {
                        case 5:
                            levelDisplay = "B";
                            break;
                        case 6:
                            levelDisplay = "S";
                            break;
                    }

                    MarkLevelComplete($"level{world}-{levelDisplay}", worldCompleted, worldStarred);
                }
            }
            
            // Special levels.
            for (var level = 1; level <= 11; level++)
            {
                MarkLevelComplete(
                    $"levelS-{level}",
                    achievements["allStarsWorld5"],
                    level != 11
                );
            }
        }

        private static void MarkLevelComplete(string levelPrefix, bool worldCompleted, bool worldStarred)
        {
            if (!worldCompleted) return;
            LouisSaveSlotData.SetBool($"{levelPrefix}isLocked", false);
            LouisSaveSlotData.SetBool($"{levelPrefix}isCompleted", true);
            
            if (!worldStarred) return;
            LouisSaveSlotData.SetIntArray($"{levelPrefix}stars", new []{4, 4, 4});
        }
    }
}