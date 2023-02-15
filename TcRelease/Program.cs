using System;
using NDesk.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using TcBuild;
using tcBuild;

namespace TcRelease
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("TcRelease");
            Console.WriteLine("A Twincat 3 library release tool");
            string SolutionFilePath = null;
            string ProjectName = null;
            string LibaryName = null;
            string OutputPath = null;
            OptionSet options = new OptionSet()
                .Add("-v=|v=|SolutionFilePath=", "The full path to the TwinCAT project (sln-file)", v => SolutionFilePath = v)
                .Add("-p=|p=|ProjectName=", "TwinCAT project name", v => ProjectName = v)
                .Add("-l=|l=|LibaryName=", "TwinCAT Library name", w => LibaryName = w)
                .Add("-o=|o=|OutputPath=", "Output path for twincat library", w => OutputPath = w);

            options.Parse(args);
            if (SolutionFilePath == null)
            {
                SolutionFilePath = @"C:\source\repos\Project\Project.sln";
                ProjectName = @"Project";
                LibaryName = @"Library";
                OutputPath = @"C:\source\repos\Project";
            }

            Helper helper = new Helper();
            Console.WriteLine($"Administrator: {helper.Administrator()}");

            Solution solution = new Solution();
            Console.WriteLine("Solution Open");
            solution.Open(SolutionFilePath);
            Console.WriteLine("Solution Project Open");
            solution.Project.Open(ProjectName);
            Console.WriteLine("CheckAllObjects");
            solution.Project.CheckAllObjects(LibaryName);
            Console.WriteLine("BuildLibrary");
            solution.Project.BuildLibrary(OutputPath, LibaryName, false);
            Console.WriteLine("StartRestartTwinCAT");
            solution.Project.StartRestartTwinCAT();


        }
    }
}
