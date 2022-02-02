using Godot;
using System;

public class Pause : Node
{
    ColorRect PauseMenu, ControlsMenu;
    bool InControls = false;
    bool InPause = false;
public override void _Ready()
    {
        PauseMenu = GetNode<ColorRect>("PauseMenu");
        ControlsMenu = GetNode<ColorRect>("Controls");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      if(Input.IsActionJustPressed("ui_cancel") & InControls == false) {
          GetTree().Paused = !GetTree().Paused;
          PauseMenu.Visible = !PauseMenu.Visible;
          InPause = !InPause;
      }

      if(Input.IsActionJustPressed("ui_cancel") & InControls == true) {
          ControlsMenu.Visible = false;
          PauseMenu.Visible = true;
          InControls = false;
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
  }

  public void _on_Back_pressed() {
        ControlsMenu.Visible = false;
        PauseMenu.Visible = true;
        InControls = false;
    }

}
