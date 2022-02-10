using Godot;
using System;

public class Player
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private static float health = 30;
    private float speed = 400;
    private float maxHealth = 30;
    private int kills = 0;


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

    public float GetMaxHealth(){
        return maxHealth;
    }
    public int GetKills(){
        return kills;
    }
    public void AddKills(int i){
        kills += i;
    }
    public void SetKills(int i){
        kills = i;
    }
}
