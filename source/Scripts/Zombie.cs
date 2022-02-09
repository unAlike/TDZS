using Godot;
using System;
using System.Collections.Generic;

public class Zombie : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    int health = 3;
    public KinematicBody2D target;
    public Vector2 velocity = new Vector2();
    TextureProgress healthProg;
    Navigation2D nav;
    List<Vector2> path; //Stores a list of points which make up a path to the target

    int currentPoint = 1; //Stores the current point to move to

    public override void _Ready()
    {
        healthProg = GetNode<TextureProgress>("HealthBar");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta){
        PathFind();
    }

    void PathFind()
    {
        if(target!=null){
            KinematicBody2D zom = GetNode<KinematicBody2D>("Zombie");
            zom.LookAt(target.GlobalPosition);

            //If the player is not within range of the end of the current path...
            if(GlobalPosition.DistanceTo(path[path.Count-1]) > 2)
            {
                //Generate a new path towards the players current position
                path = new List<Vector2>(nav.GetSimplePath(zom.GlobalPosition, target.GlobalPosition, false));
                currentPoint = 1; //Reset the current point to the first point of the new path
            }
            
             //If the path exists
            if(path.Count != 0)
            {
                //Move Zombie towards the current point
                velocity = zom.GlobalPosition.DirectionTo(path[currentPoint]) * 300;
                zom.MoveAndSlide(velocity);
            }

            //If the zombie it within 1 unit of the next point, start moving to the one after it
            if(GlobalPosition.DistanceTo(path[Mathf.Clamp(currentPoint + 1, 0, path.Count-1)]) < 1)
            {
                currentPoint += 1;
            }

            // velocity = nav.GetClosestPoint(zom.GlobalPosition.DirectionTo(target.GlobalPosition)) * 300;
            // velocity = zom.MoveAndSlide(velocity);
            // nav.


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

    public void setTarget(KinematicBody2D t){
        target = t;
    }

    public void BodyEntered(Node body){
        if(body.Name == "Player"){
            PlayerMovement player = (PlayerMovement)body;
            player.player.SetHealth(player.player.GetHealth()-1);
            GD.Print(player.player.GetHealth());
            player.knockback = 3;
            player.lasthitbody = GetNode<KinematicBody2D>("Zombie");
        }
    }

    //This function is called from the GameLoop whenever a new zombie is spawned and passed the level navigation to the zombie
    public void setNav(Navigation2D levelNav)
    {
        nav = levelNav;
        path = new List<Vector2>(nav.GetSimplePath(GlobalPosition, target.GlobalPosition)); //Generate an initial path
        
    }

}
