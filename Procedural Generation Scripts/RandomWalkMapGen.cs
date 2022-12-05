using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalkMapGen : AbstractDungeonGen
{
    [SerializeField]
    protected DungeonScriptableObjects randomWalkParams; // WITH THIS WE CAN SET THE PARAMETERS OF OUR DUNGEON FROM THE EDITOR OR CHANGE IT DEPENDING ON THE LEVEL 
    // BY USING A DIFFERENT SO
    protected override void RunPPG()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParams,startPos);
        tileMaker.ClearTiles();
        tileMaker.PlaceFloorTiles(floorPositions);
        WallMaker.MakeWalls(floorPositions, tileMaker);
    }
    protected HashSet<Vector2Int> RunRandomWalk(DungeonScriptableObjects randomWalkParams,Vector2Int position) // protected means that this can only be acessed by classes that derive from RandomWalkMapGen
    {
        var pos = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // Hashset of positions where we need to draw tiles
        for(int i=0;i < randomWalkParams.loops; i++)
        {
            var path = RandomWalk.RandomWalkAlg(pos, randomWalkParams.stepsToTake); // We make a union between the taken path and the tile positions so there aren't gaps
            floorPositions.UnionWith(path); // UnionWith is a method from the HashSet
            //We do this to not go over tiles multiple times :)
            if (randomWalkParams.startRandomlyEachLoop)
            {
                pos = floorPositions.ElementAt(Random.Range(0, floorPositions.Count)); 
            }
        }
        return floorPositions;
    }

   
}
