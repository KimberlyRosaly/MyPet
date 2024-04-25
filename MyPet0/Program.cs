using System;
// ALLOW MULTITHREADING
using System.Threading.Tasks;

class Program
{
    static string petInhale = @"
      _ _ _ 
    /       \
    | -   - | 
    \_ _ _ _/
";
    static string petExhale = @"
      _ _ _ 
    /       \
   |  -   -  |
    \_ _ _ _/
"; static string petDead = @"
      _ _ _ 
    /       \
   |  x   X  |
    \_ _ _ _/
";
    static string hungerMeter0 = "[ ][ ][ ][ ][ ]";
    static string hungerMeter1 = "[x][ ][ ][ ][ ]";
    static string hungerMeter2 = "[x][x][ ][ ][ ]";
    static string hungerMeter3 = "[x][x][x][ ][ ]";
    static string hungerMeter4 = "[x][x][x][x][ ]";
    static string hungerMeter5 = "[x][x][x][x][x]";

    static object consoleLock = new object();
    static bool alive = true;
    static bool breathing = false;

    static void SlowPrint(string message, int delay)
    {
        for (int i = 0; i < message.Length; i++)
        {
            Console.Write(message[i]);
            System.Threading.Thread.Sleep(delay);
        }
        Console.WriteLine();
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
            // CREATE DEFAULT VALUE | FLAG VALUE | READ ALONE INDICES FOR METER ACCESS
            int meterIndex = 0;
            while (true)
            {
                lock (consoleLock)
                {
                    Console.SetCursorPosition(0, 6);
                    Console.Write("LEVEL OF H U N G E R : ");
                    switch (meterIndex)
                    {
                        case 0:
                            Console.WriteLine(hungerMeter0);
                            {
                                Console.SetCursorPosition(0, 13);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, 13);
                                SlowPrint("Ahhh.. I'm not hungry at all!", 50);
                            }
                            meterIndex += 1;
                            break;
                        case 1:
                            Console.WriteLine(hungerMeter1);
                            {
                                Console.SetCursorPosition(0, 13);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, 13);
                                SlowPrint("*Belly grumble*", 50);
                            }
                            meterIndex += 1;
                            break;
                        case 2:
                            Console.WriteLine(hungerMeter2);
                            {
                                Console.SetCursorPosition(0, 13);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, 13);
                                SlowPrint("*Belly grumbles louder*", 50);
                            }
                            meterIndex += 1;
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(hungerMeter3);
                            Console.ResetColor();
                            meterIndex += 1;
                            {
                                Console.SetCursorPosition(0, 13);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, 13);
                                SlowPrint("Uh oh..I'm super hungry now..", 50);                                
                            }
                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine(hungerMeter4);
                            Console.ResetColor();
                            meterIndex += 1;
                            {
                                Console.SetCursorPosition(0, 13);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, 13);
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                SlowPrint("I M SSSSTARVING! HALP!", 50);
                                Console.ResetColor();
                            }
                            break;
                        case 5:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(hungerMeter5);
                            Console.ResetColor();
                            alive = !alive;
                            lock (consoleLock)
                            {
                                Console.SetCursorPosition(0, 13);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, 13);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("O'NOOOO! I M DED!");
                                Console.ResetColor();

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
                    if (alive)
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

        Console.ReadLine();
        Console.Clear();
        lock (consoleLock)
        {
            Console.SetCursorPosition(0, 3);
            Console.WriteLine("     G O O D B Y E ! I WILL MISS YOU!");
            Console.Write("Thanks for playing! ", 50);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("❤");
            Console.ResetColor();
            Console.WriteLine(" ");
            Console.WriteLine(" ");
        }



    }
}
