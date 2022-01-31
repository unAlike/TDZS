using Godot;
using System;

public class PlayerMovement : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    public float Speed; //How fast the player will move

    [Export]
    public int fireDelay = 50;

    public Vector2 ScreenSize; //Size of the game window
    private AnimationPlayer animationPlayer; //Animation player

    int shotCooldown = 0;
    PackedScene BulletScene;

    public override void _Ready()
    {
        BulletScene = GD.Load<PackedScene>("res://Bullet.tscn");
        ScreenSize = GetViewportRect().Size;
        Player player = new Player();
        Speed = player.GetSpeed();

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _Process(float delta)
    {
        if(shotCooldown<=fireDelay) shotCooldown++;
        LookAt(GetGlobalMousePosition());
        MovePlayer(delta);
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

        

        //Normalize velocity direction and calculate speed based on direction
        if(velocity.Length() > 0)
        {   
            float tempSpeed = Speed;
            tempSpeed *= 1 - (Mathf.Abs(velocity.AngleTo(GetGlobalMousePosition() - GlobalPosition)) / (float)Math.PI);

            if(tempSpeed < Speed * 0.65)
                tempSpeed = Speed * 0.65f;

            velocity = velocity.Normalized() * tempSpeed;
            animationPlayer.Play("Walk");
        }
        else
        {
            animationPlayer.Stop();
        }
        
        //Update Position
        Position += velocity * delta;
        //Clamp Position to stay on screen
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, 0, ScreenSize.x),
            y: Mathf.Clamp(Position.y, 0, ScreenSize.y)
            );
    }

    public override void _Input(InputEvent inputEvent){
        if(inputEvent is InputEventMouseButton button){
            shoot();
        }
    }

    public void shoot(){
        if(shotCooldown>fireDelay){
            shotCooldown = 0;
            Bullet bullet = (Bullet)BulletScene.Instance();
            GetParent().AddChild(bullet);
            bullet.Position = Position;
            bullet.ApplyCentralImpulse(new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation)) * 1000);
        }
    }
}
