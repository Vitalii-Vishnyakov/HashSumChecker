using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace HashSumChecker
{
    public partial class Form1 : Form
    {
        public static string ToSHA256(string path)
        {
            FileStream file = System.IO.File.OpenRead(path);
            
            using var sha256 = SHA256.Create();
            byte[] fileData = new byte[file.Length];
            file.Read(fileData, 0, (int)file.Length);
            byte[] checkSum = sha256.ComputeHash(fileData);
            string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
            file.Close();
            return result.ToLower();
            
        }
        public static string ToSHA1(string path)
        {
            FileStream file = System.IO.File.OpenRead(path);
            
            using var sha1 = SHA1.Create();
            byte[] fileData = new byte[file.Length];
            file.Read(fileData, 0, (int)file.Length);
            byte[] checkSum = sha1.ComputeHash(fileData);
            string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
            file.Close();
            return result.ToLower();
            
        }
        public static string ToMD5(string path)
        {
            FileStream file = System.IO.File.OpenRead(path);
            
            using var md5 = MD5.Create();
            byte[] fileData = new byte[file.Length];
            file.Read(fileData, 0, (int)file.Length);
            byte[] checkSum = md5.ComputeHash(fileData);
            string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
            file.Close();
            return result.ToLower();
            
        }
        public void Test()
        {
            string testString = "a";
            string testHash = "0cc175b9c0f1b6a831c399e269772661";
            using var md5 = MD5.Create();
            byte[] fileData = new byte[testString.Length];
            fileData = Encoding.UTF8.GetBytes(testString);
            byte[] checkSum = md5.ComputeHash(fileData);
            string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
            if (result.ToLower()!= testHash)
            {
                label3.Visible = true;
                label3.Text = "Ошибка шифрования, программа не работает";
                button1.Enabled = false;
            }
        }
        public Form1()
        {
            InitializeComponent();
            Test();
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
            textBox1.BackColor = Color.LightGray;
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] inputFile = (string[])e.Data.GetData(DataFormats.FileDrop);
            textBox1.Text = inputFile[0];
            textBox1.BackColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text)
                && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text)){
                try
                {
                    string dataFromFile = File.ReadAllText(textBox1.Text);
                    string[] tmp = dataFromFile.Split(' ', '\n', '\r');
                    var tmplist = new List<string>();
                    for (int k = 0; k < tmp.Length; k++)
                    {
                        if (tmp[k] != "")
                        {
                            tmplist.Add(tmp[k]);
                        }
                    }
                    string[] separateDataFromFile = tmplist.ToArray();
                    string[] filesInDir = Directory.GetFiles(textBox2.Text, "*.*");
                    bool IsFileFound = false;
                    for (int i = 0;i< separateDataFromFile.Length; i+=3)
                    {
                        for (int j = 0;j< filesInDir.Length; j++)
                        {
                            if (filesInDir[j].Contains(separateDataFromFile[i]))
                            {
                                IsFileFound = true;
                                if (separateDataFromFile[i + 1].ToLower() == "md5")
                                {
                                    if (ToMD5(filesInDir[j])== separateDataFromFile[i + 2])
                                    {
                                        listBox1.Items.Add(separateDataFromFile[i] + " OK");
                                        break;
                                    }
                                    else
                                    {
                                        listBox1.Items.Add(separateDataFromFile[i] + " FAIL");
                                        break;
                                    }
                                }
                                if (separateDataFromFile[i + 1].ToLower() == "sha1")
                                {
                                    if (ToSHA1(filesInDir[j]) == separateDataFromFile[i + 2])
                                    {
                                        listBox1.Items.Add(separateDataFromFile[i] + " OK");
                                        break;
                                    }
                                    else
                                    {
                                        listBox1.Items.Add(separateDataFromFile[i] + " FAIL");
                                        break;
                                    }
                                }
                                if (separateDataFromFile[i + 1].ToLower() == "sha256")
                                {
                                    if (ToSHA256(filesInDir[j]) == separateDataFromFile[i + 2])
                                    {
                                        listBox1.Items.Add(separateDataFromFile[i] + " OK");
                                        break;
                                    }
                                    else
                                    {
                                        listBox1.Items.Add(separateDataFromFile[i] + " FAIL");
                                        break;
                                    }
                                }
                            }
                            
                        }
                        if (!IsFileFound)
                        {
                            listBox1.Items.Add(separateDataFromFile[i] + " NOT FOUND");
                            
                        }
                        IsFileFound = false;
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                label3.Visible = true;
                label3.Text = "Не все поля заполнены";
            }
        }

        private void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            string[] inputFile = (string[])e.Data.GetData(DataFormats.FileDrop);
            textBox2.Text = inputFile[0];
            textBox2.BackColor = Color.White;
        }

        private void textBox2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
            textBox2.BackColor = Color.LightGray;
        }

        private void textBox1_DragLeave(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.White;
        }

        private void textBox2_DragLeave(object sender, EventArgs e)
        {
            textBox2.BackColor = Color.White;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label3.Visible = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label3.Visible = false;
        }
    }
}
