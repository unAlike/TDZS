using Godot;
using System;

public class PlayerMovement : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    public float Speed = 0; //How fast the player will move

    [Export]
    public int fireDelay = 50;
    public Vector2 ScreenSize; //Size of the game window

    int shotCooldown = 0;
    PackedScene BulletScene;
    PackedScene ZombieScene;

    public override void _Ready()
    {
        BulletScene = GD.Load<PackedScene>("res://Bullet.tscn");
        ZombieScene = GD.Load<PackedScene>("res://Zombie.tscn");
        ScreenSize = GetViewportRect().Size;
        Player player = new Player();
        Speed = player.GetSpeed();

        //Hide();
    }

    public override void _Process(float delta)
    {
        if(shotCooldown<=fireDelay) shotCooldown++;
        LookAt(GetGlobalMousePosition());
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

        if(velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
        }
        if(Input.IsKeyPressed(32)){
            Zombie zom = ZombieScene.Instance<Zombie>();
            zom.setTarget(this);
            zom.Position = Position;
            GetParent().AddChild(zom);
        }

        Position += velocity * delta;
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
            bullet.Rotation = (float)(Rotation + Math.PI/2);
            GetParent().AddChild(bullet);
            bullet.Position = Position;
            bullet.ApplyCentralImpulse(new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation)) * 1000);
        }
    }
}
