using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace QuestFinder
{
    [StaticConstructorOnStartup]
    public static class QuestFinderUtility
    {
        private static readonly List<QuestScriptDef> possibleQuests;
        private static readonly Dictionary<QuestScriptDef, QuestInformation> extensions;
        public static Harmony Harm = new Harmony("eeg.treasurehunter");

        static QuestFinderUtility()
        {
            QuestFinderUtility.possibleQuests = DefDatabase<QuestScriptDef>.AllDefs.Where<QuestScriptDef>((Func<QuestScriptDef, bool>)(def => def.GetModExtension<QuestInformation>() != null)).ToList<QuestScriptDef>();
            QuestFinderUtility.extensions = QuestFinderUtility.possibleQuests.ToDictionary<QuestScriptDef, QuestScriptDef, QuestInformation>((Func<QuestScriptDef, QuestScriptDef>)(q => q), (Func<QuestScriptDef, QuestInformation>)(q => q.GetModExtension<QuestInformation>()));
            QuestFinderUtility.Harm.Patch((MethodBase)AccessTools.Method(typeof(Quest), "End"), postfix: new HarmonyMethod(typeof(QuestFinderUtility), "Quest_End_Postfix"));
        }

        public static IEnumerable<QuestScriptDef> PossibleQuests => (IEnumerable<QuestScriptDef>)QuestFinderUtility.possibleQuests;

        public static QuestInformation FinderInfo(this QuestScriptDef quest) => QuestFinderUtility.extensions[quest];

        public static string Label(this QuestScriptDef quest) => QuestFinderUtility.extensions[quest].label;

        public static string LabelCap(this QuestScriptDef quest) => QuestFinderUtility.extensions[quest].label.CapitalizeFirst();

        public static void Quest_End_Postfix(Quest __instance, QuestEndOutcome outcome) => GameComponent_QuestFinder.Instance.Notify_QuestComplete(__instance, outcome);
    }
}