using Godot;
using System;

public class Bullet : RigidBody2D
{
    public float MaxDistance = 6000;

    private Vector2 originalPosition;

    public override void _Ready(){
        ContactMonitor = true;
        ContactsReported = 100;
    }
    public override void _PhysicsProcess(float delta){
        float distanceTravelled = this.Position.DistanceTo(this.originalPosition);
        if (distanceTravelled > this.MaxDistance)
            this.QueueFree();
    }

    public void Entered(Node body){
        if(body.Name == "Zombie"){
            Zombie zom = body.GetParent<Zombie>();
            if(!zom.spawning){
                zom.setHealth(zom.getHealth()-1);
            }
            this.QueueFree();
        }
        if(body.Name == "TileMap" || body.Name == "Walls"){
            this.QueueFree();
        }
    }

}