using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCatSysManagerLib;

namespace TcBuild
{
    internal class Project
    {
        private EnvDTE.Solution? Solution = null;
        private ITcSysManager? project = null;

        public Project (EnvDTE.Solution Solution) 
        { 
            this.Solution = Solution;
        }

        public bool Open(string projectName)
        {
            MessageFilter.Register();
            if (Solution != null )
            {
                foreach (EnvDTE.Project Project in Solution.Projects)
                {
                    if (Project.Name == projectName)
                    {
                        project = (ITcSysManager)Project.Object;
                        MessageFilter.Revoke();
                        return true;
                    }
                }
            }
            MessageFilter.Revoke();
            return false;
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
            if (project!= null) {
                ITcSmTreeItem treeItem = project.LookupTreeItem($"TIPC^{LibaryName}^{LibaryName} Project");
                ITcPlcIECProject2 iecProject = (ITcPlcIECProject2)treeItem;
                if (iecProject != null)
                {
                    return iecProject.CheckAllObjects();
                }
                return false;
            }
            return false;
        }

        public bool BuildLibrary(string OutputPath, string LibaryName, bool Install)
        {
            string outputPath = Path.GetFullPath(OutputPath + "\\" + LibaryName + ".library").Replace("\\", "/");
            Directory.Exists(outputPath);
            if (project != null)
            {
                ITcSmTreeItem treeItem = project.LookupTreeItem($"TIPC^{LibaryName}^{LibaryName} Project");
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

        public bool GenerateBootProject(string PlcName)
        {
            if(project!= null) 
            {
                ITcSmTreeItem treeItem = project.LookupTreeItem($"TIPC^{PlcName}^{PlcName} Project");
                project.StartRestartTwinCAT();
                ITcPlcProject2 plcProject = (ITcPlcProject2)treeItem;
                plcProject.BootProjectAutostart = true;
                plcProject.GenerateBootProject();
                return true;
            }
            return false;
        }

        public bool StartRestartTwinCAT()
        {
            if (project!= null)
            {
                project.StartRestartTwinCAT();
                return project.IsTwinCATStarted();
            }
            return false;
         
        }
        public bool ActivateConfiguration()
        {
            if (project != null)
            {
                project.ActivateConfiguration();
                return true;
            }
            return false;
        }
               
    }
}
