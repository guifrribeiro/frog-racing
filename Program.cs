class Program
{
  private static readonly int TotalJumps = 15;
  private static readonly int TotalFrogs = 7;
  private static int[] jumpCount;
  private static int[] finishingPlace;
  private static int place;
  private static object raceLock = new object();
  private static Random random = new Random();

  static void Main(string[] args)
  {
    place = 0;
    finishingPlace = new int[TotalFrogs];
    jumpCount = new int[TotalFrogs];

    for (int index = 0; index < TotalFrogs; index++)
    {
      jumpCount[index] = 0;
      finishingPlace[index] = 0;
    }

    PrintRace();

    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("Aperte qualquer botão para iniciar a corrida!");

    Console.ReadKey(false);

    List<Thread> threads = new List<Thread>();
    for (int index = 0; index < TotalFrogs; index++)
    {
      Thread thread = new Thread(HandleSingleFrog);
      thread.Start(index + 1);
      threads.Add(thread);
    }

    foreach (Thread thread in threads)
    {
      thread.Join();
    }

    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("A corrida acabou!");
    Console.ReadKey(false);
  }

  public static int GetPlace()
  {
    lock (raceLock)
    {
      place++;
      return place;
    }
  }

  public static void HandleSingleFrog(Object numberAsObject)
  {
    int number = (int)numberAsObject;

    for (int jump = 0; jump < TotalJumps; jump++)
    {
      Thread.Sleep(random.Next(3000) + 500);
      jumpCount[number - 1]++;
      PrintRace();
    }

    finishingPlace[number - 1] = GetPlace();
    PrintRace();
  }

  public static void PrintRace()
  {
    lock (raceLock)
    {
      Console.Clear();

      Console.ForegroundColor = ConsoleColor.Blue;
      Console.WriteLine("CORRIDA");
      Console.WriteLine("       DE SAPOS!");

      Console.ForegroundColor = ConsoleColor.White;
      for (int frog = 0; frog < TotalFrogs; frog++)
      {
        if (finishingPlace[frog] > 0)
        {
          Console.Write(finishingPlace[frog] + " ");
        }
        else
        {
          Console.Write("? ");
        }
      }

      Console.WriteLine();

      for (int row = 0; row < TotalJumps + 1; row++)
      {
        for (int frog = 0; frog < TotalFrogs; frog++)
        {
          if (jumpCount[frog] == TotalJumps - row)
          {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Ω ");
          }
          else
          {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(". ");
          }
        }

        Console.WriteLine();
      }
    }
  }
}