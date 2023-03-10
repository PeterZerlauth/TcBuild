using System;
using NDesk.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using TcXae;
using TwinCAT;
using TwinCAT.Ads;
using static System.Net.Mime.MediaTypeNames;
using TwinCAT.Ads.TypeSystem;

namespace TcRelease
{
    class Program
    {
        
        static void Main(string[] args)
        {

            AdsClient client = new AdsClient();
            Console.WriteLine("==============================================================================");
            Console.WriteLine("TcBuild.exe");
            Console.WriteLine("A Twincat 3 library release tool");
            string? SolutionFilePath = @"C:\source\repos\TcRelease\TcReleaseTest\bin\Debug\net6.0\resources\TwinCATProject.sln";
            string? ProjectName = @"TwinCATProject";
            string? LibaryName = @"Library";
            string? OutputPath = @"C:\source\repos\Project";

            string? Help = null;
            OptionSet options = new OptionSet()
                .Add("-v=|v=|SolutionFilePath=", "The full path to the TwinCAT project (sln-file)", v => SolutionFilePath = v)
                .Add("-p=|p=|ProjectName=", "TwinCAT project name", v => ProjectName = v)
                .Add("-l=|l=|LibaryName=", "TwinCAT Library name", l => LibaryName = l)
                .Add("-o=|o=|OutputPath=", "Output path for twincat library", l => OutputPath = l)
                .Add("-h=|h|?=|Help=", "Help", v => Help = v);
            options.Parse(args);
            Console.WriteLine("==============================================================================");


            // Build
            TcXae.Solution solution = new TcXae.Solution();
            Console.WriteLine($"Solution Open: {SolutionFilePath} {solution.Open(SolutionFilePath)}");
            Console.WriteLine($"Project Open: {ProjectName} {solution.Project.Open(ProjectName)}");
            bool state = solution.Project.CheckAllObjects(LibaryName);
            Console.WriteLine($"CheckAllObjects: {LibaryName} {state}");
            if (!state )
            {
                solution.Close();
            }
            Console.WriteLine($"BuildLibrary: {OutputPath}\\{LibaryName}.library");
            solution.Project.BuildLibrary(OutputPath, LibaryName, true);
            if (solution.Project.Contains("Testing"))
            {
                // Run
                Console.WriteLine("Testing project found");
                Console.WriteLine($"GenerateBootProject: {solution.Project.GenerateBootProject("Testing")}");
                Console.WriteLine($"ActivateConfiguration: {solution.Project.ActivateConfiguration()}");
                Console.WriteLine($"StartRestartTwinCAT: {solution.Project.StartRestartTwinCAT()}");

                Console.Write("Wait for Twincat");
                while (!solution.Project.IsTwinCATStarted())
                {
                    Console.Write(".");
                }
                Console.WriteLine("");

                string netid = solution.Project.NetId;
                int port = 851;
                Console.WriteLine($"Connect to {netid} {port.ToString()}");
                client.Connect(solution.Project.NetId, 851);

                bool IsConnected = client.IsConnected;
                
                if (!IsConnected)
                {
                    Console.WriteLine($"client is connected: {IsConnected}");
                }

                E_State value = client.ReadValue<E_State>("MAIN.fbTestsuites.eState");
                if (value == E_State.Stopped) 
                {
                    client.WriteValue<string>("MAIN.fbTestsuites.sFilePathName", OutputPath + "\\report.xml");
                    client.WriteValue<E_State>("MAIN.fbTestsuites.eRequest", E_State.Started);
                }
                Task.Delay(100).Wait();
                value = client.ReadValue<E_State>("MAIN.fbTestsuites.eState");
                while (value != E_State.Stopped)
                {
                    Console.Write(".");
                    value = client.ReadValue<E_State>("MAIN.fbTestsuites.eState");
                    Task.Delay(100).Wait();
                }

                Console.WriteLine("");
                Task.Delay(2000).Wait();

            }

            solution.Close();
            Console.WriteLine("==============================================================================");
        }
        enum E_State : ushort
        {
            Init = 0,
            Starting = 2,
            Started = 3,
            Stopping = 4,
            Aborting = 5,
            Aborted = 6,
            Reset = 7,
            Stopped = 1

        }
    }

 
}
