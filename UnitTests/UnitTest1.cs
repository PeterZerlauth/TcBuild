using System.IO.Compression;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        static TcXae.Solution solution = solution = new TcXae.Solution();


        [TestMethod]
        public void TestMethod01()
        {
            ZipFile.ExtractToDirectory(Directory.GetCurrentDirectory() + "\\resources\\TwinCATProject.zip", Directory.GetCurrentDirectory() + "\\resources\\TwinCATProject", true);
            string solutionFilePath = Directory.GetCurrentDirectory() + "\\resources\\TwinCATProject\\TwinCATProject.sln";
            Assert.IsTrue(File.Exists(solutionFilePath), "Solution.Exists");
        }

        [TestMethod]
        public void TestMethod02()
        {
            string solutionFilePath = Directory.GetCurrentDirectory() + "\\resources\\TwinCATProject\\TwinCATProject.sln";
            bool result = solution.Open(solutionFilePath);
            Assert.IsTrue(result, "solution.Open");
        }


        [TestMethod]
        public void TestMethod03()
        {
            bool result = solution.Project.Open("TwinCATProject");
            Assert.IsTrue(result, "Project.Open");
        }

        [TestMethod]
        public void TestMethod04()
        {
            bool result = solution.Build();
            Assert.IsTrue(result, "Project.Open");
        }

        [TestMethod]
        public void TestMethod05()
        {
            bool result = solution.Project.CheckAllObjects("Library");
            Assert.IsTrue(result, "CheckAllObjects");
        }
        [TestMethod]
        public void TestMethod06()
        {
            string OutputPath = Directory.GetCurrentDirectory();
            bool result = solution.Project.BuildLibrary(OutputPath, "Library", false);
            Assert.IsTrue(result, "BuildLibrary");
        }


        [TestMethod]
        public void TestMethod07()
        {
            string result = solution.Project.NetId;
            Assert.IsTrue(result != "", "Get NetId");
        }

        [TestMethod]
        public void TestMethod08()
        {
            bool result = solution.Project.ActivateConfiguration();
            Assert.IsTrue(result, "ActivateConfiguration");
        }

        [TestMethod]
        public void TestMethod09()
        {
            bool result = solution.Project.GenerateBootProject("Testing");
            Assert.IsTrue(result, "GenerateBootProject");
        }


        [TestMethod]
        public void TestMethod10()
        {
            bool result = solution.Project.StartRestartTwinCAT();
            Assert.IsTrue(result, "StartRestartTwinCAT");
        }

        [TestMethod]
        public void TestMethod11()
        {
            bool result = solution.Project.Save();
            Assert.IsTrue(result, "Project.Save");
        }
        [TestMethod]
        public void TestMethod12()
        {
            bool result = solution.Close();
            Assert.IsTrue(result, "solution.Close()");

        }
    }
}