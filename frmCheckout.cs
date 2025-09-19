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
using System.Xml.Linq;

namespace RMS.Views
{
    public partial class frmCheckout : Form
    {
        public frmCheckout()
        {
            InitializeComponent();
        }

        public decimal amt;
        public int MainID = 0;
        public int PromotionID = 0;
        private void txtReceived_TextChanged(object sender, EventArgs e)
        {
            decimal totalAmt = 0;
            decimal receipt = 0;
            decimal change = 0;

            decimal.TryParse(txtTotalAmount.Text, out totalAmt);
            decimal.TryParse(txtReceived.Text, out receipt);

            change = Math.Abs(totalAmt - receipt); //convert positive or negative number to positive number 

            txtChange.Text = change.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtReceived.Text))
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Please enter received amount");
                return;
            }

            if (Convert.ToDecimal(txtTotalAmount.Text) < Convert.ToDecimal(txtReceived.Text)) 
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Received amount cannot exceed total amount.");
                return; 
            }

            var conn = MainClass.Connection();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                string qry = @"Update tblMaster set total = @total, discountAmount = @discountAmount, 
                            discountPercentage = @discountPercentage, promotionID = @promotionID, 
                            received = @rec, change = @change, status = 'Paid' Where MainID = @id;
                            UPDATE p
                            SET p.stockQuantity = p.stockQuantity - d.qty
                            FROM products p
                            INNER JOIN tblDetails d ON p.productID = d.proID
                            INNER JOIN tblMaster m ON d.mainID = m.mainID
                            WHERE m.mainID = @id;
                                ";

                Hashtable ht = new Hashtable();
                ht.Add("@id", MainID);
                ht.Add("@total", txtBillAmount.Text);
                ht.Add("@discountAmount", txtDiscountAmt.Text == "" ? "0" : txtDiscountAmt.Text);
                ht.Add("@discountPercentage", txtDiscountPercent.Text == "" ? "0" : txtDiscountPercent.Text);
                ht.Add("@promotionID", PromotionID);
                ht.Add("@rec", txtReceived.Text);
                ht.Add("@change", txtChange.Text);

                if (MainClass.SQL(qry, ht) > 0)
                {
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show("Saved Successfully");
                    this.Close();
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

        private void frmCheckout_Load(object sender, EventArgs e)
        {
            txtBillAmount.Text = amt.ToString();
            txtTotalAmount.Text = amt.ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //frmPOS frm = new frmPOS();
            //frm.LoadProducts();

            this.Close();
        }

        private void txtDiscountAmt_TextChanged(object sender, EventArgs e)
        {
            //if (decimal.TryParse(txtDiscountAmt.Text, out decimal discountAmount))
            //{
            //    decimal billAmount = decimal.Parse(txtBillAmount.Text);
            //    decimal percentage = (discountAmount / billAmount) * 100;
            //    txtDiscountPercent.Text = percentage.ToString("0.00");
            //    UpdateTotal();
            //}

            //Check if Discount Amount is a valid decimal
            //if (decimal.TryParse(txtDiscountAmt.Text, out decimal discountAmount))
            //{
            //    // Check if Bill Amount is a valid decimal
            //    if (decimal.TryParse(txtBillAmount.Text, out decimal billAmount))
            //    {
            //        // Ensure the bill amount is not zero to avoid division by zero
            //        if (billAmount != 0)
            //        {
            //            // Calculate percentage
            //            decimal percentage = (discountAmount / billAmount) * 100;

            //            // Clamp the percentage to a reasonable range (0% to 100%)
            //            if (percentage < 0)
            //            {
            //                percentage = 0;
            //            }
            //            else if (percentage > 100)
            //            {
            //                percentage = 100;
            //            }

            //            // Update Discount Amount text box to display the percentage
            //            txtDiscountPercent.Text = percentage.ToString("0.00");

            //            // Update the total
            //            UpdateTotal();
            //        }
            //        else
            //        {
            //            MessageBox.Show("Bill Amount cannot be zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Invalid Bill Amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Invalid Discount Amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void txtDiscountPercent_TextChanged(object sender, EventArgs e)
        {
            //if (decimal.TryParse(txtDiscountPercent.Text, out decimal percentage))
            //{
            //    decimal billAmount = decimal.Parse(txtBillAmount.Text);
            //    decimal discountAmount = (percentage / 100) * billAmount;
            //    txtDiscountPercent.Text = discountAmount.ToString("0.00");
            //    UpdateTotal();
            //}

            // Ensure the input is a valid decimal percentage
            //if (decimal.TryParse(txtDiscountPercent.Text, out decimal percentage))
            //{
            //    // Ensure the percentage is within a valid range
            //    if (percentage >= 0 && percentage <= 100)
            //    {
            //        // Ensure the bill amount is a valid decimal
            //        if (decimal.TryParse(txtBillAmount.Text, out decimal billAmount))
            //        {
            //            // Calculate the discount amount based on the percentage
            //            decimal discountAmount = (percentage / 100) * billAmount;

            //            // Update the DiscountAmount field with the calculated value
            //            txtDiscountAmt.Text = discountAmount.ToString("0.00");

            //            // Update the total amount after applying the discount
            //            UpdateTotal();
            //        }
            //        else
            //        {
            //            MessageBox.Show("Invalid bill amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Discount percentage must be between 0 and 100.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Invalid discount percentage.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //if (string.IsNullOrEmpty(txtDiscountPercent.Text))
            //{
            //    txtDiscountPercent.Text = "0";
            //}
        }

        private void ApplyCoupon()
        {
            string couponCode = txtApplyCoupon.Text.Trim();

            if (string.IsNullOrEmpty(couponCode))
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Please enter a coupon code.");

                return;
            }

            var conn = MainClass.Connection();

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                string qry = @"SELECT promotionID,DiscountAmount, DiscountPercentage 
                         FROM Promotions 
                         WHERE CouponCode = @CouponCode 
                         AND IsActive = 1 
                         AND ValidFrom <= GETDATE() 
                         AND ValidTo >= GETDATE()";

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@CouponCode", couponCode);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            PromotionID = reader["promotionID"] != DBNull.Value ? Convert.ToInt32(reader["promotionID"]) : 0;
                            decimal discountAmount = reader["discountAmount"] != DBNull.Value ? Convert.ToDecimal(reader["discountAmount"]) : 0;
                            decimal discountPercentage = reader["discountPercentage"] != DBNull.Value ? Convert.ToDecimal(reader["discountPercentage"]) : 0;

                            // Ensure the discountAmount is rounded correctly if it comes directly from the database
                            discountAmount = Math.Round(discountAmount, 2);

                            // If discount is in percentage, calculate discount amount
                            if (discountPercentage > 0)
                            {
                                decimal billAmount = decimal.Parse(txtBillAmount.Text);
                                discountAmount = Math.Round((discountPercentage / 100) * billAmount);
                            }

                            txtDiscountAmt.Text = discountAmount.ToString("0.00");

                            // Show applied coupon message
                            lblAppliedCoupon.Text = $"Coupon applied: {couponCode}";
                            lblAppliedCoupon.Visible = true;

                            UpdateTotal();
                        }
                        else
                        {
                            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                            guna2MessageDialog1.Show("Invalid or expired coupon.");
                        }
                    }
                }
            
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void UpdateTotal()
        {
           decimal billAmount = decimal.Parse(txtBillAmount.Text);
            decimal discountAmount = decimal.Parse(txtDiscountAmt.Text);
            decimal totalAmount = billAmount - discountAmount;
            txtTotalAmount.Text = totalAmount.ToString("0.00");
            if (!string.IsNullOrEmpty(txtReceived.Text)) 
            {
                txtReceived.Text = totalAmount.ToString("0.00");
            }
        }

        private void btnApplyCoupon_Click(object sender, EventArgs e)
        {
            ApplyCoupon();
        }

        private void txtDiscountPercent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true; // Disallow other characters
            }

            //// Allow only one decimal point
            //if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            //{
            //    e.Handled = true; // Disallow multiple decimal points
            //}

            // Check if the Enter key is pressed
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Prevent the ding sound or default behavior

                // Validate and process the discount
                if (decimal.TryParse(txtDiscountPercent.Text, out decimal percentage))
                {
                    // Ensure the percentage is within a valid range
                    if (percentage >= 0 && percentage <= 100)
                    {
                        // Ensure the bill amount is a valid decimal
                        if (decimal.TryParse(txtBillAmount.Text, out decimal billAmount))
                        {
                            // Calculate the discount amount based on the percentage
                            decimal discountAmount = (percentage / 100) * billAmount;

                            // Update the DiscountAmount field with the calculated value
                            txtDiscountAmt.Text = discountAmount.ToString("0.00");

                            // Update the total amount after applying the discount
                            UpdateTotal();
                        }
                        else
                        {
                            MessageBox.Show("Invalid bill amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Discount percentage must be between 0 and 100.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDiscountPercent.Text = "0";
                    }
                }
                else
                {
                    MessageBox.Show("Invalid discount percentage.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtDiscountAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true; // Disallow other characters
            }

            // Check if the Enter key is pressed
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Prevent the default behavior (like beep sound)

                if (decimal.TryParse(txtDiscountAmt.Text, out decimal discountAmount))
                {
                    // Check if Bill Amount is a valid decimal
                    if (decimal.TryParse(txtBillAmount.Text, out decimal billAmount))
                    {
                        // Ensure the bill amount is not zero to avoid division by zero
                        if (billAmount != 0)
                        {
                            // Calculate percentage
                            decimal percentage = (discountAmount / billAmount) * 100;

                            // Clamp the percentage to a reasonable range (0% to 100%)
                            if (percentage < 0)
                            {
                                percentage = 0;
                            }
                            else if (percentage > 100)
                            {
                                percentage = 100;
                            }

                            // Update Discount Percentage text box to display the percentage
                            txtDiscountPercent.Text = percentage.ToString("0.00");

                            // Update the total
                            UpdateTotal();
                        }
                        else
                        {
                            MessageBox.Show("Bill Amount cannot be zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Bill Amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Discount Amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtReceived_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true; // Disallow other characters
            }
        }
    }
}
