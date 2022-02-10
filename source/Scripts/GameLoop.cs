using Godot;
using System;

public class GameLoop : Node
{
	float time = 0;
	public int kills = 0;
	RandomNumberGenerator rng = new RandomNumberGenerator();
	PackedScene ZombieScene;
	PlayerMovement player;
	Navigation2D nav;
	int zomcount = 0;
	public override void _Ready()
	{
		ZombieScene = GD.Load<PackedScene>("res://Zombie.tscn");
		player = (PlayerMovement)GetNode<KinematicBody2D>("Player");
		nav = GetNode<Navigation2D>("Navigation2D");

	}
	public override void _Process(float delta)
	{
		time += delta;
		if(Math.Floor(time)>zomcount){
			SpawnZombie();
			zomcount++;
			//GD.Print("Spawned Zombie");
		}
	}

	public void SpawnZombie(){
		rng.Randomize();
		TileMap tile = GetNode<TileMap>("TileMap");
		bool spawned = false;
		Rect2 rect = tile.GetUsedRect();
		int x = rng.RandiRange((int)(rect.Position.x),(int)(rect.End.x));
		int y = rng.RandiRange((int)(rect.Position.y),(int)(rect.End.y));
		while(!spawned){
			rng.Randomize();
			if(tile.GetCell(x,y) == TileMap.InvalidCell || tile.GetCell(x,y) == 3){
				x = rng.RandiRange((int)(rect.Position.x),(int)(rect.End.x));
				y = rng.RandiRange((int)(rect.Position.y),(int)(rect.End.y));
				//GD.Print(x + " " + y + " Invalid Spawn");
			}
			else{
				Zombie zom = ZombieScene.Instance<Zombie>();
				zom.setTarget(player);
				zom.setNav(nav);
				GetParent().AddChild(zom);
				zom.GlobalPosition = new Vector2(tile.MapToWorld(new Vector2(x,y)));
				spawned = true;
				//GD.Print("Spanwed Zombie at: X: " + x + "  Y: "+y);
			}
		}
	}

}
