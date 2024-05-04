using System;
// ALLOW MULTITHREADING
using System.Threading.Tasks;

class Program
{
    static Dictionary<string, string> prompts = new Dictionary<string, string>
    {
        { "greeting", "WELCOME HOME!" },
        { "introduction", "I AM YOUR PET!" },
        { "instructions", "Please, make sure I don't get too hungry!" },
        { "callToAction", "Revive me? Input 'PLEASE!' and press [ENTER] key" },
        { "outro", "G O O D B Y E !\nI WILL MISS YOU!\nThanks for playing! ❤" }
    };
    static Dictionary<string, string> hunger = new Dictionary<string, string>
    {
        { "label", "LEVEL OF H U N G E R : " },
        { "meter0", "[ ][ ][ ][ ][ ]" },
        { "meter1", "[x][ ][ ][ ][ ]" },
        { "meter2", "[x][x][ ][ ][ ]" },
        { "meter3", "[x][x][x][ ][ ]" },
        { "meter4", "[x][x][x][x][ ]" },
        { "meter5", "[x][x][x][x][x]" }
    };
    static Dictionary<string, string> pet = new Dictionary<string, string>
    {
        { "inhale", @"
      _ _ _ 
    /       \
    | -   - | 
    \_ _ _ _/
" },
        { "exhale", @"
      _ _ _ 
    /       \
   |  -   -  |
    \_ _ _ _/
" },
        { "inhaleWideEyed", @"
      _ _ _ 
    /       \
    | O   o | 
    \_ _ _ _/
" },
        { "dead", @"
      _ _ _ 
    /       \
   |  x   X  |
    \_ _ _ _/
" }
    };
static Dictionary<string, string> petTalk = new Dictionary<string, string>
    {
        { "not hungry", "Ahhh.. I'm not hungry at all!" },
        { "getting hungry", "*Belly grumbles*" },
        { "getting hungrier", "*Belly grumbles louder*" },
        { "super hungry", "Uh oh.. I'm super hungry now.." },
        { "starving", "I M S-S-STARVING! HALP!" },
        { "death", "O'NOOOO! I M DED!" }
    };

    static object consoleLock = new object();
    static bool alive = true;
    static bool breathing = false;
    static bool starving = false;
    static int meterIndex = 0;

    static void SlowPrint(string message, int delay)
    {
        for (int i = 0; i < message.Length; i++)
        {
            Console.Write(message[i]);
            System.Threading.Thread.Sleep(delay);
        }
        Console.WriteLine();
    }

    static void RevivePet()
    {
        alive = true;
        starving = false;
        meterIndex = 0;
        timer.Start();
    }

    static System.Timers.Timer timer = new System.Timers.Timer(1000);

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Clear();
        // (1) PROMPTS
        lock (consoleLock)
        {
            Console.SetCursorPosition(0, 2);
            SlowPrint("WELCOME HOME!", 100);
            SlowPrint("I AM YOUR PET!", 100);
            SlowPrint("Please, make sure I don't get too hungry!", 50);
        }

        // (2) HUNGER
        Task.Run(async () =>
        {

            while (true)
            {
                lock (consoleLock)
                {
                    Console.SetCursorPosition(0, 6);
                    Console.Write(hunger["label"]);
                    switch (meterIndex)
                    {
                        case 0:
                            Console.WriteLine(hunger["meter0"]);
                            {
                                Console.SetCursorPosition(0, 13);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, 13);
                                SlowPrint(petTalk["not hungry"], 50);
                            }
                            meterIndex += 1;
                            break;
                        case 1:
                            Console.WriteLine(hunger["meter1"]);
                            {
                                Console.SetCursorPosition(0, 13);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, 13);
                                SlowPrint(petTalk["getting hungry"], 50);
                            }
                            meterIndex += 1;
                            break;
                        case 2:
                            Console.WriteLine(hunger["meter2"]);
                            {
                                Console.SetCursorPosition(0, 13);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, 13);
                                SlowPrint(petTalk["getting hungrier"], 50);
                            }
                            meterIndex += 1;
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(hunger["meter3"]);
                            Console.ResetColor();
                            meterIndex += 1;
                            {
                                Console.SetCursorPosition(0, 13);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, 13);
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                SlowPrint(petTalk["super hungry"], 50);   
                                Console.ResetColor();
                            }
                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine(hunger["meter4"]);
                            Console.ResetColor();
                            starving = true;
                            meterIndex += 1;
                            {
                                Console.SetCursorPosition(0, 13);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, 13);
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                SlowPrint(petTalk["starving"], 50);
                                Console.ResetColor();
                            }
                            break;
                        case 5:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(hunger["meter5"]);
                            Console.ResetColor();
                            alive = !alive;
                            lock (consoleLock)
                            {
                                Console.SetCursorPosition(0, 13);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, 13);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(petTalk["death"]);
                                Console.ResetColor();

                            }
                            Console.SetCursorPosition(0, 15);
                            Console.WriteLine("Revive me? Input 'PLEASE!' and press [ENTER] key");
                            string userInput = Console.ReadLine();
                            if (userInput == "PLEASE!")
                            {
                                RevivePet();
                            }
                            else
                            {
                                Console.WriteLine("ENDING PROGRAM BEcAUSE YOU STARVED ME!");
                            }
                                break;

                    }
                }
                await Task.Delay(2000);
            }
        });

        // (3) PET
        Task.Run(() =>
        {

            timer.Elapsed += (sender, e) =>
            {
                lock (consoleLock)
                {
                    Console.SetCursorPosition(0, 7);
                    if (alive && starving == false)
                    {
                        if (breathing)
                        {
                            Console.Write(petInhale);
                        }
                        else
                        {
                            Console.Write(petExhale);
                        }
                        breathing = !breathing;
                    }
                    else if (alive && starving == true)
                    {
                        if (breathing)
                        {
                            Console.Write(petInhaleWideEyed);
                        }
                        else
                        {
                            Console.Write(petExhale);
                        }
                        breathing = !breathing;
                    }
                    else
                    {
                        Console.SetCursorPosition(0, 7);
                        Console.Write(petDead);
                        timer.Stop();
                    };
                };
            };

            timer.Start();
        });

        while (alive)
        {
            Console.ReadLine();
        }
        
        Console.Clear();
        lock (consoleLock)
        {
            Console.SetCursorPosition(0, 3);
            Console.WriteLine("     G O O D B Y E ! I WILL MISS YOU!");
            Console.WriteLine(" ");
            Console.Write("Thanks for playing! ", 50);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("❤");
            Console.ResetColor();
            Console.WriteLine(" ");
            Console.WriteLine(" ");
        }



    }
}

/* 
  (1) PROMPTS
        Greeting
        Introduction
        Instructions
  (2) HUNGER
        Label | Meter
  (3) PET
        Artwork
        Dialogue
  (4) ACTIONS
        Call To Action
        Instructions
  (5) OUTRO
        Farewell
        Adulation
*/