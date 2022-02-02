using Godot;
using System;

public class Zombie : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    int health = 3;
    public Area2D target;
    public Vector2 velocity = new Vector2();
    TextureProgress healthProg;

    public override void _Ready()
    {
        healthProg = GetNode<TextureProgress>("HealthBar");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta){
        if(target!=null){
            KinematicBody2D zom = GetNode<KinematicBody2D>("Zombie");
            zom.LookAt(target.GlobalPosition);
            velocity = zom.GlobalPosition.DirectionTo(target.GlobalPosition) * 100;
            velocity = zom.MoveAndSlide(velocity);
            GD.Print("Player pos: " + target.Position + " \nZombie pos: " + zom.GlobalPosition);
            healthProg.SetPosition(zom.Position + new Vector2(-60,-60));
        }
        
    }
    public void setHealth(int nhealth){
        
        if(nhealth <= 0){
            QueueFree();
            //TODO DEATH CODE
        }
        else{
            health = nhealth;
            healthProg.Value = nhealth;
        }
    }
    public int getHealth(){
        return health;
    }

    public void setTarget(Area2D t){
        target = t;
    }

}
