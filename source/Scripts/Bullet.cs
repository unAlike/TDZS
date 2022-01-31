using Godot;
using System;

public class Bullet : RigidBody2D
{
    public float MaxDistance = 6000;

    private Vector2 originalPosition;


    public override void _PhysicsProcess(float delta){
        float distanceTravelled = this.Position.DistanceTo(this.originalPosition);
        if (distanceTravelled > this.MaxDistance)
            this.QueueFree();
    }
}