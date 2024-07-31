using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace QuestFinder
{
    [DefOf]
    public static class TreasureDefOf
    {
        public static JobDef EEG_OperateQuest;


        static TreasureDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(TreasureDefOf));
    }

}