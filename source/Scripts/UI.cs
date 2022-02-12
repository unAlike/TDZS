using Godot;
using System;

public class UI : CanvasLayer
{

    TextureProgress PlayerHealth;
    Player HealthUI = new Player();
    RichTextLabel ScoreUI, LevelUI;
    Player player;
    GameLoop gl;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // GD.Print(GetTree().CurrentScene.Name);
        var playerm =  (PlayerMovement)(GetTree().CurrentScene.GetNode<KinematicBody2D>("Player"));
        player = playerm.player;
        // GD.Print(playerm.player.GetHealth());
        PlayerHealth = GetNode<TextureProgress>("PlayerHealth");
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
    }

}
