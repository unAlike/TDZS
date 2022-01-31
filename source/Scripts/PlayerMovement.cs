using Godot;
using System;

public class PlayerMovement : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    public float Speed; //How fast the player will move

    public Vector2 ScreenSize; //Size of the game window
    private float lastDistance; //Store the distance from player to cursor from the last frame

    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
        Player player = new Player();
        Speed = player.GetSpeed();
        lastDistance = GlobalPosition.DistanceTo(GetGlobalMousePosition());
    }

    public override void _Process(float delta)
    {
        LookAt(GetGlobalMousePosition());
        MovePlayer(delta);
        lastDistance = GlobalPosition.DistanceTo(GetGlobalMousePosition());
    }

    public void MovePlayer(float delta)
    {
        var velocity = Vector2.Zero;

        if(Input.IsActionPressed("move_right"))
        {
            velocity.x += 1;
        }
        if(Input.IsActionPressed("move_left"))
        {
            velocity.x -= 1;
        }
        if(Input.IsActionPressed("move_up"))
        {
            velocity.y -= 1;
        }
        if(Input.IsActionPressed("move_down"))
        {
            velocity.y += 1;
        }

        //If the player is not moving in the direction they are facing, they move at 75% speed
        float tempSpeed = Speed;
        if(GetDirection() != velocity)
            tempSpeed *= 0.75f;

        //Normalize velocity direction and multiply by speed if there is a velocity
        if(velocity.Length() > 0)
        {   
            velocity = velocity.Normalized() * tempSpeed;
        }
        
        //Update Position
        Position += velocity * delta;
        //Clamp Position to stay on screen
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, 0, ScreenSize.x),
            y: Mathf.Clamp(Position.y, 0, ScreenSize.y)
            );
    }

    public Vector2 GetDirection()
    {
        float rotation = RotationDegrees;

        //Make sure the direction is always a positive angle
        if(rotation > 360)
            RotationDegrees = 0;
        if(rotation < -360)
            RotationDegrees = 0;
        if(rotation < 0)
            RotationDegrees += 360;

        Vector2 direction = Vector2.Zero;

        //Convert angle to one of four directions
        if(rotation < 45 || rotation > 315)
            direction = Vector2.Right;
        else if(rotation < 135)
            direction = Vector2.Down;
        else if(rotation < 225)
            direction = Vector2.Left;
        else if(rotation < 315)
            direction = Vector2.Up;

        return direction;
    }
}
