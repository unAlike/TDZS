using Godot;
using System;

public class GameLoop : Node
{
    float time = 0;
    int kills = 0;
    public override void _Ready()
    {
        
    }
    public override void _Process(float delta)
    {
        time += delta;
        GD.Print(time);
    }

    public void SpawnZombie(){
        TileMap tile = GetNode<TileMap>("TileMap");
        tile.
    }

}
