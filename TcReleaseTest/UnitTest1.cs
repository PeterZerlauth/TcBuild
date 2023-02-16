using System;
using TcXae;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;

namespace TcReleaseTest
{
    [TestClass]
    public class UnitTest1
    {
        string solutionFilePath = "";
        string projectName = @"TwinCATProject";
        string LibaryName = @"Library";
        string OutputPath = @"C:\source\repos\Project";
        string Command = @"Build";
        string Install = "False";
        static TcXae.Solution solution = new TcXae.Solution();


        [TestMethod, Timeout(10000)]
        public void TestMethod1()
        {
            string solutionFilePath = Directory.GetCurrentDirectory() + "//resources//TwinCATProject//TwinCATProject.sln";
            Assert.IsTrue(File.Exists(solutionFilePath), "Solution.Exists");
        }

        [TestMethod, Timeout(10000)]
        public void TestMethod2()
        {
            solution = new TcXae.Solution();
            Assert.IsTrue(solution!=null, "Solution");
        }

        [TestMethod, Timeout(10000)]
        public void TestMethod3()
        {
            bool result = solution.Open(solutionFilePath);
            Assert.IsTrue(result, "solution.Open");
        }

        [TestMethod, Timeout(10000)]
        public void TestMethod4()
        {
            bool result = solution.Project.Open(projectName);
            Assert.IsTrue(result, "Project.Open");
        }

        [TestMethod, Timeout(10000)]
        public void TestMethod5()
        {
            bool result = solution.Project.CheckAllObjects(LibaryName);
            Assert.IsTrue(result, "CheckAllObjects");
        }
        [TestMethod, Timeout(10000)]
        public void TestMethod6()
        {
            bool result = solution.Project.BuildLibrary(OutputPath, LibaryName, false);
            Assert.IsTrue(result, "BuildLibrary");
        }
        [TestMethod, Timeout(10000)]
        public void TestMethod7()
        {
            bool result = solution.Project.StartRestartTwinCAT();
            Assert.IsTrue(result, "StartRestartTwinCAT");
        }
        [TestMethod, Timeout(10000)]
        public void TestMethod8()
        {
            bool result = solution.Project.ActivateConfiguration();
            Assert.IsTrue(result, "ActivateConfiguration");
        }
        [TestMethod, Timeout(10000)]
        public void TestMethod9()
        {
            bool result = solution.Project.Save();
            Assert.IsTrue(result, "Project.Save");
        }
        [TestMethod, Timeout(10000)]
        public void TestMethod10()
        {
            bool result = solution.Close();
            Assert.IsTrue(result, "solution.Close()");
            
        }


    }
}