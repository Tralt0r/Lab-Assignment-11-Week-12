using System.Collections.Generic;
using UnityEngine;

//Info that gets sent when a target when hit
public struct TargetHitEvent
{
    public int points; 
    public Vector3 position;
}

//Hit information
public interface IHitObserver
{
    void OnTargetHit(TargetHitEvent hitEvent);
}

//Keeps a list of listeners and tells them when a hit happens
public class HitSubject
{
    private readonly List<IHitObserver> observers = new List<IHitObserver>();

    public void Subscribe(IHitObserver observer) { if (!observers.Contains(observer)) observers.Add(observer); }

    public void Unsubscribe(IHitObserver observer) { observers.Remove(observer); }

    public void Notify(TargetHitEvent hitEvent)
    {
        for (int i = observers.Count - 1; i >= 0; i--) observers[i].OnTargetHit(hitEvent);
    }
}

//Event shared by all
public static class GameEvents
{
    public static HitSubject Hits = new HitSubject();
    public static void Reset() { Hits = new HitSubject(); }
}