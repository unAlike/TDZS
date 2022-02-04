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
        GD.Print("Hit");
        GD.Print(body);
        if(body.Name == "Zombie"){
            Zombie zom = body.GetParent<Zombie>();
            zom.setHealth(zom.getHealth()-1);
            body.GetNode<TextureProgress>("HealthBar");
            this.QueueFree();
        }
        if(body.Name == "TileMap"){
            this.QueueFree();
        }
    }

}