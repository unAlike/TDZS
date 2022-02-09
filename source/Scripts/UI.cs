using Godot;
using System;

public class UI : Node
{
    
    TextureProgress PlayerHealth;
    Player HealthUI = new Player();
    RichTextLabel ScoreUI;
    Zombie Score = new Zombie();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PlayerHealth = GetNode<TextureProgress>("PlayerHealth");
        ScoreUI = GetNode<RichTextLabel>("PlayerScore");
    }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      PlayerHealth.Value = HealthUI.GetHealth();
      PlayerHealth.MaxValue = HealthUI.GetMaxHealth();
      ScoreUI.Text = "" + (Score.getScore());
  }

}
