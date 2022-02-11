using Godot;
using System;

public class PlayerMovement : KinematicBody2D
{
    [Signal]
    public delegate void Hit();
    
    private float speed; //How fast the player will move
    private float minSpeed = 270; //Minimum Speed
    private float acceleration = 0.3f; //Determines how fast the player gets up to speed
    private float friction = 0.1f; //Determines how fast the player slows down

    private bool sprint; //Tells if the player is hitting the sprint button
    private float sprintMultiplier = 1.5f; //Speed multiplier when sprinting
    private float maxStamina = 1; //Maximum amount of stamina
    private float stamina; //Holds the current stamina at any point
    private float staminaGainRate = 0.5f; //Rate that stamina regenerates at

    private Vector2 velocity = Vector2.Zero; //Velocity that starts at zero

    private AnimatedSprite animationPlayer; //Animation player

    float shotCooldown = 0;
    float fireDelay = 0.2f;
    PackedScene BulletScene;
    PackedScene ZombieScene;
    public Player player = new Player();
    public float knockback = 0;
    public KinematicBody2D lasthitbody;


    public override void _Ready(){

        BulletScene = GD.Load<PackedScene>("res://Bullet.tscn");
        ZombieScene = GD.Load<PackedScene>("res://Zombie.tscn");
        speed = player.GetSpeed();
        stamina = maxStamina;
        animationPlayer = GetNode<AnimatedSprite>("AnimatedSprite");
    }
    public override void _Input(InputEvent inputEvent){
        if(inputEvent is InputEventMouseButton button){
            shoot();
        }
    }

    public override void _Process(float delta){
        shotCooldown += delta;
        if(Input.IsMouseButtonPressed(1)){
            shoot();
        }
        if(player.GetHealth()>0){ 
            MovePlayer(delta);
            LookAt(GetGlobalMousePosition());
        }
        else{
            //Death Anim End Game
        }
    }

    public void MovePlayer(float delta){
        //Start each frame with clear input velocity
        Vector2 inputVelocity = Vector2.Zero;

        //Read user input to inputVelocity
        if(Input.IsActionPressed("move_right"))
            inputVelocity.x += 1;
        if(Input.IsActionPressed("move_left"))
            inputVelocity.x -= 1;
        if(Input.IsActionPressed("move_up"))
            inputVelocity.y -= 1;
        if(Input.IsActionPressed("move_down"))
            inputVelocity.y += 1;

        //Check if the player is pressing the sprint button
        if(Input.IsActionPressed("sprint"))
            sprint = true;
        else
            sprint = false;

        //Calculate speed and move the player if there is any inputVelocity
        if(inputVelocity.Length() > 0)
        {   
            //tempSpeed will be modified depending on sprint and direction they're walking
            float tempSpeed = speed;
            
            //Speed adjusted depending on the angle between player facing direction and velocity direction. Player moves slower the further away the vectors
            tempSpeed *= 1 - (Mathf.Abs(inputVelocity.AngleTo(GetGlobalMousePosition() - GlobalPosition)) / (float)Math.PI);

            //Speed can't be lower than min speed
            if(tempSpeed < minSpeed)
                tempSpeed = minSpeed;

            //Decrement the stamina while the player is sprinting and increase speed
            if(sprint && stamina > 0)
            {
                tempSpeed *= sprintMultiplier;
                stamina -= delta;
            }
                
            //Set the final input velocity after modifying for direction and sprinting
            inputVelocity = inputVelocity.Normalized() * tempSpeed;

            //Change actual velocity smoothly
            velocity = velocity.LinearInterpolate(inputVelocity, acceleration);

            //Play the walk animation
            animationPlayer.Play("Walk");

            if(!GetNode<AudioStreamPlayer2D>("StepSound").Playing)
                GetNode<AudioStreamPlayer2D>("StepSound").Play();

        }
        else
        {
            //If no movement input, slow down to zero velocity
            velocity = velocity.LinearInterpolate(Vector2.Zero, friction);

            //Stop playing the walk animation
            animationPlayer.Stop();
        }

        //Regenerate stamina if not sprinting
        if(stamina < maxStamina && !sprint)
        {
            stamina += delta * staminaGainRate;
        }

        if(knockback>0){
            Vector2 knock_point =  GlobalPosition - lasthitbody.GlobalPosition;
            velocity = knock_point.Normalized() * 800;
            
            MoveAndSlide(velocity);
            knockback--;
            if(!GetNode<AudioStreamPlayer2D>("DamageSound").Playing)
                GetNode<AudioStreamPlayer2D>("DamageSound").Play();
        }
        //Update the players position
        MoveAndSlide(velocity);
    }



    public void shoot(){
        if(shotCooldown>fireDelay){
            shotCooldown = 0;
            Bullet bullet = (Bullet)BulletScene.Instance();
            bullet.Rotation = (float)(Rotation + Math.PI/2);
            GetParent().AddChild(bullet);
            // GetParent().AddChildBelowNode(GetNode<Area2D>("Player"), bullet);
            bullet.Position = GetNode<Position2D>("MuzzlePos").GlobalPosition;
            bullet.ApplyCentralImpulse(new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation)) * 3000);
            GD.Print(bullet);
            GetNode<AudioStreamPlayer2D>("ShootSound").Play();

        }
    }
}
