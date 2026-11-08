Lab Submission 11

Members: Xavier Rosario

What I changed (`Pathfinding.cs`, BFS)

SetStart and SetGoal = move start/goal at runtime and recompute the path.

GenerateRandomGrid(width, height, obstacleProbability) = random walls; start and goal stay walkable.

AddObstacle(Vector2Int) = blocks a cell and recompute

Controls: Left click = start, Right click = goal, Shift + Left click = obstacle, G = new random grid.

Reflection
1. How does each algorithm prioritize paths?

BFS checks cells in the order it finds them, spreading out one step at a time in every direction. It finds the path with the fewest steps, but it doesn’t care about cost. Dijkstra always checks the cell with the lowest total cost at point discovery, so it finds the cheapest path. It still spreads out in every direction because it doesn’t know where the goal is. A* works like Dijkstra but also guesses how far each cell is from the goal. This makes it head toward the goal, so it checks fewer cells. It still finds the best path.

2. Challenges of changing obstacles in real time

A new wall can make the old path blocked or unnavigable. A wall might also be on the path, the start, or the goal, or block every route to the goal. To avoid recalculating a lot, you can recalculate only when the new wall is on the current path.

3. Which algorithm for larger grids / open worlds?

I would use A* as it already knows where the goal is and doesn't need to search as much. For bigger maps, I would use a faster priority queue (a binary heap) and split the map into large regions, planning between regions first. I could also use navigation meshes instead of a grid.

4. Adding weighted cells

Use a cost function and have different terrain cost more and switch to Dijkstra or A* with newCost system. I would also scale the A*  by the terrain cost.
