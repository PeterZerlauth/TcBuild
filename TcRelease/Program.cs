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
            Console.WriteLine("==============================================================================");
            
            string? SolutionFilePath = @"C:\source\repos\TwinCATProject\TwinCATProject.sln";
            string? ProjectName = @"TwinCATProject";
            string? LibaryName = @"Control";
            string? OutputPath = @"C:\source\repos\TwinCATProject";
            string? Command = @"build";
            string? Install = "False";
            string? Help = null;
            OptionSet options = new OptionSet()
                .Add("-c=|c=|Command=", "Build / Release", v => Command = v)
                .Add("-v=|v=|SolutionFilePath=", "The full path to the TwinCAT project (sln-file)", v => SolutionFilePath = v)
                .Add("-p=|p=|ProjectName=", "TwinCAT project name", v => ProjectName = v)
                .Add("-l=|l=|LibaryName=", "TwinCAT Library name", w => LibaryName = w)
                .Add("-o=|o=|OutputPath=", "Output path for twincat library", w => OutputPath = w)
                .Add("-i=|i=|Install=", "Install Library True/False", v => Command = v)
                .Add("-h=|h|?=|Help=", "Help", v => Help = v);
            options.Parse(args);
            if (SolutionFilePath == null)
            {
                
            }
            bool install = false;
            try
            {
                install = Convert.ToBoolean(Install);
            }
            catch            
            {
                install = false;
                Console.WriteLine($"Install set to False");
            }
            


            Helper helper = new Helper();
            if (!helper.Administrator())
            {
                Console.WriteLine($"No administrator rights can cause problems with a build agent");
            }


            TcXae.Solution solution = new TcXae.Solution();
            Console.WriteLine($"Solution Open: {SolutionFilePath}");
            solution.Open(SolutionFilePath);
            Console.WriteLine($"Project Open: {ProjectName}");
            solution.Project.Open(ProjectName);

            if (Command.ToLower() == "build")
            {

                Console.WriteLine($"CheckAllObjects: {solution.Project.CheckAllObjects(LibaryName)}");
                Console.WriteLine($"Generate Boot Project: {LibaryName}");
                solution.Project.GenerateBootProject(LibaryName);
                Console.WriteLine($"Activate Configuration:");
                solution.Project.ActivateConfiguration();
                //Console.WriteLine("StartRestartTwinCAT");
                //solution.Project.StartRestartTwinCAT();
            }
            else
            {
                Console.WriteLine($"CheckAllObjects: {solution.Project.CheckAllObjects(LibaryName)}");
                Console.WriteLine($"BuildLibrary: {OutputPath}/{LibaryName}.library");
                solution.Project.BuildLibrary(OutputPath, LibaryName, install);
            }

            solution.Close();
            Console.WriteLine("==============================================================================");
        }
    }
}
