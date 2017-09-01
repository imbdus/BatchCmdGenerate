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
using System.Text.RegularExpressions;

namespace Batch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string path;


        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            //Add column header
            listView1.Columns.Add("$1", 300);
            listView1.Columns.Add("$2", 100);
            listView1.Columns.Add("$3", 400);

            //tooltops loading
            tooltips(sender, e);
        }

        private void tooltips(object sender, EventArgs e)
        {
            //this.toolTip1.SetToolTip(this.bt_loadsub, "选择一个文件，获取文件夹下所有的相同类文件");
        }


        // bt_loadsub
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Chose folder";
            if (openFileDialog1.ShowDialog() == DialogResult.OK || openFileDialog1.ShowDialog() == DialogResult.Yes)
            {
                path = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);

                //获取目录下文件名
                DirectoryInfo folder = new DirectoryInfo(path);
                foreach (FileInfo item in folder.GetFiles("*" + Path.GetExtension(openFileDialog1.FileName)))
                {                   
                    string[] arr = {
                        item.Directory.FullName,
                        item.Name,
                        item.FullName
                    };
                    listView1.Items.Add(new ListViewItem(arr));
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK || folderBrowserDialog1.ShowDialog() == DialogResult.Yes)
            {
                path = folderBrowserDialog1.SelectedPath;

                //获取所有的子目录
                DirectoryInfo di = new DirectoryInfo(path);//https://msdn.microsoft.com/en-us/library/s7xk2b58(v=vs.110).aspx
                DirectoryInfo[] diArr = di.GetDirectories("*.*", System.IO.SearchOption.AllDirectories);//https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/file-system/how-to-iterate-through-a-directory-tree

                foreach (DirectoryInfo dir in diArr)
                {
                    //MessageBox.Show("Test" + dir.Name + " "+ dir.Root + " " + dir.FullName);
                    //dir.FullName 就是目录树下的所有子目录的文件

                    foreach (FileInfo item in dir.GetFiles(tb_fileextension.Text))
                    {
                        string[] arr = {
                            item.Directory.FullName,
                            item.Name,
                            item.FullName//mc[0].Value
                        };
                        listView1.Items.Add(new ListViewItem(arr));
                    }//foreach
                }//foreach
            }//fi
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Chose one";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo item = new FileInfo(openFileDialog1.FileName);

                string[] arr = {
                        item.Directory.FullName,
                        item.Name,
                        item.FullName
                    };
                listView1.Items.Add(new ListViewItem(arr));
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (listView1.Items != null)
            {

                string[] nums = {
                @listView1.Items[0].SubItems[0].Text,
                @listView1.Items[0].SubItems[1].Text,
                @listView1.Items[0].SubItems[2].Text
                //@listView1.SelectedItems[0].SubItems[2].Text
             };


                string[] patterns =  {
                    @"\p{Sc}1",@"\p{Sc}2",@"\p{Sc}3"
            };
                //https://msdn.microsoft.com/zh-cn/library/e7f5w83z(v=vs.110).aspx
                //https://msdn.microsoft.com/zh-cn/library/ewy2t5e0(v=vs.110).aspx

                string ans = @tb_re.Text;

                for (int i = 0; i < patterns.Length; i++)
                {
                    //MessageBox.Show("for: " + input_text+"  "+  patterns[i] + " "+ nums[i]);
                    ans = @Regex.Replace(@ans, @patterns[i], @nums[i]);
                }

                MessageBox.Show(@ans);


                //MatchCollection mc = Regex.Matches(item.Name, pattern);
                //MessageBox.Show(mc[0].Value);
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (listView1.Items != null)
            {
                //正则替换的结果
                List<string> ans_l = new List<string>();

                foreach (ListViewItem item in listView1.Items)
                {
                    string ans = @tb_re.Text;
                    for (int i = 0; i < item.SubItems.Count; i++)
                    {
                        //进行正则替换 通过 listView1.Items[0].SubItems.Count 次替换 替换所有的参数 生成ans
                        ans = @Regex.Replace(@ans, (@"\p{Sc}" + (i + 1)), @item.SubItems[i].Text);
                    }
                    ans_l.Add(@ans);     //MessageBox.Show(ans);
                }//foreach

                //save ans_l into text file
                SaveFileDialog svd = new SaveFileDialog();
                svd.Title = "Save commands to...";
                if (svd.ShowDialog() == DialogResult.OK)
                {
                    if (svd.CheckPathExists)
                    {
                        File.WriteAllLines(svd.FileName, ans_l.ToArray());
                    }

                    //打开文件夹
                    System.Diagnostics.Process.Start("notepad.exe", svd.FileName);
                }

            }//fi
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Chose one";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo item = new FileInfo(openFileDialog1.FileName);
                System.Diagnostics.Process.Start("cmd.exe", svd.FileName);
            }

        }
    }
}
