using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace testUI
{
    class SSH1111
    { 

        public static void t5()
        {
            using (var client = new SshClient("127.0.0.1", 22, "admin", " "))
            {
                client.Connect();

                var command = client.CreateCommand("gcc -v");
                var asyncExecute = command.BeginExecute();
                command.OutputStream.CopyTo(Console.OpenStandardOutput());
                command.EndExecute(asyncExecute);

                client.Disconnect();
            }

            Console.ReadLine();
        }

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

        public static void t3()
        {
            // Création de la connexion SSH
            SshClient ssh = new SshClient("127.0.0.1", 22, "admin", " ");
            ssh.Connect();
            // Création des flux du terminal
            //ShellStream steam = ssh.CreateShellStream("term", 80, 24, 800, 600, 1024);
            System.Threading.Thread.Sleep(3000);
            ShellStream steam = ssh.CreateShellStream("xterm", 80, 24, 800, 600, 1024);
            //ShellStream steam = ssh.CreateShellStream("term", 0, 0, 0, 0, 1024);
            StreamReader reader = new StreamReader(steam);
            StreamWriter writer = new StreamWriter(steam);
            writer.AutoFlush = true;

            // Permet de passer le motd
            while (steam.Length == 0)
            {
                System.Threading.Thread.Sleep(500);
            }
            string line = reader.ReadLine();
            while (line != null)
            {
                line = reader.ReadLine();
                Console.WriteLine(line);
            }

            // Envoie de la commande
            writer.WriteLine("gcc -v");
            while (steam.Length == 0)
            {
                System.Threading.Thread.Sleep(500);
            }

            // Affiche le résultat de la commande
            line = reader.ReadLine();
            Console.WriteLine(line);
            while (line != null)
            {
                Console.WriteLine(line);
                System.Threading.Thread.Sleep(250);
                line = reader.ReadLine();
            }

            // Ferme la connexion SSH
            ssh.Disconnect();
            Console.ReadLine();
        }

        public static void t2()
        {
            // initialize client and connect like you normally would
            //var client = new SshClient("127.0.0.1", 55555, "admin", " ");
            //var client = new SshClient("127.0.0.1", 55555, "admin", " ");
            //client.Connect();

            // quick way to use ist, but not best practice - SshCommand is not Disposed, ExitStatus not checked...
            //Console.WriteLine(client.CreateCommand("ls -l").Execute());

            //Console.WriteLine(client.CreateCommand("gcc -v").Execute());

            //// await a directory listing
            //var listing = await client.ListDirectoryAsync(".");

            //// await a file upload
            //using (var localStream = File.OpenRead("path_to_local_file"))
            //{
            //    await client.UploadAsync(localStream, "upload_path");
            //}

            // disconnect like you normally would
            //client.Disconnect();
        }

        public static void t1()
        {
            string host = "127.0.0.1";
            string user = "admin";
            string pass = " ";

            Action runCommand = () =>
            {
                SshClient client = new SshClient(host, 55555, user, pass);
                try
                {
                    client.Connect();
                    //var terminal = client.RunCommand("/bin/run.sh");

                    Thread.Sleep(3000);

                    var terminal = client.RunCommand("gcc -v");

                    Console.WriteLine(terminal.Result);
                }
                finally
                {
                    client.Disconnect();
                    client.Dispose();
                }
            };

            ThreadPool.QueueUserWorkItem(x => runCommand());
        }
    }
}
