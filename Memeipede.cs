using BepInEx;

namespace Memeipede
{
    [BepInPlugin("xerillic.memeipede", "Memeipede", "0.1.0")] // (GUID, mod name, mod version)
    public class Memeipede : BaseUnityPlugin
    {
        public void OnEnable()
        {
            On.Centipede.Act += Centipede_Act;
            On.Centipede.SpitOutOfShortCut += Centipede_SpitOutOfShortCut;
        }
        private void Centipede_SpitOutOfShortCut(On.Centipede.orig_SpitOutOfShortCut orig, Centipede self, RWCustom.IntVector2 pos, Room newRoom, bool spitOutAllSticks)
        {
            orig(self, pos, newRoom, spitOutAllSticks);
            if (behave == CentipedeAI.Behavior.Hunt)
            {
                self.room.PlayCustomChunkSound("memeipede", self.mainBodyChunk, 1f, 1f);
            }
        }
        private void Centipede_Act(On.Centipede.orig_Act orig, Centipede self)
        {
            orig(self);
            if (behave != self.AI.behavior)
            {
                behave = self.AI.behavior;
                if (behave == CentipedeAI.Behavior.Hunt) 
                {
                    self.room.PlayCustomSound("memeipede", self.mainBodyChunk.pos, 1f, 1f);
                }
                else
                {
                    for (int i = 0; i < self.room.game.cameras[0].virtualMicrophone.soundObjects.Count; i++)
                    {
                        if (self.room.game.cameras[0].virtualMicrophone.soundObjects[i].soundData.soundName == "memeipede")
                        {
                            self.room.game.cameras[0].virtualMicrophone.soundObjects[i].Destroy();
                        }
                    }
                }
            }
        }

        public CentipedeAI.Behavior behave;
    }
}