
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
}