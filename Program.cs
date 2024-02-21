namespace CAB201Project
{   
    class Program
    {       
        static void Main()
        {
            while (true)
            {
                DisplayMenu();
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "g":
                        AddGuard();
                        break;
                    case "f":
                        AddFence();
                        break;
                    case "s":
                        AddSensor();
                        break;
                    case "c":
                        AddCamera();
                        break;
                    case "b":
                        AddSawBlades();
                        break;
                    case "d":
                        ShowSafeDirections();
                        break;
                    case "m":
                        DisplayObstacleMap();
                        break;
                    case "p":
                        FindSafePath();
                        break;
                    case "l":
                        AddLaserGrid();
                        break;
                    case "x":
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
        
        static void DisplayMenu()
        {
            Console.WriteLine("Select one of the following options");
            Console.WriteLine("g) Add 'Guard' obstacle");
            Console.WriteLine("f) Add 'Fence' obstacle");
            Console.WriteLine("s) Add 'Sensor' obstacle");
            Console.WriteLine("c) Add 'Camera' obstacle");
            Console.WriteLine("l) Add 'LaserGrid' obstacle");
            Console.WriteLine("b) Add 'Sawblade' obstacle");
            Console.WriteLine("d) Show safe directions");
            Console.WriteLine("m) Display obstacle map");
            Console.WriteLine("p) Find safe path");
            Console.WriteLine("x) Exit");
            Console.Write("Enter code: ");
        }

        static List<Obstacle> obstaclelist = new List<Obstacle>();


        static void AddGuard()
        {
            Console.Write("Enter the guard's location (X,Y): ");
            try
            {
                var coordinates = Console.ReadLine().Split(',');
                int x = int.Parse(coordinates[0]);
                int y = int.Parse(coordinates[1]);

                var guard = new Guard(x, y);
                obstaclelist.Add(guard);

                Console.WriteLine("Guard added successfully.");
            }
            catch
            {
                Console.WriteLine("Invalid input.");
                AddGuard();  // Retry adding the guard
            }
        }

        /// <summary>
        /// Show safe directions based on the given locations and the obstacles
        /// </summary>
        static void ShowSafeDirections()
        {
            Console.Write("Enter your current location (X,Y):\n");
            try
            {
                var coordinates = Console.ReadLine().Split(',');
                int x = int.Parse(coordinates[0]);
                int y = int.Parse(coordinates[1]);

                bool northSafe = true, southSafe = true, eastSafe = true, westSafe = true; // Initially setting the directions as safe 

                foreach (var obstacle in obstaclelist)
                {
                    if (obstacle.IsPositionBlocked(x, y))
                    {
                        Console.WriteLine("Agent, your location is compromised. Abort mission.");
                        return;
                    }
                    if (obstacle.IsPositionBlocked(x, y - 1))
                    {
                        northSafe = false;
                    }
                    if (obstacle.IsPositionBlocked(x, y + 1))
                    {
                        southSafe = false;
                    }
                    if (obstacle.IsPositionBlocked(x + 1, y))
                    {
                        eastSafe = false;
                    }
                    if (obstacle.IsPositionBlocked(x - 1, y))
                    {
                        westSafe = false;
                    }
                }

                var safeDirections = new List<string>();

                if (northSafe) safeDirections.Add("N");
                if (southSafe) safeDirections.Add("S");
                if (eastSafe) safeDirections.Add("E");
                if (westSafe) safeDirections.Add("W");

                if (safeDirections.Count == 0)
                {
                    Console.WriteLine("You cannot safely move in any direction. Abort mission.");
                    return;
                }

                Console.WriteLine("You can safely take any of the following directions: " + string.Join("", safeDirections));

            }
            catch
            {
                Console.WriteLine("Invalid input, Please try again");
                ShowSafeDirections();
            }
        }


        static void AddFence()
        {
            Console.Write("Enter the location where the fence starts (X,Y):\n");
            var startCoordinates = Console.ReadLine().Split(',');

            Console.Write("Enter the location where the fence ends (X,Y):\n");
            var endCoordinates = Console.ReadLine().Split(',');

            try
            {
                int startX = int.Parse(startCoordinates[0]);
                int startY = int.Parse(startCoordinates[1]);
                int endX = int.Parse(endCoordinates[0]);
                int endY = int.Parse(endCoordinates[1]);

                if ((startX == endX) || (startY == endY))
                {
                    var fence = new Fence(startX, startY, endX, endY);
                    obstaclelist.Add(fence);
                    Console.WriteLine("Fence added successfully.");
                }
                else
                {
                    Console.WriteLine("The fences must be placed horizontally or vertically, Please try again");
                    AddFence();
                }
            }
            catch
            {
                Console.WriteLine("Invalid input, Please try again");
                AddFence();  // Retry adding the fence
            }
        }


        static void AddSensor()
        {
            Console.Write("Enter the sensor's location (X,Y): ");
            var coordinates = Console.ReadLine().Split(',');

            Console.Write("Enter the sensor's range (in klicks): ");
            float range;

            try
            {
                int x = int.Parse(coordinates[0]);
                int y = int.Parse(coordinates[1]);

                if (float.TryParse(Console.ReadLine(), out range) && range > 0)
                {
                    var sensor = new Sensor(x, y, range);
                    obstaclelist.Add(sensor);
                    Console.WriteLine("Sensor added successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid range input.");
                    AddSensor();  // Retry adding the sensor
                }
            }
            catch
            {
                Console.WriteLine("Invalid input.");
                AddSensor();  // Retry adding the sensor
            }
        }
        static void AddCamera()
        {
            Console.Write("Enter the camera's location (X,Y): ");
            try
            {
                var coordinates = Console.ReadLine().Split(',');
                int x = int.Parse(coordinates[0]);
                int y = int.Parse(coordinates[1]);

                char direction;
                while (true)
                {
                    Console.Write("Enter the direction the camera is facing (n, s, e, or w): ");

                    direction = Console.ReadLine().ToLower()[0];

                    if (new[] { 'n', 's', 'e', 'w' }.Contains(direction)) break;
                    Console.WriteLine("Invalid direction.");
                }

                var camera = new Camera(x, y, direction);
                obstaclelist.Add(camera);
                Console.WriteLine("Camera added successfully.");
            }
            catch
            {
                Console.WriteLine("Invalid input.");
                AddCamera();  // Retry adding the camera
            }
        }

        static void AddLaserGrid()
        {
            Console.Write("Enter the Laser Grid's top-left corner location (X,Y): ");
            try
            {
                var coordinates = Console.ReadLine().Split(',');
                int x = int.Parse(coordinates[0]);
                int y = int.Parse(coordinates[1]);

                Console.Write("Enter the Width of the Laser Grid: ");
                int width = int.Parse(Console.ReadLine());

                Console.Write("Enter the Height of the Laser Grid: ");
                int height = int.Parse(Console.ReadLine());

                if (width > 0 && height > 0)
                {
                    var laserGrid = new LaserGrid(x, y, width, height);
                    obstaclelist.Add(laserGrid);
                    Console.WriteLine("Laser Grid added successfully.");
                }
                else
                {
                    Console.WriteLine("Width and Height must be greater than 0. Try again.");
                    AddLaserGrid();  // Retry adding the Laser Grid
                }
            }
            catch
            {
                Console.WriteLine("Invalid input.");
                AddLaserGrid();  // Retry adding the Laser Grid
            }
        }

        static void AddSawBlades()
        {
            Console.Write("Enter the Sawlade's location (X,Y): ");
            var coordinates = Console.ReadLine().Split(',');

            Console.Write("Enter the length of the blade: ");
            float bladelength;

            try
            {
                int x = int.Parse(coordinates[0]);
                int y = int.Parse(coordinates[1]);

                if (float.TryParse(Console.ReadLine(), out bladelength) && bladelength > 0)
                {
                    var sawblade = new Sawblade(x, y, bladelength);
                    obstaclelist.Add(sawblade);
                    Console.WriteLine("Sawblade added successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid blade length, try again");
                    AddSawBlades();  // Retry 
                }
            }
            catch
            {
                Console.WriteLine("Invalid input.");
                AddSawBlades();  // Retry 
            }

        }

        /// <summary>
        /// Displays a map of obstacles within specified coordinates.
        /// </summary>
        static void DisplayObstacleMap()
        {
            
            Console.Write("Enter the location of the top-left cell of the map (X,Y): \n");
            var topLeft = Console.ReadLine().Split(',');
            int startX = int.Parse(topLeft[0]);
            int startY = int.Parse(topLeft[1]);

            Console.Write("Enter the location of the bottom-right cell of the map (X,Y): \n");
            var bottomRight = Console.ReadLine().Split(',');
            int endX = int.Parse(bottomRight[0]);
            int endY = int.Parse(bottomRight[1]);

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    char obstacleChar = GetObstacleCharAt(x, y);
                    Console.Write(obstacleChar);
                }
                Console.WriteLine(); // Move to the next line for next row
            }
        }
        /// <summary>
        /// Retrieves the character representing an obstacle at a given position.
        /// </summary>
        /// <param name="x">The x-coordinate of the position.</param>
        /// <param name="y">The y-coordinate of the position.</param>
        /// <returns>A character representing the obstacle, or '.' if no obstacle is present.</returns>
        static char GetObstacleCharAt(int x, int y)
        {
            foreach (var obstacle in obstaclelist)
            {
                if (obstacle.IsPositionBlocked(x, y))

                {
                    if (obstacle is Guard) return 'g';
                    if (obstacle is Fence) return 'f';
                    if (obstacle is Sensor) return 's';
                    if (obstacle is Camera) return 'c';
                    if (obstacle is Sawblade) return 'b';
                    if (obstacle is LaserGrid) return 'l';
                }
            }
            return '.'; // No obstacle at this position
        }

        /// <summary>
        /// Performs Breadth-First Search to find the shortest path from the given start position to the end position.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns> A dictionary that maps each position to its predecessor on the shortest path found. 
        /// If no path is found, it returns null.
        /// </returns>
        private static Dictionary<(int, int), (int, int)> BFS((int, int) start, (int, int) end)
        {
            var visited = new HashSet<(int, int)>(); // To keep track of all the visited spots
            var queue = new Queue<(int, int)>(); // Queue for BFS traversal (first in/first out)
            var parentMap = new Dictionary<(int, int), (int, int)>(); 

            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0) // A loop the keeps on running as long as there is items in the queue
            {
                var current = queue.Dequeue();
                if (current == end)
                {
                    return parentMap;
                }

                foreach (var dir in new[] { (-1, 0), (1, 0), (0, -1), (0, 1) }) // Representing N, S, E, W respectively
                {
                    var next = (current.Item1 + dir.Item1, current.Item2 + dir.Item2); // Checking if the next position is not visited and not blocked by obstacles

                    if (!visited.Contains(next) && !IsPositionBlockedByAnyObstacle(next.Item1, next.Item2))
                    {
                        // Add to visited and queue, update parentMap
                        visited.Add(next);
                        queue.Enqueue(next);
                        parentMap[next] = current; // Set current position as parent of the next position
                    }
                }
            }
            // If the queue is empty we return null as there is no path to be found
            return null;
        }

        /// <summary>
        /// Checks if a given position (x, y) on the map is blocked by any obstacle.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if the position is blocked by any obstacle, else false.</returns>
        private static bool IsPositionBlockedByAnyObstacle(int x, int y)
        {
            return obstaclelist.Any(o => o.IsPositionBlocked(x, y));  

        }

        /// <summary>
        ///  Finds and displays a safe path from the user's current location to the given objective loc.
        /// </summary>
        static void FindSafePath()
        {
            Console.Write("Enter your current location (X,Y): \n");
            var startCoords = Console.ReadLine().Split(',');
            int startX = int.Parse(startCoords[0]);
            int startY = int.Parse(startCoords[1]);

            Console.Write("Enter the location of your objective (X,Y): \n");
            var endCoords = Console.ReadLine().Split(',');
            int endX = int.Parse(endCoords[0]);
            int endY = int.Parse(endCoords[1]);

            if (startX == endX && startY == endY)
            {
                Console.WriteLine("Agent, you are already at the objective.\n");
                return;
            }

            if (IsPositionBlockedByAnyObstacle(endX, endY))
            {
                Console.WriteLine("The objective is blocked by an obstacle and cannot be reached.\n");
                return;
            }

            var parentMap = BFS((startX, startY), (endX, endY)); // Calling the BFS function to find a safe path

            if (parentMap == null || !parentMap.ContainsKey((endX, endY))) // If the parent map dictionary is null or does not contain the end coordinates
            {
                Console.WriteLine("There is no safe path to the objective.\n"); 
                return;
            }

            var path = new List<char>(); // Creating a new list to store the directions
            var current = (endX, endY); // Setting the start of the BFS as the end coordinates

            while (current != (startX, startY))
            {
                var parent = parentMap[current];
                // Determine the direction of movement from parent to current position
                if (current.Item1 - parent.Item1 == 1) path.Add('E'); // If the difference in x-coordinates is 1, it means we moved east 
                else if (current.Item1 - parent.Item1 == -1) path.Add('W'); // If the difference in x-coordinates is -1, it means we moved west
                else if (current.Item2 - parent.Item2 == 1) path.Add('S'); // If the difference in y-coordinates is 1, it means we moved south else north
                else path.Add('N');
                current = parent;
            }
            path.Reverse(); // Reverse the path as it was made from end to start

            Console.WriteLine("The following path will take you to the objective:");
            Console.WriteLine(string.Join("", path));
        }
    }
}
