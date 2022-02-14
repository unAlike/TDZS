using Godot;
using System;

public class UI : CanvasLayer
{

    TextureProgress PlayerHealth, PlayerStamina;
    Player HealthUI = new Player();
    RichTextLabel ScoreUI, LevelUI;
    Player player;
    GameLoop gl;
    PlayerMovement pm;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // GD.Print(GetTree().CurrentScene.Name);
        pm =  (PlayerMovement)(GetTree().CurrentScene.GetNode<KinematicBody2D>("Player"));
        player = pm.player;
        // GD.Print(playerm.player.GetHealth());
        PlayerHealth = GetNode<TextureProgress>("PlayerHealth");
        PlayerStamina = GetNode<TextureProgress>("PlayerStamina");
        ScoreUI = GetNode<RichTextLabel>("PlayerScore");
        LevelUI = GetNode<RichTextLabel>("Level");
        gl = (GameLoop)(GetTree().Root.GetNode<Node2D>("Node2D"));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        PlayerHealth.Value = player.GetHealth();
        PlayerHealth.MaxValue = player.GetMaxHealth();
        ScoreUI.Text = "" + (player.GetKills());
        LevelUI.Text = "Level: " + gl.level;
        PlayerStamina.Value = pm.stamina * 100;
        PlayerStamina.MaxValue = pm.maxStamina * 100;
    }

}
