using RMS.DataManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RMS.Views
{
    public partial class frmKitchenView : Form
    {
        public frmKitchenView()
        {
            InitializeComponent();
        }

        private void frmKitchenView_Load(object sender, EventArgs e)
        {
            GetOrders();
        }

        private void GetOrders()
        {
            flowLayoutPanel1.Controls.Clear();

            var conn = MainClass.Connection();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                string qry1 = "SELECT MainID, TableName, WaiterName, aTime, orderType FROM tblMaster where status = 'Pending'";
                SqlCommand cmd1 = new SqlCommand(qry1, conn);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);

                FlowLayoutPanel p1;
                FlowLayoutPanel p2;

                for (global::System.Int32 i = 0; i < dt1.Rows.Count; i++)
                {
                    p1 = new FlowLayoutPanel();
                    p1.AutoSize = true;
                    p1.Width = 230;
                    p1.Height = 350;
                    p1.FlowDirection = FlowDirection.TopDown;
                    p1.BorderStyle = BorderStyle.FixedSingle;
                    p1.Margin = new Padding(10, 10, 10, 10);

                    p2 = new FlowLayoutPanel();
                    p2.BackColor = Color.FromArgb(50, 55, 89);
                    p2.AutoSize = true;
                    p2.Width = 230;
                    p2.Height = 125;
                    p2.FlowDirection = FlowDirection.TopDown;
                    p2.Margin = new Padding(0, 0, 0, 0);

                    Label lb1 = new Label();
                    lb1.ForeColor = Color.White;
                    lb1.Margin = new Padding(10, 10, 3, 0);
                    lb1.AutoSize = true;

                    Label lb2 = new Label();
                    lb2.ForeColor = Color.White;
                    lb2.Margin = new Padding(10, 10, 3, 0);
                    lb2.AutoSize = true;

                    Label lb3 = new Label();
                    lb3.ForeColor = Color.White;
                    lb3.Margin = new Padding(10, 10, 3, 0);
                    lb3.AutoSize = true;

                    Label lb4 = new Label();
                    lb4.ForeColor = Color.White;
                    lb4.Margin = new Padding(10, 10, 3, 10);
                    lb4.AutoSize = true;

                    lb1.Text = "Table : " + dt1.Rows[i]["TableName"].ToString();
                    lb2.Text = "Waiter Name : " + dt1.Rows[i]["WaiterName"].ToString();
                    lb3.Text = "Order Time : " + dt1.Rows[i]["aTime"].ToString();
                    lb4.Text = "Order Type : " + dt1.Rows[i]["orderType"].ToString();

                    p2.Controls.Add(lb1);
                    p2.Controls.Add(lb2);
                    p2.Controls.Add(lb3);
                    p2.Controls.Add(lb4);

                    p1.Controls.Add(p2);

                    //now add products

                    int mid = 0;
                    mid = Convert.ToInt32(dt1.Rows[i]["MainID"].ToString());

                    string qry2 = @"SELECT productName, qty FROM tblMaster m
                                    INNER JOIN tblDetails d on m.MainID = d.MainID
                                    INNER JOIN products p ON p.productID = d.proID
                                    WHERE m.MainID = " + mid +"";

                    SqlCommand cmd2 = new SqlCommand(qry2, conn);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);

                    for (global::System.Int32 j = 0; j < dt2.Rows.Count; j++)
                    {
                        Label lb5 = new Label();
                        lb5.ForeColor = Color.Black;
                        lb5.Margin = new Padding(10, 5, 3, 0);
                        lb5.AutoSize = true;

                        int no = j + 1;

                        lb5.Text = "" + no + " " + dt2.Rows[j]["productName"].ToString() + " " + dt2.Rows[j]["qty"].ToString();

                        p1.Controls.Add(lb5);
                    }

                    //Add button to change the order status
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.AutoRoundedCorners = true;
                    b.Size = new Size(100, 35);
                    b.FillColor = Color.FromArgb(18, 184, 134);
                    b.Margin = new Padding(30, 5, 3, 10);
                    b.Text = "Complete";
                    b.Tag = dt1.Rows[i]["MainID"].ToString(); //store the id

                    //event for click
                    b.Click += new EventHandler(b_Click);
                    p1.Controls.Add(b);

                    flowLayoutPanel1.Controls.Add(p1);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void b_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as Guna.UI2.WinForms.Guna2Button)?.Tag?.ToString() ?? "0");

            // Create a new instance of Guna2MessageDialog
            using (var messageDialog = new Guna.UI2.WinForms.Guna2MessageDialog())
            {
                messageDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                messageDialog.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;

                if (messageDialog.Show("Are you sure you want to delete?") == DialogResult.Yes)
                {
                    string qry = @"Update tblMaster set status = 'Completed' where MainID = @ID";

                    Hashtable ht = new Hashtable
                    {
                        { "@ID", id }
                    };

                    if (MainClass.SQL(qry, ht) > 0)
                    {
                        using (var successDialog = new Guna.UI2.WinForms.Guna2MessageDialog())
                        {
                            successDialog.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                            successDialog.Show("Saved Successfully");
                        }
                    }

                    GetOrders();
                }
            }
        }
    }
}
