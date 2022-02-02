using Godot;
using System;

public class PlayerMovement : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    public float Speed; //How fast the player will move

    public float friction = 0.05f; //Determines how fast the player slows down
    public float acceleration = 0.3f; //Determines how fast the player gets up to speed
    public Vector2 velocity = Vector2.Zero; //Velocity that starts at zero
    public bool sprint; //Tells if the player is hitting the sprint button
    public float sprintMultiplier = 1.5f; //Speed multiplier when sprinting
    public float maxStamina = 1; //Maximum amount of stamina
    public float stamina; //Holds the current stamina at any point
    public float staminaGainRate = 0.5f; //Rate that stamina regenerates at

    public Vector2 ScreenSize; //Size of the game window

    private AnimationPlayer animationPlayer; //Animation player
    float shotCooldown = 0;
    float fireDelay = 0.5f;
    PackedScene BulletScene;
    PackedScene ZombieScene;

    public override void _Ready(){
        BulletScene = GD.Load<PackedScene>("res://Bullet.tscn");
        ZombieScene = GD.Load<PackedScene>("res://Zombie.tscn");
        ScreenSize = GetViewportRect().Size;
        Player player = new Player();
        Speed = player.GetSpeed();
        stamina = maxStamina;
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public override void _Input(InputEvent inputEvent){
        if(inputEvent is InputEventMouseButton button){
            shoot();
            GD.Print("Shot");
        }
    }

    public override void _Process(float delta){
        shotCooldown += delta;
        LookAt(GetGlobalMousePosition());
        MovePlayer(delta);
    }

    public void MovePlayer(float delta){
        var inputVelocity = Vector2.Zero;

        if(Input.IsActionPressed("move_right"))
        {
            inputVelocity.x += 1;
        }
        if(Input.IsActionPressed("move_left"))
        {
            inputVelocity.x -= 1;
        }
        if(Input.IsActionPressed("move_up"))
        {
            inputVelocity.y -= 1;
        }
        if(Input.IsActionPressed("move_down"))
        {
            inputVelocity.y += 1;
        }

        //Check if the player is pressing the sprint button
        if(Input.IsActionPressed("sprint"))
            sprint = true;
        else
            sprint = false;

       //Normalize velocity direction and calculate speed based on direction
        if(inputVelocity.Length() > 0)
        {   

            float tempSpeed = Speed;
            
            //Move at full speed in direction being faced, slower if moving in a different direction.
            tempSpeed *= 1 - (Mathf.Abs(inputVelocity.AngleTo(GetGlobalMousePosition() - GlobalPosition)) / (float)Math.PI);

            //Speed can't be lower than 70%
            if(tempSpeed < Speed * 0.70)
                tempSpeed = Speed * 0.70f;

            //Decrement the stamina while the player is sprinting, else regenerate stamina
            if(sprint && stamina > 0)
            {
                tempSpeed *= sprintMultiplier;
                stamina -= delta;
            }
            else if(!sprint && stamina < maxStamina)
            {
                stamina += delta * staminaGainRate;
            }
                
            //Set the final input velocity after modifying for direction and sprinting
            inputVelocity = inputVelocity.Normalized() * tempSpeed;

            //Change actual velocity smoothly
            velocity = velocity.LinearInterpolate(inputVelocity, acceleration);
            animationPlayer.Play("Walk");
        }
        else
        {
            //If no movement input, slow down to zero velocity
            velocity = velocity.LinearInterpolate(Vector2.Zero, friction);
            animationPlayer.Stop();

            //Regenerate stamina while not moving
            if(stamina < maxStamina)
            {
                stamina += delta * staminaGainRate;
            }
        }

        if(Input.IsKeyPressed(32)){
            Zombie zom = ZombieScene.Instance<Zombie>();
            zom.setTarget(this);
            zom.Position = Position;
            GetParent().AddChild(zom);
        }

        Position += inputVelocity * delta;
    }



    public void shoot(){
        if(shotCooldown>fireDelay){
            shotCooldown = 0;
            Bullet bullet = (Bullet)BulletScene.Instance();
            bullet.Rotation = (float)(Rotation + Math.PI/2);
            GetParent().AddChild(bullet);
            // GetParent().AddChildBelowNode(GetNode<Area2D>("Player"), bullet);
            bullet.Position = Position;
            bullet.ApplyCentralImpulse(new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation)) * 1000);
            GD.Print(bullet);

        }
    }
}
