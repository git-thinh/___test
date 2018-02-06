using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace testUI
{
    class SSH
    {

        public static void t4()
        {
            using (var client = new SshClient("127.0.0.1", 22, "admin", " "))
            {
                client.Connect();

                List<string> commands = new List<string>();
                commands.Add("pwd");
                commands.Add("gcc -v");
                commands.Add("min");

                ShellStream shellStream = client.CreateShellStream("xterm", 80, 24, 800, 600, 1024);

                // Switch to root
                //SwithToRoot("rootpassword", shellStream);

                // Execute commands under root account
                foreach (string command in commands)
                {
                    WriteStream(command, shellStream);

                    string answer = ReadStream(shellStream);
                    int index = answer.IndexOf(System.Environment.NewLine);
                    answer = answer.Substring(index + System.Environment.NewLine.Length);
                    Console.WriteLine("\n\n\n" + answer.Trim());
                }

                client.Disconnect();
            }

            Console.ReadLine();
        }
        private static void WriteStream(string cmd, ShellStream stream)
        {
            stream.WriteLine(cmd + "; echo this-is-the-end");
            while (stream.Length == 0)
                Thread.Sleep(500);
        }

        private static string ReadStream(ShellStream stream)
        {
            StringBuilder result = new StringBuilder();
            long len = stream.Length;

            string line;
            while ((line = stream.ReadLine()) != "this-is-the-end")
                if (!line.Contains("admin@DESKTOP-5LHMNS2") && !line.Contains("]0;~") && !line.Contains("this-is-the-end"))
                    result.AppendLine(line);

            string s = result.ToString();
            int lenText = result.Length;

            return s;
        }
    }
}
