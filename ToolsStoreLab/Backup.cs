using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows.Forms;
using ToolsStoreLab.Models;

namespace ToolsStoreLab
{
    public class Backup
    {
        private ToolsStoreLabContext _context = new ToolsStoreLabContext();

        private string handArchivePath = "D:\\Instruments\\BackupDb";
        private string autoArchivePath = "D:\\Instruments\\BackupDb";
        private string handArchiveName = "HandBackup.bak";
        private string autoArchiveName = "Backup.bak";
        private string connectionString;       

        public string HandArchivePath
        {
            get { return handArchivePath; }
        }
        public string AutoArchivePath
        {
            get { return autoArchivePath; }
        }
        public string HandArchiveName
        {
            get { return handArchiveName; }
        }
        public string AutoArchiveName
        {
            get { return autoArchiveName; }
        }

        public Backup()
        {
            connectionString = _context.Database.GetConnectionString();//= "Data Source=(local);Initial Catalog=ToolsStoreLab;Integrated Security=True;TrustServerCertificate=True;";
        }

        public void MakeBackup(string archivePath, string archiveName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"BACKUP DATABASE ToolsStoreLab TO DISK = '{archivePath + "\\" + archiveName}'";

                command.ExecuteNonQuery();               
            }          
        }

        public void RestoreBackup()
        {          
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    string commandStr1 = "Alter Database [ToolsStoreLab] Set SINGLE_USER With Rollback Immediate";
                    SqlCommand command1 = new SqlCommand(commandStr1, connection);
                    command1.ExecuteNonQuery();

                    string commandStr2 = $"Use Master Restore Database [ToolsStoreLab] From Disk = '{handArchivePath + "\\" + handArchiveName}' With Replace";
                    SqlCommand command2 = new SqlCommand(commandStr2, connection);
                    command2.ExecuteNonQuery();

                    string commandStr3 = "Alter Database [ToolsStoreLab] Set MULTI_USER";
                    SqlCommand command3 = new SqlCommand(commandStr3, connection);
                    command3.ExecuteNonQuery();

                    System.Windows.MessageBox.Show("Database restored succesfuly");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        public void ChangeHandArchivePath()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.InitialDirectory = handArchivePath;

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var directory = folderBrowserDialog.SelectedPath;

                if (directory != string.Empty)
                    handArchivePath = directory;
            }
        }
    }
}
