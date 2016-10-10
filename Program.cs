using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

/*
Practice: Develop a stopwatch console application using threading.
*/

namespace TwelfthTask
{
    class Program
    {
        public static Mutex mutexObj = new Mutex();
        public static bool pause = false;
        public static bool alive = false;

        public static void Stopwatch()
        {
            int hour = 0, min = 0, sec = 0, msec = 0;
            while (alive)
            {
                Console.SetCursorPosition(0, 5);
                Console.WriteLine("Time: " + hour.ToString().PadLeft(2) + ":" + min.ToString().PadLeft(2) + ":" + sec.ToString().PadLeft(2) + ":" + msec.ToString().PadLeft(2));
                if (pause)
                {
                    mutexObj.WaitOne();
                    mutexObj.ReleaseMutex();
                }
                msec++;
                if (msec == 100)
                {
                    sec++;
                    msec = 0;
                    if (sec == 60)
                    {
                        min++;
                        sec = 0;
                        if (min == 60)
                        {
                            hour++;
                            min = 0;
                            if (hour == 24)
                                hour = 0;
                        }
                    }
                }
                Thread.Sleep(10);     
            }
        }

        static void Main(string[] args)
        {
            Thread th = new Thread(new ThreadStart(Stopwatch));
            
            int choice = 0;

            while (choice != 4)
            {
                Console.Clear();
                Console.WriteLine("Menu: \n1. Start. \n2. Pause. \n3. Restart. \n4. Exit.");
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException) { }
                switch (choice)
                {
                    case 1:
                        if (!pause)
                        {
                            if (!alive)
                            {
                                alive = true;
                                th.Start();
                            }
                        }
                        else
                        {
                            pause = false;
                            mutexObj.ReleaseMutex();
                        }
                        break;
                    case 2:
                        if (!pause)
                        {
                            mutexObj.WaitOne();
                            pause = true;
                        }
                        break;
                    case 3:
                        if (alive)
                        {
                            alive = false;
                            if (pause)
                            {
                                pause = false;
                                mutexObj.ReleaseMutex();
                            }
                            th.Join();
                        }
                        th = new Thread(new ThreadStart(Stopwatch));
                        break;
                    case 4:
                        if (alive)
                        {
                            alive = false;
                            if (pause)
                            {
                                pause = false;
                                mutexObj.ReleaseMutex();
                            }
                            th.Join();
                        }
                        break;
                }
            }
        }
    }
}
