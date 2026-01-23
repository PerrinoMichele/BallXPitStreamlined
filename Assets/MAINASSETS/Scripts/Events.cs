
namespace Core.Events
{
    public class XPCollectEvent : IGameEvent
    {
        public int XP { get; set; }
        
        public XPCollectEvent(int xp)
        {
            XP = xp;
        }
    }
    
    
    public class EnableBoosterPanelEvent : IGameEvent { }

    public class BoosterCollectedEvent : IGameEvent
    {
        public BoosterData BoosterData { get; set; }
        
        public BoosterCollectedEvent(BoosterData boosterData)
        {
            BoosterData = boosterData;
        }
    }
}