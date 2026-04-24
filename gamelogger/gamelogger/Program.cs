namespace gamelogger
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text.Json;
    using System.Xml.Linq;

    internal class Program
    {
        class Player
        {
            public string Username { get; set; }
            public int PlayerID { get; set; }
            public int HighScore { get; set; }
            public int HoursPlayed { get; set; }
        }

        static void Main(string[] args)
        {
            MainMenu();

        }
        static void MainMenu()            //Display menu options + record response
        {
            Console.WriteLine("""
                ----------------------------------
                Select an option:
                1. Add Player(s)
                2. Update Player(s) statistics
                3. Search for Player (by ID)
                4. Search for Player (by Username)
                5. Scoreboard
                6. Exit
                ----------------------------------
                """);
            int o = int.Parse(Console.ReadLine());
            if (o < 1 || o > 6)
            {
                while (o < 1 || o > 6)
                {
                    Console.WriteLine("Invalid option, please try again");
                    Console.WriteLine("""
                ----------------------------------
                Select an option:
                1. Add Player(s)
                2. Update Player(s) statistics
                3. Search for Player (by ID)
                4. Search for Player (by Username)
                5. Scoreboard
                6. Exit
                ----------------------------------
                """);
                    o = int.Parse(Console.ReadLine());
                }
            }
            if (o == 1) { AddPlayers(); }
            else if (o == 2) { UpdatePlayer(); }
            else if (o == 3) { PlayerIDSearch(); }
            else if (o == 4) { PlayerUserSearch(); }
            else if (o == 5) { DisplayScores(); }
            else if (o == 6) { } //can stay blank as it breaks code to exit anyway
        }
        static void AddPlayers()            //Read through current file + add new players using input
        {
            //new list for players (after reading out the old list)
            string jsonData = File.ReadAllText("players.json");
            List<Player> playerList1 = JsonSerializer.Deserialize<List<Player>>(jsonData); ; 
            
            string user;
            
            int id, hours, hiscore;
            bool more = false;
            char op = 'N';

            //initialize all variables
            Console.WriteLine("Username: ");
            user = Console.ReadLine();
            Console.WriteLine("Player ID: ");
            id = int.Parse(Console.ReadLine());
            Console.WriteLine("Hours Played: ");
            hours = int.Parse(Console.ReadLine());
            Console.WriteLine("High Score: ");
            hiscore = int.Parse(Console.ReadLine());

            //add inputs to list
            playerList1.Add(new Player { Username = user, PlayerID = id, HoursPlayed = hours, HighScore = hiscore });

            //repeat for multiple new users
            Console.WriteLine("Would you like to add another player? [Y/N] ");
            op = char.Parse(Console.ReadLine());
            if (op == 'Y' || op == 'y')
            {
                more = true;
            }
            while (more)
            {
                Console.WriteLine("Username: ");
                user = Console.ReadLine();
                Console.WriteLine("Player ID: ");
                id = int.Parse(Console.ReadLine());
                Console.WriteLine("Hours Played: ");
                hours = int.Parse(Console.ReadLine());
                Console.WriteLine("High Score: ");
                hiscore = int.Parse(Console.ReadLine());

                playerList1.Add(new Player { Username = user, PlayerID = id, HoursPlayed = hours, HighScore = hiscore });

                Console.WriteLine("Would you like to add another player? [Y/N] ");
                op = char.Parse(Console.ReadLine());
                if (op == 'Y' || op == 'y')
                {
                    more = true;
                }
                else
                {
                    more = false;
                }
            }

            // Output
            Console.WriteLine("Adding: ");
            foreach (Player Player in playerList1)
            {
                Console.WriteLine(Player.Username + " " + Player.PlayerID);
            }
            Console.WriteLine("to file ");

            //Serialize list and store in file
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(playerList1, options);
            File.WriteAllText("players.json", json);

            //return to main menu
            MainMenu();
        }
        static void UpdatePlayer()
        {
            string sUser, sAgain;
            bool bUserFound = false;
            int id, hours, hiscore;


            Console.WriteLine("Enter player's username to update score: ");
            sUser = Console.ReadLine().ToLower();

            string jsonData = File.ReadAllText("players.json");
            List<Player> playerUserList = JsonSerializer.Deserialize<List<Player>>(jsonData);
            Console.WriteLine("Searching for user matching: " + sUser);
            foreach (Player player in playerUserList)
            {
                if (player.Username.ToLower() == sUser && bUserFound == false)
                {
                    Console.WriteLine("Player found! ");
                    bUserFound = true;
                    Console.WriteLine("\nCurrent stats: \nPlayer: " + player.Username + "\nID: " + player.PlayerID + "\nHigh Score: " + player.HighScore + "\nHours Played: " + player.HoursPlayed);

                    Console.WriteLine("Hours Played: ");
                    hours = int.Parse(Console.ReadLine());
                    Console.WriteLine("High Score: ");
                    hiscore = int.Parse(Console.ReadLine());
                }
            }
        }           //Search for player by ID, store info then update
        static void PlayerIDSearch() 
        {
            int pID;
            bool bIDFound = false;
            string sAgain;
            
            Console.WriteLine("Enter a player ID: ");
            pID = int.Parse(Console.ReadLine());

            string jsonData = File.ReadAllText("players.json");
            List<Player> playerIDList = JsonSerializer.Deserialize<List<Player>>(jsonData);
            Console.WriteLine("Searching for player matching ID: " + pID);
            foreach (Player player in playerIDList)
            {
                if (player.PlayerID == pID && bIDFound == false)
                {
                    Console.WriteLine("Player found! ");
                    bIDFound = true;
                    Console.WriteLine("\nPlayer: " + player.Username + "\nID: " + player.PlayerID + "\nHigh Score: " + player.HighScore + "\nHours Played: " + player.HoursPlayed);
                    
                }
            }
            if (bIDFound == false)
            {
                Console.WriteLine("There is not a player matching that ID, search for another? [Y/N] ");
                sAgain = Console.ReadLine();
                if(sAgain == "Y" || sAgain == "y")
                {
                    PlayerIDSearch();
                }
            }

            //return to main menu
            MainMenu();
        }           //Search through file for specific player ID
        static void PlayerUserSearch() 
        {
            string sUser, sAgain;
            bool bUserFound = false;

            Console.WriteLine("Enter a username: ");
            sUser = Console.ReadLine().ToLower();

            string jsonData = File.ReadAllText("players.json");
            List<Player> playerUserList = JsonSerializer.Deserialize<List<Player>>(jsonData);
            Console.WriteLine("Searching for user matching: " + sUser);
            foreach (Player player in playerUserList)
            {
                if (player.Username.ToLower() == sUser && bUserFound == false)
                {
                    Console.WriteLine("Player found! ");
                    bUserFound = true;
                    Console.WriteLine("\nPlayer: " + player.Username + "\nID: " + player.PlayerID + "\nHigh Score: " + player.HighScore + "\nHours Played: " + player.HoursPlayed);

                }
            }
            if (bUserFound == false)
            {
                Console.WriteLine("There is not a player matching that ID, search for another? [Y/N] ");
                sAgain = Console.ReadLine();
                if (sAgain == "Y" || sAgain == "y")
                {
                    PlayerUserSearch();
                }
            }
            MainMenu();
        }          //Search through file for specific username
        static void DisplayScores()
        {
            //open file and store to list
            string jsonData = File.ReadAllText("players.json");
            List<Player> playerList2 = JsonSerializer.Deserialize<List<Player>>(jsonData);

            //read through list and output all values
            Console.WriteLine("Players: ");
            foreach (Player player in playerList2)
                Console.WriteLine("\nPlayer: " + player.Username + "\nID: " + player.PlayerID + "\nHigh Score: " + player.HighScore + "\nHours Played: " + player.HoursPlayed);
        }       //Read through file and display everything
        static void ReadScoreboard()
        {
            //Deserialized
            string jsonData = File.ReadAllText("players.json");
            List<Player> playerList1 = JsonSerializer.Deserialize<List<Player>>(jsonData);

            Console.WriteLine("Deserialized List of Objects: ");
            foreach (Player player in playerList1)
                Console.WriteLine(player.Username + " " + player.PlayerID);
        }
    }
}
