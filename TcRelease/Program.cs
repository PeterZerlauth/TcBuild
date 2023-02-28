using System;
using NDesk.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using TcXae;

namespace TcRelease
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("==============================================================================");
            Console.WriteLine("TcRelease.exe");
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
                Console.WriteLine("Download Test programm");
            }




            solution.Close();
            Console.WriteLine("==============================================================================");
        }
    }
}
