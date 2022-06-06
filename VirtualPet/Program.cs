// See https://aka.ms/new-console-template for more information

using System;

namespace VirtualPet
{
    class Program
    {
        public static readonly Dictionary<string, string> difficulties = new Dictionary<string, string>()
        {
            { "easy", "e" },
            { "medium", "m" },
            { "hard", "h" },
            { "god", "g" }
        };

        public static readonly Dictionary<string, Cake> cakes = new Dictionary<string, Cake>()
        {
            { "cake", new Cake("cake", 2, 0, 0) },
            { "berry", new Cake("berry", 10, 5, 5) },
            { "banana", new Cake("banana", 15, 2, 10) },
            { "peach", new Cake("peach", 20, 0, 20) },
            { "pea", new Cake("pea", 5, 10, 10) },
            { "bean", new Cake("bean", 2, 15, 25) },
            { "pod", new Cake("pod", 0, 20, 40) },
            { "ambrosia", new Cake("ambrosia", 10000, 10000, 200) }
        };

        static void Main(string[] args)
        {
            Console.WriteLine($@"                               --VIRTUAL PET--

A quote from the developer:
                        _______________________________

            {'"'}How long can you keep your creations alive ? !Muahaha!{'"'}
                        _______________________________

");

            // Determine the difficulty the user wishes to play at
            Console.WriteLine("You can choose from: easy, medium, hard, and god.");
            string difficulty = "";
            while (!difficulties.Values.Contains(difficulty))
            {
                Console.Write("What difficulty would you like to play at? ");
                difficulty = Console.ReadLine().Trim().ToLower();
                if (difficulties.Keys.Contains(difficulty))
                {
                    difficulty = difficulties[difficulty];
                }
                else if (!difficulties.Values.Contains(difficulty))
                {
                    Console.WriteLine("I don't recognise that as a difficulty you can play at.");
                }
            }
            Console.WriteLine();

            // Number of ticks incremented between actions
            int ticksIn;
            if (difficulty == "e" || difficulty == "m")
            {
                ticksIn = 1;
            }
            else
            {
                ticksIn = 5;
            }

            int tickCount = 0;
            int wallet = 100;

            // Get the names of the user's pets, loop until valid
            string[] names = new string[] { "", "", "" };
            for (int i = 1; i < 4; i++)
            {
                string name = "";
                while (name == "" || names.Select(n => n.ToLower()).Contains(name.ToLower()))
                {
                    Console.Write($"What is the name of pet {i}? ");
                    name = Console.ReadLine().Trim();
                    if (name == "")
                    {
                        Console.WriteLine("That's not really a name...");
                    }
                    else if (names.Select(n => n.ToLower()).Contains(name.ToLower()))
                    {
                        Console.WriteLine("That'll just get confusing...");
                    }
                }
                names[i - 1] = name;
            }
            Console.WriteLine();

            // Create virtual pets
            Dictionary<string, Pet> pets = new Dictionary<string, Pet>() { };
            Dictionary<string, List<int>> cakeHist = new Dictionary<string, List<int>>() { };
            foreach (string name in names)
            {
                int petType = new Random().Next(1, 3);
                if (petType == 1)
                {
                    pets[name.ToLower()] = new Pet(name);
                }
                else if (petType == 2)
                {
                    pets[name.ToLower()] = new WeakPet(name);
                }
                else
                {
                    pets[name.ToLower()] = new StrongPet(name);
                }
                cakeHist[name.ToLower()] = new List<int> { };
            }

            // Keep playing until all the pets die!
            Dictionary<string, List<string>> cemetery = new Dictionary<string, List<string>>() { };
            while (pets.Keys.Count() > 0)
            {
                for (int i = 0; i < ticksIn; i++)
                {
                    // Print and increment the tick count
                    if (i == ticksIn - 1)
                    {
                        Console.WriteLine($"Tick {tickCount}:\n");
                    }
                    else
                    {
                        Console.WriteLine($"Processing tick {tickCount}...");
                    }
                    tickCount += 1;

                    // Print out status of each pet after tick applied, record happiness
                    int noHappy = 0;
                    List<string> dead = new List<string> { };
                    foreach (string pet in pets.Keys)
                    {
                        pets[pet].Tick();
                        if (i == ticksIn - 1)
                        {
                            pets[pet].Status();
                        }
                        if (pets[pet].Mood[0] == "dead")
                        {
                            Console.WriteLine($"{pets[pet].Name} has died. RIP\n");
                            dead.Add(pet);
                            cemetery[pets[pet].Name] = new List<string> { tickCount.ToString(), Methods.JoinWithAnd(pets[pet].Sounds) };
                        }
                        else if (pets[pet].Mood[2] == "happy")
                        {
                            noHappy += 1;
                        }
                    }

                    foreach (string pet in dead)
                    {
                        pets.Remove(pet);
                    }

                    if (pets.Keys.Count() == 0)
                    {
                        break;
                    }

                    // Determine coins based off happiness
                    if (noHappy == 0)
                    {
                        Console.WriteLine("None of your pets are happy (0 coins gained) :(");
                    }
                    else
                    {
                        Console.WriteLine($"{noHappy} pet{(noHappy == 1 ? " was" : "s were")} happy ({noHappy} coin{(noHappy == 1 ? "" : "s")} gained)!");
                        wallet += noHappy;
                    }
                    Console.WriteLine();
                }

                Console.WriteLine($"You have {wallet} coin{(wallet == 1 ? "" : "s")}.\n");

                // Get action to take
                string input;
                if (pets.Keys.Count() > 0)
                {
                    Console.Write(">>> ");
                    input = Console.ReadLine().Trim().ToLower();
                }
                else
                {
                    input = "";
                }
                Console.WriteLine();

                // Advance to the next tick if the user does nothing
                if (input != "")
                {
                    // Break down command into its components and execute them
                    string pet;
                    string[] action = input.Split(' ');
                    switch (action[0])
                    {
                        case "feed":
                            string cake;
                            try
                            {
                                pet = action[1];
                            }
                            catch (IndexOutOfRangeException)
                            {
                                Console.WriteLine("I understood you as far as wanting to feed a pet.\n");
                                continue;
                            }
                            try
                            {
                                if (pets.Keys.Contains(pet))
                                {
                                    if (action.Count() == 3)
                                    {
                                        cake = action[2];
                                    }
                                    else
                                    {
                                        Console.WriteLine("The cake specified doesn't exist.\n");
                                        continue;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("The pet specified doesn't exist.\n");
                                    continue;
                                }
                            }
                            catch (IndexOutOfRangeException)
                            {
                                Console.WriteLine($"I understood you as far as wanting to feed {pets[pet].Name}.\n");
                                continue;
                            }

                            // Cake can be added to the end of an input, remove here
                            if (cake == "cake" || cake == "cakecake")
                            {
                                cake = "cake";
                            }
                            else if (cakes.Keys.Contains(string.Join("", cake.Split("cake"))))
                            {
                                cake = string.Join("", cake.Split("cake"));
                            }

                            // Feed pet the cake if the user can afford it
                            if (cakes.Keys.Contains(cake))
                            {
                                if (wallet - cakes[cake].Cost >= 0)
                                {
                                    if (difficulty != "m" && difficulty != "g")
                                    {
                                        pets[pet].Feed(cakes[cake]);
                                        wallet -= cakes[cake].Cost;
                                    }
                                    else
                                    {
                                        // Pets cannot eat twice in a row for m/g
                                        try
                                        {
                                            if (cakeHist[pet][cakeHist[pet].Count() - 1] != tickCount - ticksIn)
                                            {
                                                pets[pet].Feed(cakes[cake]);
                                                wallet -= cakes[cake].Cost;
                                                cakeHist[pet].Add(tickCount);
                                            }
                                            else
                                            {
                                                Console.WriteLine($"{pets[pet].Name} is still full from their last cake!\n");
                                            }
                                        }
                                        catch (IndexOutOfRangeException)
                                        {
                                            // Exception occurs if no cakes eaten by pet yet
                                            pets[pet].Feed(cakes[cake]);
                                            wallet -= cakes[cake].Cost;
                                            cakeHist[pet].Add(tickCount);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"You can't afford {cake}cake.\n");
                                }
                            }
                            else if (pets.Keys.Contains(cake))
                            {
                                int regen = Math.Max(pets[cake].HungerLimit - pets[cake].Hunger, 0);
                                pets[pet].Hunger -= regen;
                                pets[pet].Boredom = 0;
                                Console.WriteLine($"{pets[pet].Name} has eaten {pets[cake].Name} (restored all boredom and {regen} hunger!\n");
                                Console.WriteLine($"{pets[cake].Name} has died. RIP\n");
                                cemetery[pets[cake].Name] = new List<string> { tickCount.ToString(), Methods.JoinWithAnd(pets[cake].Sounds) };
                                pets.Remove(cake);
                            }
                            else
                            {
                                Console.WriteLine("The cake specified doesn't exist.");
                            }
                            break;

                        case "teach":
                            try
                            {
                                pet = action[1];
                            }
                            catch (IndexOutOfRangeException)
                            {
                                Console.WriteLine("I understood you as far as wanting to teach a pet a sound.\n");
                                continue;
                            }
                            if (pets.Keys.Contains(pet))
                            {
                                try
                                {
                                    // This will throw IndexOutOfRangeException if no sound is entered
                                    string test = action[2];
                                    // Pets cannot be taught the same sound twice
                                    if (!pets[pet].Sounds.Contains(string.Join(' ', new ArraySegment<string>(action, 2, action.Count() - 2))))
                                    {
                                        pets[pet].Train(string.Join(' ', new ArraySegment<string>(action, 2, action.Count() - 2)));
                                    }
                                    else
                                    {
                                        Console.WriteLine($"{pets[pet].Name} knows that already.\n");
                                    }
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    Console.WriteLine($"I understood you as far as wanting to teach {pets[pet].Name} a sound.");
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    Console.WriteLine($"I understood you as far as wanting to teach {pets[pet].Name} a sound.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("The pet specified doesn't exist.\n");
                            }
                            break;

                        case "sounds":
                            try
                            {
                                pet = action[1];
                                if (pets.Keys.Contains(pet) && action.Count() == 2)
                                {
                                    // Print out all the sounds the pet has learnt
                                    if (pets[pet].Sounds.Count() > 0)
                                    {
                                        Console.WriteLine($"{pets[pet].Name} knows how to say: {Methods.JoinWithAnd(pets[pet].Sounds)}.\n");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"{pets[pet].Name} hasn't learnt any sounds yet :(\n");
                                    }
                                }
                                else if (pets.Keys.Contains(pet))
                                {
                                    Console.WriteLine($"I am confused as to why you wrote '{string.Join(' ', new ArraySegment<string>(action, 2, action.Count() - 2))}'.");
                                }
                                else
                                {
                                    Console.WriteLine("The specified pet doesn't exist.\n");
                                }
                            }
                            catch (IndexOutOfRangeException)
                            {
                                Console.WriteLine("I understood you as far as wanting to print a list of sounds a pet can make.\n");
                            }
                            break;

                        case "cakes":
                            if (action.Count() == 1)
                            {
                                // Print out all cakes and their info
                                Console.WriteLine("List of available cakes:");
                                foreach (Cake cakeType in cakes.Values)
                                {
                                    Console.WriteLine($"{Methods.Capitalise(cakeType.Type)}cake: costs {cakeType.Cost}, restores {cakeType.Hunger} hunger and {cakeType.Health} health.");
                                }
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Cakes doesn't support secondary commands.\n");
                            }
                            break;

                        case "help":
                            if (action.Count() == 1)
                            {
                                // Print out all available commands
                                Console.WriteLine(@"List of available commands:
teach *pet name* *sound* - teaches specified pet the specified sound
feed *pet name* *cake* - feeds specified pet the specified cake (see cakes)
sounds *pet name* - prints a list of sounds the specified pet can make
cakes - prints a list of all the cakes that can be fed to pets
help - prints this text :). Incidentally this is the only place instructions to
    find these instructions are...
[enter] - advances to the next tick without taking an action
");
                            }
                            else
                            {
                                Console.WriteLine("Help doesn't support secondary commands.\n");
                            }
                            break;

                        default:
                            // If the user enters an invalid command, what do I care?
                            Console.WriteLine("I don't recognise that command.\n");
                            break;
                    }
                }
            }

            // Game over
            Console.WriteLine($@"All your pets have died :(

Well done on lasting {tickCount} ticks!

{names[0]} died on tick {cemetery[names[0]][0]}
    {names[0]} knew how to say {cemetery[names[0]][1]}
{names[1]} died on tick {cemetery[names[1]][0]}
    {names[1]} knew how to say {cemetery[names[1]][1]}
{names[2]} died on tick {cemetery[names[0]][0]}
    {names[2]} knew how to say {cemetery[names[2]][1]}

May they be forever at peace.
");
        }
    }
}
