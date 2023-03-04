using System;
using System.Windows.Forms;

namespace BTree1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            nudMinDeg.Minimum = 2;
            b = new BTree((int)nudMinDeg.Value);
            btnAdd.Enabled = false;
            btnClear.Enabled = false;
            btnDelete.Enabled = false;
            btnFind.Enabled = false;
            txtbInput.Enabled = false;
        }

        BTree b;
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnAdd_Click(sender, e);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtbInput.Text.Trim() == "")
            {
                MessageBox.Show("Enter number");
                return;
            }
            b.Insert(Int32.Parse(txtbInput.Text.Trim()));

            b.Show(treeView1);
            txtbInput.Clear();
            txtbInput.Focus();
        }

        private void btnDeg_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            b = new BTree((int)nudMinDeg.Value);
            btnAdd.Enabled = true;
            btnClear.Enabled = true;
            btnDelete.Enabled = true;
            btnFind.Enabled = true;
            txtbInput.Enabled = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (b.root == null)
            {
                MessageBox.Show("Tree is empty");
                return;
            }
            if (txtbInput.Text.Trim() == "")
            {
                MessageBox.Show("Enter number");
                return;
            }
            if (b.Contain(Int32.Parse(txtbInput.Text.Trim())))
            {
                MessageBox.Show("Found");
            }
            else
            {
                MessageBox.Show("Not Found");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (b.root == null)
            {
                MessageBox.Show("Tree is empty");
                return;
            }
            if (txtbInput.Text.Trim() == "")
            {
                MessageBox.Show("Enter number");
                return;
            }
            int key = Convert.ToInt32(txtbInput.Text.Trim());
            b.Remove(key);
            b.Show(treeView1);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (b.root == null)
            {
                MessageBox.Show("Tree is empty");
                return;
            }
            treeView1.Nodes.Clear();
            b.Clear();
        }
    }
}
