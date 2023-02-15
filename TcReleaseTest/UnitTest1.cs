using System;
using XAE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;

namespace TcReleaseTest
{
    [TestClass]
    public class UnitTest1
    {
        string SolutionFilePath = @"C:\source\repos\Project\Project.sln";
        string ProjectName = @"Project";
        string LibaryName = @"Library";
        string OutputPath = @"C:\source\repos\Project";
        string Command = @"Build";
        string Install = "False";
        static XAE.Solution solution = new XAE.Solution();


        [TestMethod, Timeout(10000)]
        public void TestMethod1()
        {
            Assert.IsTrue(solution!=null, "solution.Open");
        }

        [TestMethod, Timeout(10000)]
        public void TestMethod2()
        {
            solution = new XAE.Solution();
            string currentDirectory = Directory.GetCurrentDirectory();
            bool result = solution.Open(SolutionFilePath);
            Assert.IsTrue(result, "solution.Open");
        }

        [TestMethod, Timeout(10000)]
        public void TestMethod3()
        {
            bool result = solution.Project.Open(ProjectName);
            Assert.IsTrue(result, "Project.Open");
        }

        [TestMethod, Timeout(10000)]
        public void TestMethod4()
        {
            bool result = solution.Project.CheckAllObjects(LibaryName);
            Assert.IsTrue(result, "CheckAllObjects");
        }
        [TestMethod, Timeout(10000)]
        public void TestMethod5()
        {
            bool result = solution.Project.BuildLibrary(OutputPath, LibaryName, false);
            Assert.IsTrue(result, "BuildLibrary");
        }
        [TestMethod, Timeout(10000)]
        public void TestMethod6()
        {
            bool result = solution.Project.StartRestartTwinCAT();
            Assert.IsTrue(result, "StartRestartTwinCAT");
        }
        [TestMethod, Timeout(10000)]
        public void TestMethod7()
        {
            bool result = solution.Project.ActivateConfiguration();
            Assert.IsTrue(result, "ActivateConfiguration");
        }
        [TestMethod, Timeout(10000)]
        public void TestMethod8()
        {
            bool result = solution.Project.Save();
            Assert.IsTrue(result, "Project.Save");
        }
        [TestMethod, Timeout(10000)]
        public void TestMethod9()
        {
            bool result = solution.Close();
            Assert.IsTrue(result, "solution.Close()");
            
        }


    }
}