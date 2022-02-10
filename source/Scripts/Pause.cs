using Godot;
using System;

public class Pause : Node
{
    ColorRect PauseMenu, ControlsMenu, DeathMenu;
    bool InControls = false;
    bool InPause = false;
    bool IsDead = false;
    Player player;
public override void _Ready()
    {
        GD.Print(GetTree().CurrentScene.Name);
        var playerm =  (PlayerMovement)(GetTree().CurrentScene.GetNode<KinematicBody2D>("Player"));
        player = playerm.player;
        PauseMenu = GetNode<ColorRect>("PauseMenu");
        ControlsMenu = GetNode<ColorRect>("Controls");
        DeathMenu = GetNode<ColorRect>("DeathMenu");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      if(Input.IsActionJustPressed("ui_cancel") & InControls == false & IsDead == false) {
          GetTree().Paused = !GetTree().Paused;
          PauseMenu.Visible = !PauseMenu.Visible;
          InPause = !InPause;
      }

      if(Input.IsActionJustPressed("ui_cancel") & InControls == true) {
          ControlsMenu.Visible = false;
          PauseMenu.Visible = true;
          InControls = false;
      }
      try{
        if(player.GetHealth() == 0) {
            GetTree().Paused = true;
            DeathMenu.Visible = true;
            IsDead = true;
        }
      }catch(NullReferenceException){

      }
  }

  public void _on_Resume_pressed() {
      GetTree().Paused = !GetTree().Paused;
      PauseMenu.Visible = false;
      InPause = false;
  }

  public void _on_Controls_pressed() {
      ControlsMenu.Visible = true;
      PauseMenu.Visible = false;
      InControls = true;
  }

  public void _on_MainMenu_pressed() {
      GetTree().Paused = !GetTree().Paused;
      player.SetKills(0);
      GetTree().ChangeScene("res://MainMenu.tscn");
      foreach(Node n in GetTree().Root.GetChildren()){
          GD.Print(n.Name);
          if(n.Name == "Node2D"){
              n.QueueFree();
              GD.Print("Killed");
          }
          if(n.Name.Contains("Zombie")){
              n.QueueFree();
          }
      }

      InPause = false;
      player.SetHealth(player.GetMaxHealth());
      
      
  }

   public void _on_Back_pressed() {
        ControlsMenu.Visible = false;
        PauseMenu.Visible = true;
        InControls = false;
   }

   public void _on_Replay_pressed() {
       GetTree().ReloadCurrentScene();
       player.SetHealth(player.GetMaxHealth());
       GetTree().Paused = false;
   }

}
