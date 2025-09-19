using RMS.DataManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace RMS.Views
{
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
        }

        public int MainID = 0;
        public string OrderType = string.Empty;
        public int driverID = 0;
        public int CustomerId = 0;
        public int PromotionID = 0;
        public string customerName = string.Empty;
        public string customerPhone = string.Empty;

        private bool btnDeliveryClicked = false;
        private bool btnTakeAwayClicked = false;
        private bool btnDineClicked = false;

        private int currentPage = 1;

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();

            ProductPanel.Controls.Clear();
            LoadProducts(currentPage);

            guna2DataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;

            // Subscribe to the CellValueChanged event
            // Subscribe to the necessary events
            guna2DataGridView1.CellBeginEdit += new DataGridViewCellCancelEventHandler(guna2DataGridView1_CellBeginEdit);
            guna2DataGridView1.CellEndEdit += new DataGridViewCellEventHandler(guna2DataGridView1_CellEndEdit);
            guna2DataGridView1.CellValueChanged += new DataGridViewCellEventHandler(guna2DataGridView1_CellValueChanged);
            guna2DataGridView1.CellValidating += new DataGridViewCellValidatingEventHandler(guna2DataGridView1_CellValidating);
            // Setup the DataGridView
            SetupDataGridView();
        }

        private void guna2DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0)
            //{
            //    var row = guna2DataGridView1.Rows[e.RowIndex];

            //    // Check if the changed cell is Quantity or Price
            //    if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvQty" ||
            //        guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvPrice")
            //    {
            //        try
            //        {
            //            int qty = Convert.ToInt32(row.Cells["dgvQty"].Value);
            //            decimal price = Convert.ToDecimal(row.Cells["dgvPrice"].Value);
            //            row.Cells["dgvAmount"].Value = qty * price;

            //            GetTotal(); // Update the total amount
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("Invalid input. Please enter valid numeric values.");
            //        }
            //    }
            //}

            if (e.RowIndex >= 0)
            {
                var row = guna2DataGridView1.Rows[e.RowIndex];

                // Check if the changed cell is Quantity
                if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvQty")
                {
                    try
                    {
                        int qty = Convert.ToInt32(row.Cells["dgvQty"].Value);
                        decimal price = Convert.ToDecimal(row.Cells["dgvPrice"].Value);
                        row.Cells["dgvAmount"].Value = qty * price;

                        GetTotal(); // Update the total amount
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid input. Please enter valid numeric values.");
                    }
                }
            }
        }

        private void guna2DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Highlight the dgvQty cell when editing starts
            if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvQty")
            {
                guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightYellow;
            }
        }

        private void guna2DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Remove the highlight from the dgvQty cell when editing ends
            if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvQty")
            {
                guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void SetupDataGridView()
        {
            // Example of how to setup the DataGridView if it's not already done in the designer
            guna2DataGridView1.Columns.Add("dgvid", "ID");
            guna2DataGridView1.Columns.Add("dgvproID", "Product ID");
            guna2DataGridView1.Columns.Add("dgvPName", "Product Name");
            guna2DataGridView1.Columns.Add("dgvQty", "Quantity");
            guna2DataGridView1.Columns.Add("dgvPrice", "Price");
            guna2DataGridView1.Columns.Add("dgvAmount", "Amount");

            // Make specific columns editable
            guna2DataGridView1.Columns["dgvQty"].ReadOnly = false;
            guna2DataGridView1.Columns["dgvPName"].ReadOnly = true;
            guna2DataGridView1.Columns["dgvPrice"].ReadOnly = true;
            guna2DataGridView1.Columns["dgvAmount"].ReadOnly = true; // Amount should be calculated

            // Set the EditMode to EditOnEnter for single click edit
            guna2DataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        private void AddCategory()
        {
            var conn = MainClass.Connection();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                string qry = "SELECT catID, catName FROM Category";
                SqlCommand cmd = new SqlCommand(qry, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                CategoryPanel.Controls.Clear();

                // Add "All" button
                Guna.UI2.WinForms.Guna2Button allButton = new Guna.UI2.WinForms.Guna2Button();
                allButton.FillColor = Color.FromArgb(50, 55, 89);
                allButton.Size = new Size(180, 45);
                allButton.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                allButton.Text = "All";

                // Event for "All" button click
                allButton.Click += new EventHandler(b_Click);

                CategoryPanel.Controls.Add(allButton);

                // Select the "All" button by default
                allButton.PerformClick();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                        b.FillColor = Color.FromArgb(50, 55, 89);
                        b.Size = new Size(180, 45);
                        b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                        b.Text = row["catName"].ToString();

                        //event for click
                        b.Click += new EventHandler(b_Click);


                        CategoryPanel.Controls.Add(b);
                    }
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
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            string selectedCategory = b.Text.Trim().ToLower();

            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                if (selectedCategory == "all")
                {
                    pro.Visible = true;
                }
                else
                {
                    pro.Visible = pro.PCategroy.ToLower().Contains(selectedCategory);
                }
            }
        }

        private void AddItems(string id, string proID, string name, string cat, string price, Image pimage, string pBarcode)
        {
            var w = new ucProduct()
            {
                PName = name,
                PPrice = price,
                PCategroy = cat,
                PImage = pimage,
                PBarcode = pBarcode,
                PID = Convert.ToInt32(proID),
                Id = Convert.ToInt32(id),
            };

            ProductPanel.Controls.Add(w);

            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;

                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    // this will check it product already there then a one to quantity and update price
                    if (Convert.ToInt32(item.Cells["dgvid"].Value) == wdg.Id && Convert.ToInt32(item.Cells["dgvproID"].Value) == wdg.PID)
                    {
                        item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1;
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) * 
                                                        decimal.Parse(item.Cells["dgvPrice"].Value.ToString());

                        GetTotal();

                        return;
                    }
                }
                //this line add new product first for sr# and 2nd 0 from id
                guna2DataGridView1.Rows.Add(new object[] {0, wdg.Id, wdg.PID, wdg.PName, 1, wdg.PPrice, wdg.PPrice });
                GetTotal();
            };
        }

        //Getting product from database
        //public void LoadProducts()
        //{           
        //    var conn = MainClass.Connection(DataBase.RM);
        //    try
        //    {
        //        if (conn.State != ConnectionState.Open)
        //        {
        //            conn.Open();
        //        }

        //        string qry = "SELECT p.productID, p.productName, p.productPrice, p.categoryID, c.catName, p.productImage, productBarcode FROM products p INNER JOIN category c ON c.catID = p.categoryID;";
        //        SqlCommand cmd = new SqlCommand(qry, conn);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        foreach (DataRow item in dt.Rows)
        //        {
        //            Byte[] imagearray = (byte[])item["productImage"];
        //            byte[] immagebytearray = imagearray;

        //            AddItems("0", item["productID"].ToString(), item["productName"].ToString(), item["catName"].ToString(), item["productPrice"].ToString(), Image.FromStream(new MemoryStream(imagearray)), item["productBarcode"].ToString());
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
        //        guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
        //        guna2MessageDialog1.Show(ex.Message.ToString());
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }

        //}

        public async void LoadProducts(int pageNumber = 1, int pageSize = 5)
        {
            var conn = MainClass.Connection();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                int startRow = (pageNumber - 1) * pageSize;

                string qry = "SELECT p.productID, p.productName, p.productPrice, p.categoryID, c.catName, p.thumbnailImage, productBarcode " +
                             "FROM products p INNER JOIN category c ON c.catID = p.categoryID " +
                             "ORDER BY p.productName " +
                             "OFFSET @startRow ROWS FETCH NEXT @pageSize ROWS ONLY;";

                SqlCommand cmd = new SqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@startRow", startRow);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                await Task.Run(() => da.Fill(dt));  // Fill the data table asynchronously

                foreach (DataRow item in dt.Rows)
                {
                    Byte[] imagearray = (byte[])item["thumbnailImage"];
                    byte[] immagebytearray = imagearray;

                    // Update UI control safely
                    UpdateProductPanelSafe(() =>
                    {
                        AddItems("0", item["productID"].ToString(), item["productName"].ToString(), item["catName"].ToString(),
                                 item["productPrice"].ToString(), Image.FromStream(new MemoryStream(imagearray)),
                                 item["productBarcode"].ToString());
                    });
                }

                //// Load more products if available
                //if (dt.Rows.Count == pageSize)
                //{
                //    // Load next page asynchronously
                //    await Task.Run(() => LoadProducts(pageNumber + 1, pageSize));
                //}

                // If there are no more products to load, disable the load button
                if (dt.Rows.Count < pageSize)
                {
                    UpdateProductPanelSafe(() =>
                    {
                        loadMoreButton.Enabled = false;
                    });
                }
            }
            catch (Exception ex)
            {
                UpdateProductPanelSafe(() =>
                {
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show(ex.Message.ToString());
                });
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void UpdateProductPanelSafe(Action action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateProductPanelSafe(action)));
            }
            else
            {
                action();
            }
        }


        private void ShowErrorMessage(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ShowErrorMessage(message)));
            }
            else
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show(message);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();

            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;

                bool isBarcode = false;

                if (!string.IsNullOrEmpty(pro.PBarcode))
                {
                    // Check if the search term is a valid barcode
                    isBarcode = pro.PBarcode.ToLower() == searchTerm;
                }

                // Set visibility based on name or barcode match
                bool isVisibleByName = pro.PName.ToLower().Contains(searchTerm);
                bool isVisibleByBarcode = pro.PBarcode.ToLower().Contains(searchTerm);

                pro.Visible = isVisibleByName || isVisibleByBarcode;

                if (isBarcode)
                {
                    foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                    {
                        // Check if the product is already in the gridview
                        if (Convert.ToInt32(row.Cells["dgvid"].Value) == pro.Id && Convert.ToInt32(row.Cells["dgvproID"].Value) == pro.PID)
                        {
                            // Update quantity and amount if product already exists
                            row.Cells["dgvQty"].Value = int.Parse(row.Cells["dgvQty"].Value.ToString()) + 1;
                            row.Cells["dgvAmount"].Value = int.Parse(row.Cells["dgvQty"].Value.ToString()) *
                                                            decimal.Parse(row.Cells["dgvPrice"].Value.ToString());

                            GetTotal();
                            return;
                        }
                    }

                    // Add new product if not already in gridview
                    guna2DataGridView1.Rows.Add(new object[] { 0, pro.Id, pro.PID, pro.PName, 1, pro.PPrice, pro.PPrice });
                    GetTotal();
                }
            }
        }

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int count = 0;

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                count++;
                row.Cells[0].Value = count;
                row.Cells[0].ReadOnly = true;
            }
        }

        private void GetTotal()
        {
            decimal total = 0;
            lblTotal.Text = string.Empty;
            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                total += Convert.ToDecimal(item.Cells["dgvAmount"].Value);
            }

            lblTotal.Text = total.ToString("N2");
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            guna2DataGridView1.Rows.Clear();
            MainID = 0;
            lblTotal.Text = "0.00";
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            btnDeliveryClicked = true;

            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Delivery";

            using (frmOverlay overlay = new frmOverlay(this))
            {
                overlay.Show();

                using (frmAddCustomer frm = new frmAddCustomer())
                {
                    //frmAddCustomer frm = new frmAddCustomer();
                    frm.mainID = MainID;
                    frm.orderType = OrderType;
                    frm.ShowDialog();

                    if (frm.txtName.Text != "") //as take away did not have dirver info
                    {
                        driverID = frm.driverID;
                        lblDriverName.Text = "Customer Name: " + frm.txtName.Text + " Phone: " + frm.txtPhone.Text + " Driver: " + frm.cbDriver.Text;
                        lblDriverName.Visible = true;
                        CustomerId = frm.CustomerID;
                        //lblCustomerId.Text = frm.CustomerID.ToString();
                        customerName = frm.txtName.Text;
                        customerPhone = frm.txtPhone.Text;
                    }
                }

                overlay.Close();                
            }
        }

        private void btnTake_Click(object sender, EventArgs e)
        {
            btnTakeAwayClicked = true;

            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Take Away";

            using (frmOverlay overlay = new frmOverlay(this))
            {
                overlay.Show();

                // Open the second form
                using (frmAddCustomer frm = new frmAddCustomer())
                {
                    //frmAddCustomer frm = new frmAddCustomer();
                    frm.mainID = MainID;
                    frm.orderType = OrderType;
                    frm.ShowDialog();

                    if (frm.txtName.Text != "") //as take away did not have dirver info
                    {
                        driverID = frm.driverID;
                        lblDriverName.Text = "Customer Name: " + frm.txtName.Text + " Phone: " + frm.txtPhone.Text;
                        lblDriverName.Visible = true;
                        CustomerId = frm.CustomerID;
                        customerName = frm.txtName.Text;
                        customerPhone = frm.txtPhone.Text;
                    }
                }

                overlay.Close();
            }

                        
        }

        private void btnDine_Click(object sender, EventArgs e)
        {
            btnDineClicked = true;

            OrderType = "Dine In";
            lblDriverName.Visible = false;
            //need to create from for table selection and waiter selection

            using (frmOverlay overlay = new frmOverlay(this))
            {
                overlay.Show();

                // Open the second form
                using (frmTableSelect frm = new frmTableSelect())
                {
                    frm.ShowDialog();
                    
                    if (frm.TableName != "")
                    {
                        lblTable.Text = frm.TableName;
                        lblTable.Visible = true;
                    }
                    else
                    {
                        lblTable.Text = "";
                        lblTable.Visible = false;
                    }
                }

                overlay.Close();
            }

            //frmTableSelect frm = new frmTableSelect();
            //frm.ShowDialog();

            using (frmOverlay overlay = new frmOverlay(this))
            {
                overlay.Show();

                // Open the second form
                using (frmWaiterSelect frm2 = new frmWaiterSelect())
                {
                    frm2.ShowDialog();

                    if (frm2.WaiterName != "")
                    {
                        lblWaiter.Text = frm2.WaiterName;
                        lblWaiter.Visible = true;
                    }
                    else
                    {
                        lblWaiter.Text = "";
                        lblWaiter.Visible = false;
                    }
                }

                overlay.Close();
            }

            //frmWaiterSelect frm2 = new frmWaiterSelect();
            //frm2.ShowDialog();

            
        }

        private void btnKot_Click(object sender, EventArgs e)
        {
            //Save the data in database
            //Create tables
            //need to add field to table to store additional info

            if (guna2DataGridView1.Rows.Count == 0 || (!btnDeliveryClicked && !btnTakeAwayClicked && !btnDineClicked))
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Please select products before choosing an option (Dine, Delivery, or Take Away).");
                return;
            }

            string qry1 = string.Empty; //Main table
            string qry2 = string.Empty; // Detail table

            int detailID = 0;

            if(MainID == 0) // insert
            {
                //qry1 = @"Insert into tblMaster Values(@aDate, @aTime, @TableName, @WaiterName, 
                //            @status, @orderType, @total, @received, @change, @driverID, @CustName, @CustPhone)
                //            SELECT SCOPE_IDENTITY()";

 //               ,aDate DATE NULL
 //,aTime VARCHAR(15) NULL
 //,TableName VARCHAR(50) NULL
 //,WaiterName VARCHAR(50) NULL
 //,status VARCHAR(20) NULL
 //,orderType VARCHAR(20) NULL
 //,total DECIMAL(18, 2) NULL DEFAULT(0)
 //,discountAmount DECIMAL(10, 2) NULL DEFAULT(0)
 //,discountPercentage DECIMAL(10, 2) NULL DEFAULT(0)
 //,promotionID INT NULL DEFAULT(NULL)
 //,received DECIMAL(18, 2) NULL DEFAULT(0)
 //,change DECIMAL(18, 2) NULL DEFAULT(0)
 //,driverID INT NULL
 //,CustomerID INT NULL

                qry1 = @"Insert into tblMaster(aDate, aTime, TableName, WaiterName, status, 
                            orderType, total, discountAmount, discountPercentage, promotionID, received, change, driverID, CustomerID) 
                            Values(@aDate, @aTime, @TableName, @WaiterName, 
                            @status, @orderType, @total, @discountAmount, @discountPercentage, 
                            @promotionID, @received, @change, @driverID, @CustomerID)
                            SELECT SCOPE_IDENTITY()";
            }
            else
            {
                qry1 = @"Update tblMaster Set status = @status, total = @total, 
                        received = @received, change = @change Where mainID = @ID";
            }

            var conn = MainClass.Connection();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand(qry1, conn);
                cmd.Parameters.AddWithValue("@ID", MainID);
                cmd.Parameters.AddWithValue("@aDate", Convert.ToDateTime(DateTime.Now.Date));
                cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@TableName", lblTable.Text);
                cmd.Parameters.AddWithValue("@WaiterName", lblWaiter.Text);
                cmd.Parameters.AddWithValue("@status", "Pending");
                cmd.Parameters.AddWithValue("@orderType", OrderType);
                cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(lblTotal.Text));
                cmd.Parameters.AddWithValue("@discountAmount", Convert.ToDecimal(0));
                cmd.Parameters.AddWithValue("@discountPercentage", Convert.ToDecimal(0));
                cmd.Parameters.AddWithValue("@received", Convert.ToDecimal(0));
                cmd.Parameters.AddWithValue("@change", Convert.ToDecimal(0));
                cmd.Parameters.AddWithValue("@driverID", driverID == 0 ? (object)DBNull.Value : driverID);
                cmd.Parameters.AddWithValue("@CustomerID", CustomerId == 0 ? (object)DBNull.Value : CustomerId);
                cmd.Parameters.AddWithValue("@promotionID", PromotionID == 0 ? (object)DBNull.Value : PromotionID);

                //cmd.Parameters.AddWithValue("@CustName", customerName);
                //cmd.Parameters.AddWithValue("@CustPhone", customerPhone);

                if (MainID == 0) { MainID = Convert.ToInt32(cmd.ExecuteScalar());} else { cmd.ExecuteNonQuery(); }
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                {
                    detailID = Convert.ToInt32(row.Cells["dgvid"].Value);

                    if (detailID == 0) //insert
                    {
                        qry2 = @"Insert into tblDetails(mainID, proID, qty, price, amount) Values(@MainID, @proID, @qty, @price, @amount)";
                        //qry2 = @"Insert into tblDetails Values(7, 2, 3, 50.00, 750.45)";
                    }
                    else //update
                    {
                        qry2 = @"Update tblDetails Set proID = @proID, qty = @qty, price = @price,
                                amount = @amount Where detailID = @ID";
                    }

                    SqlCommand cmd2 = new SqlCommand(qry2, conn);
                    cmd2.Parameters.AddWithValue("@ID", detailID);
                    cmd2.Parameters.AddWithValue("@MainID", MainID);
                    cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvproID"].Value));
                    cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                    cmd2.Parameters.AddWithValue("@price", Convert.ToDecimal(row.Cells["dgvPrice"].Value));
                    cmd2.Parameters.AddWithValue("@amount", Convert.ToDecimal(row.Cells["dgvAmount"].Value));

                    cmd2.ExecuteNonQuery();      

                }

                guna2MessageDialog1.Show("Saved Successfully");
                MainID = 0;
                detailID = 0;
                guna2DataGridView1.Rows.Clear();
                lblTable.Text = "";
                lblWaiter.Text = "";
                lblTable.Visible = false;
                lblWaiter.Visible = false;
                lblTotal.Text = "0.00";
                lblDriverName.Text = string.Empty;

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show(ex.Message.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                try
                {
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;

                    if (guna2MessageDialog1.Show("Are you sure you want to delete?") == DialogResult.Yes)
                    {
                        // Get the index of the current row
                        int rowIndex = guna2DataGridView1.CurrentCell.RowIndex;

                        // Remove the row from the DataGridView
                        guna2DataGridView1.Rows.RemoveAt(rowIndex);

                        GetTotal();
                    }
                }
                catch (Exception ex)
                {
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show(ex.Message.ToString());
                }

            }
        }

        private void guna2DataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Only validate if editing dgvQty column
            if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvQty")
            {
                int newInteger;

                // Check if the value is a valid integer
                if (!int.TryParse(Convert.ToString(e.FormattedValue), out newInteger) || newInteger <= 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Quantity must be a positive integer.");
                }
            }
        }

        public int id = 0;

        private void btnBill_Click(object sender, EventArgs e)
        {
            using (frmOverlay overlay = new frmOverlay(this))
            {
                overlay.Show();

                using (frmBillList frm = new frmBillList())
                {
                    frm.ShowDialog();

                    if (frm.MainID > 0)
                    {
                        id = frm.MainID;
                        MainID = frm.MainID;
                        LoadEntries();
                    }
                }

                overlay.Close();
            }
        }

        private void LoadEntries()
        {
            var conn = MainClass.Connection();

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                string qry = @"SELECT p.productName, d.detailID, d.proID, qty, d.price, d.amount, m.orderType, m.TableName, m.WaiterName FROM tblMaster m
                            INNER JOIN tblDetails d on m.MainID = d.MainID
                            INNER JOIN products p ON p.productID = d.proID
                            WHERE m.MainID = " + id + "";

                SqlCommand cmd = new SqlCommand(qry, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows[0]["orderType"].ToString().ToLower() == "delivery")
                {
                    btnDelivery.Checked = true;
                    lblWaiter.Visible = false;
                    lblTable.Visible = false;
                }
                else if (dt.Rows[0]["orderType"].ToString().ToLower() == "take away")
                {
                    btnTake.Checked = true;
                    lblWaiter.Visible = false;
                    lblTable.Visible = false;
                }
                else
                {
                    btnDine.Checked = true;
                    lblWaiter.Visible = true;
                    lblTable.Visible = true;
                }

                guna2DataGridView1.Rows.Clear();

                foreach (DataRow item in dt.Rows)
                {
                    lblTable.Text = item["TableName"].ToString();
                    lblWaiter.Text = item["WaiterName"].ToString();

                    string detailid = item["DetailID"].ToString();
                    string proid = item["proID"].ToString();
                    string proName = item["productName"].ToString();
                    string qty = item["qty"].ToString();
                    string price = item["price"].ToString();
                    string amount = item["amount"].ToString();

                    object[] obj = {0, detailid, proid, proName, qty, price, amount};
                    guna2DataGridView1.Rows.Add(obj);
                }
                GetTotal();
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

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            var conn = MainClass.Connection();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                string qry = @"Select status from tblMaster Where MainID = "+ id+"";

                SqlCommand cmd = new SqlCommand(qry, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["status"].ToString() == "Completed")
                    {
                        btnCheckout.Enabled = true;

                        using (frmOverlay overlay = new frmOverlay(this))
                        {
                            overlay.Show();

                            using (frmCheckout frm = new frmCheckout())
                            {
                                frm.MainID = id;
                                frm.amt = Convert.ToDecimal(lblTotal.Text);
                                frm.ShowDialog();
                            }

                            overlay.Close();
                        }

                        

                        MainID = 0;
                        guna2DataGridView1.Rows.Clear();
                        lblTable.Text = "";
                        lblWaiter.Text = "";
                        lblTable.Visible = false;
                        lblWaiter.Visible = false;
                        lblTotal.Text = "0.00";
                        return;
                    }
                    else if (dt.Rows[0]["status"].ToString() == "Paid")
                    {
                        //btnCheckout.Enabled = false;
                        guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                        guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                        guna2MessageDialog1.Show("Already Paid");
                        return;
                    }
                    
                }
                else
                {
                    //btnCheckout.Enabled = false;
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show("Please first send your order to kitchen");
                    return;
                }


            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show(ex.Message.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            //Save the data in database
            //Create tables

            if (guna2DataGridView1.Rows.Count == 0 || (!btnDeliveryClicked && !btnTakeAwayClicked && !btnDineClicked))
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Please select products before choosing an option (Dine, Delivery, or Take Away).");
                return;
            }

            string qry1 = string.Empty; //Main table
            string qry2 = string.Empty; // Detail table

            int detailID = 0;

            if (OrderType == "")
            {
                guna2MessageDialog1.Show("Please select order type");
                return;
            }

            if (MainID == 0) // insert
            {
                //qry1 = @"Insert into tblMaster Values(@aDate, @aTime, @TableName, @WaiterName, 
                //            @status, @orderType, @total, @received, @change, @driverID, @CustName, @CustPhone)
                //            SELECT SCOPE_IDENTITY()";

                qry1 = @"Insert into tblMaster Values(@aDate, @aTime, @TableName, @WaiterName, 
                            @status, @orderType, @total, @discountAmount, @discountPercentage, 
                            @received, @change, @driverID, @CustomerID, @promotionID)
                            SELECT SCOPE_IDENTITY()";
            }
            else
            {
                qry1 = @"Update tblMaster Set status = @status, total = @total, 
                        received = @received, change = @change Where mainID = @ID";
            }

            var conn = MainClass.Connection();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand(qry1, conn);
                cmd.Parameters.AddWithValue("@ID", MainID);
                cmd.Parameters.AddWithValue("@aDate", Convert.ToDateTime(DateTime.Now.Date));
                cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@TableName", lblTable.Text);
                cmd.Parameters.AddWithValue("@WaiterName", lblWaiter.Text);
                cmd.Parameters.AddWithValue("@status", "Hold");
                cmd.Parameters.AddWithValue("@orderType", OrderType);
                cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(lblTotal.Text)); //as we only saving data for kitchen value will update when payment received 
                cmd.Parameters.AddWithValue("@discountAmount", Convert.ToDecimal(0));
                cmd.Parameters.AddWithValue("@discountPercentage", Convert.ToDecimal(0));
                cmd.Parameters.AddWithValue("@received", Convert.ToDecimal(0));
                cmd.Parameters.AddWithValue("@change", Convert.ToDecimal(0));
                cmd.Parameters.AddWithValue("@driverID", driverID);
                cmd.Parameters.AddWithValue("@CustomerID", CustomerId);
                cmd.Parameters.AddWithValue("@promotionID", PromotionID);

                //cmd.Parameters.AddWithValue("@CustName", customerName);
                //cmd.Parameters.AddWithValue("@CustPhone", customerPhone);

                if (MainID == 0) { MainID = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                {
                    detailID = Convert.ToInt32(row.Cells["dgvid"].Value);

                    if (detailID == 0) //insert
                    {
                        qry2 = @"Insert into tblDetails(mainID, proID, qty, price, amount) Values(@MainID, @proID, @qty, @price, @amount)";
                        //qry2 = @"Insert into tblDetails Values(7, 2, 3, 50.00, 750.45)";
                    }
                    else //update
                    {
                        qry2 = @"Update tblDetails Set proID = @proID, qty = @qty, price = @price,
                                amount = @amount Where detailID = @ID";
                    }

                    SqlCommand cmd2 = new SqlCommand(qry2, conn);
                    cmd2.Parameters.AddWithValue("@ID", detailID);
                    cmd2.Parameters.AddWithValue("@MainID", MainID);
                    cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvproID"].Value));
                    cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                    cmd2.Parameters.AddWithValue("@price", Convert.ToDecimal(row.Cells["dgvPrice"].Value));
                    cmd2.Parameters.AddWithValue("@amount", Convert.ToDecimal(row.Cells["dgvAmount"].Value));

                    cmd2.ExecuteNonQuery();

                }

                guna2MessageDialog1.Show("Saved Successfully");
                MainID = 0;
                detailID = 0;
                guna2DataGridView1.Rows.Clear();
                lblTable.Text = "";
                lblWaiter.Text = "";
                lblTable.Visible = false;
                lblWaiter.Visible = false;
                lblTotal.Text = "0.00";
                lblDriverName.Text = string.Empty;


                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show(ex.Message.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void frmPOS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F) // Ctrl + F to focus on the search bar
            {
                txtSearch.Focus();
                e.Handled = true;
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    // Code to add the first search result to the order
            //    AddFirstSearchResultToOrder();
            //    e.Handled = true;
            //}

            string searchTerm = txtSearch.Text.Trim().ToLower();

            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;

                bool isVisibleByName = pro.PName.ToLower().Contains(searchTerm);
                bool isVisibleByBarcode = pro.PBarcode.ToLower().Contains(searchTerm); // Assuming you added a PBarcode property

                // Set visibility based on name or barcode match
                pro.Visible = isVisibleByName || isVisibleByBarcode;

                //pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
        }

        private void loadMoreButton_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadProducts(currentPage);
        }
    }
}
