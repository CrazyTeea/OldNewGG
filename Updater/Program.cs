using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace Updater
{
    class Program
    {
        
        static void DownloadFile(string name)
        {
            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(Client_DownloadFileCompleted);
            client.DownloadFileAsync(new Uri("http://crazytea.xyz/application/"+name), Environment.CurrentDirectory.ToString() + @"/" + name);

        }
        public static void AddFileSecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {

            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Add the FileSystemAccessRule to the security settings.
            fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);

        }
        static void Main(string[] args)
        {
            Console.WriteLine("ждем 5 секунд чтоб все было заебца");
            Thread.Sleep(5000);

            string desktop = Environment.CurrentDirectory + @"\";
            try
            {
                //получаем имя компьютора и пользователя
                WindowsIdentity wi = WindowsIdentity.GetCurrent();
                string user = wi.Name;
               //Console.WriteLine(user);
                // Add the access control entry to the file.
                AddFileSecurity(desktop, @user,FileSystemRights.FullControl, AccessControlType.Allow);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Начато обновление");       
                try
                {
                
                File.SetAttributes("NewGG.exe", FileAttributes.Normal);
                File.Delete("NewGG.exe");
                
                        Console.WriteLine("Удаление чита успешно -> продолжаю скачивать");
                    
                }
                catch (Exception)
                {
                    //  WebClient client = new WebClient();
                    Console.WriteLine("Старая версия чита не найдена -> продолжаю скачивать");
                }
                DownloadFile("NewGG.exe");
            Console.WriteLine("нажми любую клавишу");
            Console.Read();
            Thread.Sleep(1000);

        }

        static void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //то что будет после скачивания файла
            try
            {
                Process proc = new Process();
                proc.StartInfo.WorkingDirectory = Environment.CurrentDirectory + @"\";
                proc.StartInfo.FileName = "NewGG.exe";

                proc.Start();
               // Console.WriteLine(proc.StartInfo.WorkingDirectory + proc.StartInfo.FileName);
                Thread.Sleep(500);
               // Console.Read();
               // Environment.Exit(0);
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
        }
       static void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {       
            Console.Write('.');            
        }
    }
}
