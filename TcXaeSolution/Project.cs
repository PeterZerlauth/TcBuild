using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCatSysManagerLib;

namespace TcXae
{
    public class Project
    {
        private EnvDTE.Solution Solution;
        private ITcSysManager15 systemManager;

        public Project (EnvDTE.Solution Solution) 
        { 
            this.Solution = Solution;
        }

        public bool Open(string projectName)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(projectName))
            {
                if (Solution != null)
                {
                    foreach (EnvDTE.Project Project in Solution.Projects)
                    {
                        if (Project.Name == projectName)
                        {
                            systemManager = (ITcSysManager15)Project.Object;

                            result =  true;
                        }
                    }
                }
            }
            return result;
        }

        public void Delete()
        {
            if (Solution != null)
            {
                foreach (EnvDTE.Project Project in Solution.Projects)
                {
                    Project.Delete();
                }
            }
        }

        public bool Save()
        {
            if (Solution != null)
            {
                foreach (EnvDTE.Project Project in Solution.Projects)
                {
                    Project.Save();
                }
            }
            return true;
        }

        public bool Save(string projectName, string newFileName)
        {
            if (Solution != null)
            {
                foreach (EnvDTE.Project Project in Solution.Projects)
                {
                    if (Project.Name == projectName)
                    {
                        Project.SaveAs(newFileName);
                        return Project.Saved;
                    }
                }
            }
            return false;
        }

        

        public bool CheckAllObjects(string LibaryName)
        {
            if (!String.IsNullOrEmpty(LibaryName))
            {
                if (systemManager != null)
                {
                    ITcSmTreeItem treeItem = systemManager.LookupTreeItem($"TIPC^{LibaryName}^{LibaryName} Project");
                    ITcPlcIECProject2 iecProject = (ITcPlcIECProject2)treeItem;
                    if (iecProject != null)
                    {
                        return iecProject.CheckAllObjects();
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        public bool BuildLibrary(string OutputPath, string LibaryName, bool Install)
        {
            if (!String.IsNullOrEmpty(OutputPath) && !String.IsNullOrEmpty(LibaryName))
            {
                string outputPath = Path.GetFullPath(OutputPath + "\\" + LibaryName + ".library").Replace("\\", "/");
                Directory.Exists(outputPath);
                if (systemManager != null)
                {
                    ITcSmTreeItem treeItem = systemManager.LookupTreeItem($"TIPC^{LibaryName}^{LibaryName} Project");
                    ITcPlcIECProject2 iecProject = (ITcPlcIECProject2)treeItem;
                    if (iecProject != null)
                    {
                        iecProject.SaveAsLibrary(outputPath, Install);
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        public bool GenerateBootProject(string PlcName)
        {
            if(systemManager!= null) 
            {

                ITcSmTreeItem treeItem = systemManager.LookupTreeItem($"TIPC^{PlcName}");
                ITcPlcProject iecProjectRoot = (ITcPlcProject)treeItem;
                iecProjectRoot.BootProjectAutostart = true;
                iecProjectRoot.GenerateBootProject(true);
                return true;
            }
            return false;
        }

        public bool StartRestartTwinCAT()
        {
            if (systemManager!= null)
            {
                systemManager.StartRestartTwinCAT();
                return systemManager.IsTwinCATStarted();
            }
            return false;
         
        }
        public bool ActivateConfiguration()
        {
            if (systemManager != null)
            {
                systemManager.ActivateConfiguration();
                return true;
            }
            return false;
        }

        public string NetId
        {
            get => ((ITcSysManager2)systemManager).GetTargetNetId();
            set => ((ITcSysManager2)systemManager).SetTargetNetId(value);
        }



    }
}
