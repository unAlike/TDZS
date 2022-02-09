using Godot;
using System;

public class Pause : Node
{
    ColorRect PauseMenu, ControlsMenu, DeathMenu;
    bool InControls = false;
    bool InPause = false;
    bool IsDead = false;
    Player Death = new Player();
public override void _Ready()
    {
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

      if(Death.GetHealth() == 0) {
          GetTree().Paused = true;
          DeathMenu.Visible = true;
          IsDead = true;
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
      GetTree().ChangeScene("res://MainMenu.tscn");
      InPause = false;
      Death.SetHealth(100);
  }

   public void _on_Back_pressed() {
        ControlsMenu.Visible = false;
        PauseMenu.Visible = true;
        InControls = false;
   }

   public void _on_Replay_pressed() {
       GetTree().ReloadCurrentScene();
       Death.SetHealth(100);
       GetTree().Paused = false;
   }

}
