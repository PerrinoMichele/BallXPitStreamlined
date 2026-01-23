using Core.Events;
using UnityEngine;

public class XPParticleEffect : MonoBehaviour
{
    public void OnAnimationCompleted()
    {
        EventBus.Publish(new XPCollectEvent(1));
        Destroy(gameObject);

    }
}
