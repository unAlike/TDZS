using Godot;
using System;

public class Player
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private float health = 100;
    private float speed = 400;


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public float GetHealth(){
        return health;
    }
    public void SetHealth(float newHealth){
        health = newHealth;
    }

    public float GetSpeed(){
        return speed;
    }
    public void SetSpeed(float newSpeed){
        speed = newSpeed;
    }
}
