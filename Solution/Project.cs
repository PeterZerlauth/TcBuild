using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCatSysManagerLib;

namespace XAE
{
    public class Project
    {
        private EnvDTE.Solution? Solution = null;
        private ITcSysManager? project = null;

        public Project (EnvDTE.Solution Solution) 
        { 
            this.Solution = Solution;
        }

        public bool Open(string projectName)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(projectName))
            {
               
                MessageFilter.Register();
                if (Solution != null)
                {
                    foreach (EnvDTE.Project Project in Solution.Projects)
                    {
                        if (Project.Name == projectName)
                        {
                            project = (ITcSysManager)Project.Object;
                            MessageFilter.Revoke();
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
                if (project != null)
                {
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
            return false;
        }

        public bool BuildLibrary(string OutputPath, string LibaryName, bool Install)
        {
            if (!String.IsNullOrEmpty(OutputPath) && !String.IsNullOrEmpty(LibaryName))
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
