using Barotrauma;
using HarmonyLib;
using System.Runtime.CompilerServices;

[assembly: IgnoresAccessChecksTo("Barotrauma")]
[assembly: IgnoresAccessChecksTo("DedicatedServer")]
[assembly: IgnoresAccessChecksTo("BarotraumaCore")]

namespace PassiveCreatures
{
    public partial class Plugin : IAssemblyPlugin
    {
        readonly Harmony harmony = new Harmony("plag.barotrauma.passivecreatures");

        public void Initialize()
        {
            harmony.PatchAll();
        }

        public void OnLoadCompleted()
        {
        }

        public void PreInitPatching()
        {
        }

        public void Dispose()
        {
            harmony.UnpatchSelf();
        }

        [HarmonyPatch(typeof(EnemyAIController), nameof(EnemyAIController.GetTargetingTags))]
        class EnemyAIController_GetTargetingTags_Patch
        {
            static void Postfix(EnemyAIController __instance, AITarget aiTarget, ref IEnumerable<Identifier> __result)
            {
                // Human enemies can still attack the main sub.
                if (__instance.Character.IsHuman)
                {
                    return;
                }

                Submarine protectedSub = Submarine.MainSub;
                if (protectedSub == null)
                {
                    return;
                }

                if (aiTarget?.Entity == null)
                {
                    return;
                }

                if (aiTarget.Entity.Submarine == protectedSub)
                {
                    __result = Enumerable.Empty<Identifier>();
                }
            }
        }
    }
}
