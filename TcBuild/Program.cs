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
            Console.WriteLine($"CheckAllObjects: {LibaryName} {solution.Project.CheckAllObjects(LibaryName)}");
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

                client.Connect(solution.Project.NetId, 851);
                var state = client.ReadValue<ushort>("MAIN.fbTestsuites.eState");
                if (state == 1) 
                {
                    client.WriteValue<string>("MAIN.fbTestsuites.sFilePathName", OutputPath);
                    client.WriteValue<ushort>("MAIN.fbTestsuites.eState", 2);
                }
                Task.Delay(100).Wait();
                while (client.ReadValue<ushort>("MAIN.fbTestsuites.eState") != 1)
                {
                    Task.Delay(100).Wait();
                }


                Console.WriteLine("");
                Task.Delay(2000).Wait();

            }

            solution.Close();
            Console.WriteLine("==============================================================================");
        }
    }
}
