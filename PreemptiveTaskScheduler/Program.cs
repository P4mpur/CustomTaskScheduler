using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak1.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            const int numOfThreads = 2;
            const int oneSecondDelayInMilliseconds = 1000;

            CustomTaskScheduler TaskScheduler = new CustomTaskScheduler(numOfThreads, CustomTaskScheduler.Mode.Preemptive); 
            CustomTaskScheduler.MyToken tokenForMyFunction = TaskScheduler.CreateMyToken();

            void print()
            {
                if (tokenForMyFunction.IsCanceled) return;
         
           

                for (int i=0; i<5; i++)
                {
                    while (tokenForMyFunction.isOnWait)
                    {
                        tokenForMyFunction.Handler.WaitOne();
                    }
                    Console.WriteLine($"Ja sam zadatak 1 i izvrsavam se {i}");
                    Task.Delay(oneSecondDelayInMilliseconds).Wait();

                    if (tokenForMyFunction.IsCanceled)
                    {
                        return;
                    }
                }
                if (tokenForMyFunction.IsCanceled)
                {
                    return;
                }   
                
            }              
            TaskScheduler.ScheduleTask(print, tokenForMyFunction , 2, 10);


        

             CustomTaskScheduler.MyToken tokenForMyFunction2 = TaskScheduler.CreateMyToken(); 
              void print2()
              {
                  for (int i = 0; i < 10; i++)
                  {

                      while(tokenForMyFunction2.isOnWait)
                      {
                        Console.WriteLine("Zadatak 1 WaitONe()");
                        tokenForMyFunction2.Handler.WaitOne();
                      }

                      Console.WriteLine($"Ja sam zadatak 2 i izvrsavam se {i}");
                      Task.Delay(oneSecondDelayInMilliseconds).Wait();

                      if (tokenForMyFunction2.IsCanceled)
                      {
                          break;
                      }
                  }

              }
              TaskScheduler.ScheduleTask(print2, tokenForMyFunction2,3 , 5 );


              CustomTaskScheduler.MyToken tokenForMyFunction3 = TaskScheduler.CreateMyToken(); //dobija token za koristenje
              void print3()
              {
                  for (int i = 0; i < 10; i++)
                  {

                      while (tokenForMyFunction3.isOnWait)
                      {
                        Console.WriteLine("Zadatak 3 WaitONe()");
                          tokenForMyFunction3.Handler.WaitOne();
                      }

                      Console.WriteLine($"Ja sam zadatak 3 i izvrsavam se {i}");
                      Task.Delay(oneSecondDelayInMilliseconds).Wait();

                      if (tokenForMyFunction3.IsCanceled)
                      {
                          break;
                      }
                  }

              }
              TaskScheduler.ScheduleTask(print3, tokenForMyFunction3, 1, 5 );


              CustomTaskScheduler.MyToken tokenForMyFunction4 = TaskScheduler.CreateMyToken(); //dobija token za koristenje
              void print4()
              {
                  for (int i = 0; i < 10; i++)
                  {

                      while (tokenForMyFunction4.isOnWait)
                      {
                        Console.WriteLine("Zadatak 4 WaitONe()");
                          tokenForMyFunction4.Handler.WaitOne();
                      }

                      Console.WriteLine($"Ja sam zadatak 4 i izvrsavam se {i}");
                      Task.Delay(oneSecondDelayInMilliseconds).Wait();

                      if (tokenForMyFunction4.IsCanceled)
                      {
                          break;
                      }
                  }

              }
              TaskScheduler.ScheduleTask(print4, tokenForMyFunction4, 4, 5 );

              CustomTaskScheduler.MyToken tokenForMyFunction5 = TaskScheduler.CreateMyToken(); //dobija token za koristenje
              void print5()
              {
                  for (int i = 0; i < 10; i++)
                  {

                      while (tokenForMyFunction5.isOnWait)
                      {
                        Console.WriteLine("Zadatak 5 WaitONe()");
                          tokenForMyFunction5.Handler.WaitOne();
                      }

                      Console.WriteLine($"Ja sam zadatak 5 i izvrsavam se {i}");
                      Task.Delay(oneSecondDelayInMilliseconds).Wait();

                      if (tokenForMyFunction5.IsCanceled)
                      {
                          break;
                      }
                  }

              }
              TaskScheduler.ScheduleTask(print5, tokenForMyFunction5, 1, 4 );

              CustomTaskScheduler.MyToken tokenForMyFunction6 = TaskScheduler.CreateMyToken(); //dobija token za koristenje
              void print6()
              {
                  for (int i = 0; i < 10; i++)
                  {

                      while (tokenForMyFunction6.isOnWait)
                      {
                        Console.WriteLine("Zadatak 6 WaitONe()");
                          tokenForMyFunction6.Handler.WaitOne();
                      }

                      Console.WriteLine($"Ja sam zadatak 6 i izvrsavam se {i}");
                      Task.Delay(oneSecondDelayInMilliseconds).Wait();

                      if (tokenForMyFunction6.IsCanceled)
                      {
                          break;
                      }
                  }

              }
              TaskScheduler.ScheduleTask(print6, tokenForMyFunction6, 1, 4 );

              while (TaskScheduler.CurrentTaskCount > 0)
              {
              //    Console.WriteLine("Preostalo:" + CustomTaskScheduler.CurrentTaskCount);
                  Task.Delay(oneSecondDelayInMilliseconds).Wait();
              }


              Console.WriteLine("Done.");
        }
    }
}
