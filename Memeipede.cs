using BepInEx;

namespace Memeipede
{
    [BepInPlugin("xerillic.memeipede", "Memeipede", "0.1.0")] // (GUID, mod name, mod version)
    public class Memeipede : BaseUnityPlugin
    {
        public void OnEnable()
        {
            // Hooks
            On.Centipede.Act += Centipede_Act;
            On.Centipede.SpitOutOfShortCut += Centipede_SpitOutOfShortCut;
        }
        private void Centipede_SpitOutOfShortCut(
            On.Centipede.orig_SpitOutOfShortCut orig,
            Centipede self,
            RWCustom.IntVector2 pos,
            Room newRoom,
            bool spitOutAllSticks)
        {
            orig(self, pos, newRoom, spitOutAllSticks);
            PlaySound(behave);
        }
        private void Centipede_Act(On.Centipede.orig_Act orig, Centipede self)
        {
            if (centi != self) { centi = self; }
            orig(self);

            if (centi.Red)
            {
                if (behave != centi.AI.behavior)
                {
                    behave = centi.AI.behavior;

                    for (int i = 0; i < centi.room.game.cameras[0].virtualMicrophone.soundObjects.Count; i++)
                    {
                        if (centi.room.game.cameras[0].virtualMicrophone.soundObjects[i].soundData.soundName.Contains("memeipede"))
                        {
                            centi.room.game.cameras[0].virtualMicrophone.soundObjects[i].Destroy();
                        }
                    }
                    PlaySound(behave);
                }
            }
        }

        private void PlaySound(CentipedeAI.Behavior behavior)
        {
            // Might add more
            if (behavior == CentipedeAI.Behavior.Hunt)
            {
                centi.room.PlayCustomChunkSound("memeipede_hunt", centi.mainBodyChunk, 1f, 1f);
            }
        }

        public CentipedeAI.Behavior behave;
        public Centipede centi;
    }
}