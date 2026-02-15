Level Data Instructions

The JSON files in this folder define the spawn events for a level in the game. Each spawn event has three fields:

• delay: (float) The time in seconds to wait before spawning this object, relative to the previous event.
• prefabKey: (string) A key that determines which prefab to spawn. Valid keys are:
	- WildHog: Spawns a Wild Hog enemy.
	- IronPlate: Spawns an Iron Plate collectible.
	- IronOre: Spawns an Iron Ore collectible.
	- RoadBarrier: Spawns a Road Barrier obstacle.
• spawnY: (float) The Y position (vertical) at which the object will spawn.

Example JSON structure:

{
    "spawnEvents": [
        {
            "delay": 1.0,
            "prefabKey": "IronOre",
            "spawnY": 0.5
        },
        {
            "delay": 2.5,
            "prefabKey": "WildHog",
            "spawnY": 1.0
        }
        // Add additional spawn events here...
    ]
}

How to Create/Modify Levels:

Open the level1.json file in your preferred text editor or IDE (Visual Studio, VS Code, etc.). These editors offer color-coded formatting to help you read and edit the file.
Edit the spawn events as needed, ensuring that the sum of the delay values corresponds to the desired total level time (for example, around 300 seconds for a 5-minute level).
Save your changes. The game will load the updated level data at runtime.