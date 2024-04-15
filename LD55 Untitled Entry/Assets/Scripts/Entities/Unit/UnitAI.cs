using UnityEngine;

public class UnitAI : EntityAI
{
    protected override void Start()
    {
        base.Start();
        _nearbyEntity.Add(this.rb2D);
    }
}