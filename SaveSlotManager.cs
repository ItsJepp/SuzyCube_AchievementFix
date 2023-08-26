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
                if (!SaveSlotInUse(i))
                    return i;
            }

            return -1;
        }
        
        private static bool SaveSlotInUse(int slotIndex)
        {
            var lhs = RuntimeServices.op_Addition("slot", slotIndex);
            return !PlayerPrefsX.GetBool(RuntimeServices.op_Addition(lhs, "hasPlayed"));
        }
    }
}