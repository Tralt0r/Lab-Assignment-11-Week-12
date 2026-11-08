Save and Load



Press S to save the game and L to load it. The game also loads the last save automatically when it starts, if a save file exists.



What gets saved

\- The score and hit count

\- The shooter's position

\- The position of every target on screen (plus its speed, points, and color so it can be rebuilt)



The files

Two files, saved in Unity's `Application.persistentDataPath

\-positions.json = the shooter and target info, saved as JSON

\-score.dat = the score and hits, saved as a binary file



How it works



ISaveable

Where everything is saved. It has three things: a SaveID, SavedData, and LoadFromData.



TransformSaver

Implements ISaveable. It saves the object's position and scale. The shooter gets a fixed ID. Every target gets one automatically when the TargetBuilder builds it, with a random ID.



SaveManager

Listens for the S and L keys.

\-Save = it finds everything in the scene that has ISaveable, asks each one for its data, and puts it all in one list. If an object is a target, it also saves its speed, points, and color. The list is written to positions.json. Then the score is written to score.data.

\-Load = it reads positions.json. The shooter is found by its ID and moved back to its original spot. Targets are different because they get destroyed all the time. It deletes the targets on screen, rebuilds each saved target with the TargetBuilder, and moves it to its saved position. Then the score is loaded from score.data.



Saving the score in binary

ScoreManager has SaveBinary() and LoadBinary(). They use BinaryWriter and BinaryReader to write and read the score.



Why it's set up this way

Loading targets through the Builder means a loaded target is made the exact same way as a spawned one. SaveManager doesn't know what each object is, it just asks for the save data.

