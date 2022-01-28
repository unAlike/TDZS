using Godot;
using System;

public class Zombie : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    float health = 3;
    public Area2D target;
    public Vector2 velocity = new Vector2();

    public override void _Ready()
    {

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta){
        if(target!=null){
            LookAt(target.Position);
            velocity = Position.DirectionTo(target.Position) * 100;
            velocity = MoveAndSlide(velocity);
        }
    }


    public void setTarget(Area2D t){
        target = t;
    }
    public void body_entered(){
        GD.Print("Bonk");
    }

}
