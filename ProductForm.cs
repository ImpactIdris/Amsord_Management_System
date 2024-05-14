using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace AmsordMall
{
    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
            panelControl();
            tabProduct.ItemSize = new Size(0, 1);
            disableCurrency();
            panCatCtr.Visible = false;
            btnUpdateCat.Enabled = false;
            disableProdEntry();
        }
        //Import Sound Class
        SFX sfx = new SFX();

        void panelControl()
        {
            tabProduct.SelectedTab = pageDefault;
            panCatCtr.Visible = false;
            panSizeCtr.Visible = false;
            gbxCat.Size = new Size(342, 282);
            gbxSize.Size = new Size(339, 282);
        }

        private void btnCloseProduct_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void txtNewCat_TextChanged(object sender, EventArgs e)
        {
            lbCatName.Visible = false;
            if (txtNewCat.Text != String.Empty)
            {
                btnSaveCat.Enabled = true;
            }
        }
        
        private void lbSize_Click(object sender, EventArgs e)
        {
            lbSize.Visible = false;
            txtSizes.Focus();
        }

        private void txtSizes_TextChanged(object sender, EventArgs e)
        {
            lbSize.Visible = false;
            if (txtSizes.Text != String.Empty)
            {
                btnSaveSize.Enabled = true;
            }
            else
            {
                btnSaveSize.Enabled = false;
            }
        }

        private void lbCatName_Click(object sender, EventArgs e)
        {
            lbCatName.Visible = false;
            txtNewCat.Focus();
        }

        private void txtNewCat_Click(object sender, EventArgs e)
        {
            lbCatName.Visible = false;
            txtNewCat.Focus();
        }

        private void panCat_Click(object sender, EventArgs e)
        {
            if (txtNewCat.Text == String.Empty)
            {
                lbCatName.Visible = true;
                btnSaveCat.Enabled = false;
                panCat.Focus();
            }
        }

        private void panSize_Click(object sender, EventArgs e)
        {
            if (txtSizes.Text == String.Empty)
            {
                lbSize.Visible = true;
                btnSaveSize.Enabled = false;
                panSize.Focus();
            }
        }

        private void txtSizes_Click(object sender, EventArgs e)
        {
            lbSize.Visible = false;
            txtSizes.Focus();
        }

        private void btnNewProduct_Click(object sender, EventArgs e)
        {
            tabProduct.SelectedTab = pageProduct;
            btnNewProduct.ForeColor = Color.Gray;
            btnNewCat.ForeColor = Color.Blue;
            btnNewSize.ForeColor = Color.Blue;
            panNav2.Visible = true;
            panNav2.Location = new Point(215, 33);
            panNav2.Width = 89;
            retrieveItemInfo();
        }
                
        private void btnNewCat_Click(object sender, EventArgs e)
        {
            tabProduct.SelectedTab = pageCat;
            btnNewCat.ForeColor = Color.Gray;
            btnNewProduct.ForeColor = Color.Blue;
            btnNewSize.ForeColor = Color.Blue;
            panNav2.Location = new Point();
            panNav2.Visible = true;
            panNav2.Visible = true;
            panNav2.Location = new Point(15, 33);
            panNav2.Width = 95;
        }

        private void btnNewSize_Click(object sender, EventArgs e)
        {
            tabProduct.SelectedTab = pageSize;
            btnNewSize.ForeColor = Color.Gray;
            btnNewProduct.ForeColor = Color.Blue;
            btnNewCat.ForeColor = Color.Blue;
            panNav2.Visible = true;
            panNav2.Location = new Point(138, 33);
            panNav2.Width = 54;
        }
        //Navigation Control________________________________________________________________________
        private void btnNewCat_MouseHover(object sender, EventArgs e)
        {
            panNav.Visible = true;
            panNav.Location = new Point(15, 33);
            panNav.Width = 95;
            btnNewCat.FlatStyle = FlatStyle.Popup;
        }

        private void btnNewSize_MouseHover(object sender, EventArgs e)
        {
            panNav.Visible = true;
            panNav.Location = new Point(138, 33);
            panNav.Width = 54;
            btnNewSize.FlatStyle = FlatStyle.Popup;
        }

        private void btnNewProduct_MouseHover(object sender, EventArgs e)
        {
            panNav.Visible = true;
            panNav.Location = new Point(215, 33);
            panNav.Width = 89;
            btnNewProduct.FlatStyle = FlatStyle.Popup;
        }

        private void btnNewProduct_MouseLeave(object sender, EventArgs e)
        {
            btnNewProduct.FlatStyle = FlatStyle.Flat;
            panNav.Visible = false;
        }

        private void btnNewSize_MouseLeave(object sender, EventArgs e)
        {
            btnNewSize.FlatStyle = FlatStyle.Flat;
            panNav.Visible = false;
        }

        private void btnNewCat_MouseLeave(object sender, EventArgs e)
        {
             btnNewCat.FlatStyle = FlatStyle.Flat;
            panNav.Visible = false;
        }

        private void btnSizeOn_Click(object sender, EventArgs e)
        {
            fillDgvSize();
            panSizeCtr.Visible = true;
            gbxSize.Size = new Size(680, 282);
        }

        private void btnSizeOff_Click(object sender, EventArgs e)
        {
            panSizeCtr.Visible = false;
            gbxSize.Size = new Size(339, 282);
            cancelEditSize();
        }

        private void btnCatOn_Click(object sender, EventArgs e)
        {
            fillDgvCat();
            gbxCat.Size = new Size(680, 282);
            panCatCtr.Visible = true;
            btnUpdateCat.Enabled = false;
        }

        private void btnCatOff_Click(object sender, EventArgs e)
        {
            panCatCtr.Visible = false;
            gbxCat.Size = new Size(342, 282);
            cancelEditCat();
        }

        private void btnUpdateCat_Click(object sender, EventArgs e)
        {
            if (txtEditCat.Text != String.Empty)
            {
                try
                {
                    DB db = new DB();
                    db.openConnection();
                    SqlCommand cmd = new SqlCommand("Update ProdCategoryTbl set Category=@Category where id=@id", db.con);
                    cmd.Parameters.AddWithValue("@id", txtCatId.Text);
                    cmd.Parameters.AddWithValue("@Category", txtEditCat.Text);
                    cmd.ExecuteNonQuery();
                    db.closeConnection();
                    fillDgvCat();
                    populateCatEntry();
                    btnUpdateCat.Enabled = false;
                    btnEditCat.Text = "Edit";
                    //Sound
                    sfx.Updated();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            cancelEditCat();
        }

        private void btnEditCat_Click(object sender, EventArgs e)
        {
             String editCatEntry = btnEditCat.Text;
            switch (editCatEntry)
            {
                case "Edit":
                    editCat();
                    break;
                case "Cancel":
                    cancelEditCat();
                    break;
                default:
                    return;
            }
        }

        void editCat()
        {
            btnEditCat.Text = "Cancel";
            txtEditCat.Enabled = true;
            btnUpdateCat.Enabled = true;
            txtEditCat.Size = new Size(147, 25);
            btnEditCat.Location = new Point(162, 202);
            btnEditCat.Size = new Size(82, 26);
            txtEditCat.Focus();
        }
        void cancelEditCat()
        {
            btnEditCat.Text = "Edit";
            txtEditCat.Clear();
            txtEditCat.Enabled = false;
            btnEditCat.Size = new Size(62, 26);
            txtEditCat.Size = new Size(166, 25);
            btnEditCat.Location = new Point(182, 202);
            populateCatEntry();
            btnUpdateCat.Enabled = false;
        }

        private void btnUpdateSize_Click(object sender, EventArgs e)
        {
            if (txtEditSize.Text != String.Empty)
            {
                try
                {
                    DB db = new DB();
                    db.openConnection();
                    SqlCommand cmd = new SqlCommand("Update ProdSizeTbl set Size=@Size where id=@id", db.con);
                    cmd.Parameters.AddWithValue("@id", txtSizeId.Text);
                    cmd.Parameters.AddWithValue("@Size", txtEditSize.Text);
                    cmd.ExecuteNonQuery();
                    db.closeConnection();
                    cancelEditSize();
                    fillDgvSize();
                    //Sound
                    sfx.Updated();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnSaveSize_Click(object sender, EventArgs e)
        {
            if (txtSizes.Text != String.Empty)
            {
                try
                {
                    DB db = new DB();
                    db.openConnection();
                    SqlCommand cmd = new SqlCommand("insert into ProdSizeTbl values (@Size)", db.con);
                    cmd.Parameters.AddWithValue("@Size", txtSizes.Text);
                    cmd.ExecuteNonQuery();
                    db.closeConnection();
                    fillDgvSize();
                    //Sound
                    sfx.SizeCreated();

                    btnSaveSize.Enabled = false;
                    txtEditSize.Enabled = false;
                    btnUpdateSize.Enabled = false;
                    txtSizes.Clear();
                    lbSize.Visible = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void fillDgvSize()
        {
            try
            {
                //Get Data..................
                DB db = new DB();
                db.openConnection();
                SqlCommand cmd = new SqlCommand("Select * from ProdSizeTbl", db.con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSize.DataSource = dt;
                db.closeConnection();
                styleDGVSizeHeader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void styleDGVSizeHeader()
        {
            //Set Column size........
            dgvSize.Columns[1].Width = 297;
            //Style DGV Header...................
            dgvSize.Columns[0].Visible = false;
            dgvSize.EnableHeadersVisualStyles = false;
            dgvSize.ColumnHeadersDefaultCellStyle.BackColor = Color.Maroon;
            dgvSize.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSize.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11.0f, FontStyle.Bold, GraphicsUnit.Point);
        }

        private void btnEditSize_Click(object sender, EventArgs e)
        {
            String editSizeEntry = btnEditSize.Text;
            switch (editSizeEntry)
            {
                case "Edit":
                    editSize();
                    break;
                case "Cancel":
                    cancelEditSize();
                    break;
                default:
                    return;
            }
        }

        void editSize()
        {
            btnEditSize.Text = "Cancel";
            txtEditSize.Enabled = true;
            btnUpdateSize.Enabled = true;
            txtEditSize.Size = new Size(147, 25);
            btnEditSize.Location = new Point(163, 203);
            btnEditSize.Size = new Size(79, 26);
        }
        void cancelEditSize()
        {
            btnEditSize.Text = "Edit";
            txtEditSize.Clear();
            txtEditSize.Enabled = false;
            btnEditSize.Size = new Size(62, 26);
            txtEditSize.Size = new Size(164, 25);
            btnEditSize.Location = new Point(180, 203);
            populateSizeEntry();
            btnUpdateSize.Enabled = false;
        }

        private void btnEuro_Click(object sender, EventArgs e)
        {
            txtCurrency.Text = "euro";
            btnEuro.BackColor = Color.Tan;
            btnEuro.Enabled = false;

            btnDollar.Enabled = true;
            btnPound.Enabled = true;
            btnNaira.Enabled = true;

            btnDollar.BackColor = Color.Transparent;
            btnPound.BackColor = Color.Transparent;
            btnNaira.BackColor = Color.Transparent;
        }

        private void btnDollar_Click(object sender, EventArgs e)
        {
            txtCurrency.Text = "dollars";
            btnDollar.BackColor = Color.Tan;
            btnDollar.Enabled = false;

            btnEuro.Enabled = true;
            btnPound.Enabled = true;
            btnNaira.Enabled = true;

            btnEuro.BackColor = Color.Transparent;
            btnPound.BackColor = Color.Transparent;
            btnNaira.BackColor = Color.Transparent;
        }

        private void btnPound_Click(object sender, EventArgs e)
        {
            txtCurrency.Text = "pounds";
            btnPound.BackColor = Color.Tan;
            btnPound.Enabled = false;

            btnEuro.Enabled = true;
            btnDollar.Enabled = true;
            btnNaira.Enabled = true;

            btnEuro.BackColor = Color.Transparent;
            btnDollar.BackColor = Color.Transparent;
            btnNaira.BackColor = Color.Transparent;
        }

        private void btnNaira_Click(object sender, EventArgs e)
        {
            txtCurrency.Text = "naira";
            btnNaira.BackColor = Color.Tan;
            btnNaira.Enabled = false;

            btnEuro.Enabled = true;
            btnDollar.Enabled = true;
            btnPound.Enabled = true;

            btnEuro.BackColor = Color.Transparent;
            btnDollar.BackColor = Color.Transparent;
            btnPound.BackColor = Color.Transparent;
        }

        private void btnNewProd_Click(object sender, EventArgs e)
        {
            lbProductHeader.Text = "New Product";
            enableProdEntry_New();
            pbxItemImage.BackgroundImage = AmsordMall.Properties.Resources.Product4;

            btnEditProd.Text = "Cancel";
            btnNewProd.Visible = false;
            btnSaveProd.Visible = true;
            btnEuro.BackColor = Color.Transparent;
            btnDollar.BackColor = Color.Transparent;
            btnPound.BackColor = Color.Transparent;
            btnNaira.BackColor = Color.Transparent;

            txtCurrency.Clear();
            dtpDateAdded.Text = DateTime.Now.ToString();
            dtpLastUpdate.Text = DateTime.Now.ToString();
            fillCbxSup();
            fillCbxSize();
            fillCbxCategory();
        }
        //________________________________________________________________________________
        private void fillCbxSup()
        {
            try 
            {
                //Get Data..................
                cbxSupply.Items.Clear();
                DB db = new DB();
                db.openConnection();
                SqlCommand cmd = new SqlCommand("Select CompanyName from SupplierTbl", db.con);
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    cbxSupply.Items.Add(dr["CompanyName"].ToString());
                }
                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fillCbxSize()
        {
            try
            {
                //Get Data..................
                cbxSize.Items.Clear();
                DB db = new DB();
                db.openConnection();
                SqlCommand cmd = new SqlCommand("Select Size from ProdSizeTbl", db.con);
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    cbxSize.Items.Add(dr["Size"].ToString());
                }
                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fillCbxCategory()
        {
            try
            {
                //Get Data..................
                cbxCategory.Items.Clear();
                DB db = new DB();
                db.openConnection();
                SqlCommand cmd = new SqlCommand("Select Category from ProdCategoryTbl", db.con);
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    cbxCategory.Items.Add(dr["Category"].ToString());
                }
                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //__________________________________PRODUCT__EDIT__&__CANCEL___________
        private void btnEditProd_Click(object sender, EventArgs e)
        {
            String editProdEntry = btnEditProd.Text;
            switch (editProdEntry)
            {
                case "Edit":
                    lbProductHeader.Text = "Edit Product";
                    //lbProductHeader.TextAlign = HorizontalAlignment.Center;

                    enableProdEntry_Edit();
                    btnUpdateProd.Enabled = true;
                    btnEditProd.Text = "Cancel";
                    btnNewProd.Enabled = false;
                    panCam.Visible = true;
                    dtpLastUpdate.Value = DateTime.Now;
                    fillCbxSize();
                    fillCbxCategory();
                    fillCbxSup();
                    break;
                case "Cancel":
                    retrieveItemInfo();
                    //lbProductHeader.Text = "Product";
                    disableProdEntry();
                    btnUpdateProd.Enabled = false;
                    btnEditProd.Text = "Edit";
                    btnNewProd.Enabled = true;
                    break;
                default:
                    return;
            }
        }

        private void enableProdEntry_Edit()
        {
            enableCurrency();
            panCam.Visible = true;

            txtBarcode.Enabled = true;
            txtProdName.Enabled = true;
            txtProdDesc.Enabled = true;
            txtCost.ReadOnly = false;
            txtSellingPrice.ReadOnly = false;

            lbProdDesc.Visible = false;
            lbProdDesc2.Visible = false;

            cbxSize.Enabled = true;
            cbxSupply.Enabled = true;
            cbxCategory.Enabled = true;
            dtpDateAdded.Enabled = false;
            dtpLastUpdate.Text = DateTime.Now.ToString();
        }

        private void disableProdEntry()
        {
            disableCurrency();
            panCam.Visible = false;
            //RefreshDatabase
            btnNewProd.Visible = true;
            btnSaveProd.Visible = false;

            txtProdName.Enabled = false;
            txtProdDesc.Enabled = false;
            txtCost.ReadOnly = true;
            txtSellingPrice.ReadOnly = true;
            txtBarcode.Enabled = false;

            lbProdDesc.Visible = false;
            lbProdDesc2.Visible = false;

            cbxSize.Enabled = false;
            cbxSupply.Enabled = false;
            cbxCategory.Enabled = false;
            dtpDateAdded.Enabled = false;
            dtpLastUpdate.Enabled = false;
        }

        private void enableProdEntry_New()
        {
            clearProdEntry();
            enableProdEntry_Edit();
            panCam.Visible = true;

            txtQinS.Text = "0";
            txtCost.Text = "0.01";
            txtCost.ReadOnly = false;
            txtSellingPrice.Text = "0.01";
            txtSellingPrice.ReadOnly = false;
            txtBarcode.Text = "--------------";

            lbProdDesc.Visible = true;
            lbProdDesc2.Visible = true;

            cbxSize.Text = "Select Size";
            cbxSupply.Text = "Choose a Supplier";
            cbxCategory.Text = "Select Category";
        }
        private void clearProdEntry()
        {
            txtQinS.Clear();
            txtCost.Clear();
            txtSellingPrice.Clear();
            txtProdName.Clear();
            txtProdDesc.Clear();
            txtBarcode.Clear();
        }

        private void txtProdName_Click(object sender, EventArgs e)
        {
            if (lbProdDesc.Enabled == true)
            {
                lbProdDesc.Visible = false;
                txtProdName.Enabled = true;
                txtProdName.Focus();
            }
        }

        private void lbProdDesc_Click(object sender, EventArgs e)
        {
            if (lbProdDesc.Enabled == true)
            {
                lbProdDesc.Visible = false;
                txtProdName.Enabled = true;
                txtProdName.Focus();
            }
        }

        private void lbProdDesc2_Click(object sender, EventArgs e)
        {
            if (lbProdDesc2.Visible == true)
            {
                lbProdDesc2.Visible = false;
                txtProdDesc.Enabled = true;
                txtProdDesc.Focus();
            }
        }

        private void txtProdDesc_Click(object sender, EventArgs e)
        {
            if (lbProdDesc2.Visible == true)
            {
                lbProdDesc2.Visible = false;
                txtProdDesc.Enabled = true;
                txtProdDesc.Focus();
            }
        }
        //Currency Settings.....................Start
        private void enableCurrency()
        {
            btnEuro.Enabled = true;
            btnDollar.Enabled = true;
            btnPound.Enabled = true;
            btnNaira.Enabled = true;
        }

        private void disableCurrency()
        {
            btnEuro.Enabled = false;
            btnDollar.Enabled = false;
            btnPound.Enabled = false;
            btnNaira.Enabled = false;
            if (txtCurrency.Text == "euro")
            {                
                // btnEuro.PerformClick();
                btnEuro.BackColor = Color.Tan;
            }
            else if (txtCurrency.Text == "dollars")
            {
               btnDollar.BackColor = Color.Tan;
            }       
            else if (txtCurrency.Text == "pounds")
            {
                //btnPound.PerformClick();
                btnPound.BackColor = Color.Tan;
            }
            else if (txtCurrency.Text == "naira")
            {
                //btnNaira.PerformClick();
                btnNaira.BackColor = Color.Tan;
            }
        }
 //Currency Settings.....................End
        private void btnSaveProd_Click(object sender, EventArgs e)
        {
            saveItemInfo();
            disableProdEntry();
            btnUpdateProd.Enabled = false;
            btnEditProd.Text = "Edit";
            pbxItemImage.BackgroundImage = AmsordMall.Properties.Resources.Product4;
        }
 //____________________________________________________SAVE INVENTORY__________________________________________
        private void saveItemInfo()
        {
            if (txtBarcode.Text == String.Empty || txtProdName.Text == String.Empty || txtProdDesc.Text == String.Empty || txtPicLocation.Text == String.Empty ||
               cbxSupply.Text == String.Empty || cbxSize.Text == String.Empty || txtCurrency.Text == String.Empty || cbxCategory.Text == String.Empty)
            {
                MessageBox.Show("Ensure no field is empty", "Warning!");
            }
            else
            {
                try
                {
                    DB db = new DB();
                    db.openConnection();
                    SqlCommand cmd = new SqlCommand("insert into InventoryTbl values (@Barcode,@ItemName,@ItemImg,@Supplier,@Category,@Size,@Description,@Cost,@SellingPrice,@Stock,@DateAdded,@Currency,@LastUpdated)", db.con);
                    cmd.Parameters.AddWithValue("@Barcode", txtBarcode.Text);
                    cmd.Parameters.AddWithValue("@ItemName", txtProdName.Text);
                            
                    MemoryStream memoStr = new MemoryStream();
                    pbxItemImage.Image.Save(memoStr, pbxItemImage.Image.RawFormat);
                    cmd.Parameters.AddWithValue("@ItemImg", memoStr.ToArray());

                    cmd.Parameters.AddWithValue("@Supplier", cbxSupply.Text);
                    cmd.Parameters.AddWithValue("@Category", cbxCategory.Text);
                    cmd.Parameters.AddWithValue("@Size", cbxSize.Text);

                    cmd.Parameters.AddWithValue("@Description", txtProdDesc.Text);

                    cmd.Parameters.AddWithValue("@Cost", txtCost.Text);
                    cmd.Parameters.AddWithValue("@SellingPrice", txtSellingPrice.Text);
                    cmd.Parameters.AddWithValue("@Stock", int.Parse(txtQinS.Text));

                    cmd.Parameters.AddWithValue("@DateAdded", dtpDateAdded.Value);
                    cmd.Parameters.AddWithValue("@Currency", txtCurrency.Text);
                    cmd.Parameters.AddWithValue("@LastUpdated", dtpLastUpdate.Value);
                    cmd.ExecuteNonQuery();
                    db.closeConnection();
                    //fillSupDgv();
                    //Sound
                    //sfx.SaveSupplier();
                    MessageBox.Show("Congratulation! A New Product is Saved", "Message");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                retrieveItemInfo();
            }
        }
        //____________________________________________________UPDATE INVENTORY__________________________________________
        private void updateItemInfo()
        {
                try
                {
                    DB db = new DB();
                    db.openConnection();
                    SqlCommand cmd = new SqlCommand("Update InventoryTbl set Barcode = @Barcode, ItemName = @ItemName, ItemImg = @ItemImg, Supplier = @Supplier, " +
                        "Category = @Category, Size = @Size, Description = @Description, Cost = @Cost, SellingPrice = @SellingPrice, " +
                        "LastUpdated = @LastUpdated Where id=@id", db.con);
                    
                    cmd.Parameters.AddWithValue("@id", txtProd_id.Text);
                    cmd.Parameters.AddWithValue("@Barcode", txtBarcode.Text);
                    cmd.Parameters.AddWithValue("@ItemName", txtProdName.Text);

                    MemoryStream memoStr = new MemoryStream();
                    pbxItemImage.Image.Save(memoStr, pbxItemImage.Image.RawFormat);
                    cmd.Parameters.AddWithValue("@ItemImg", memoStr.ToArray());

                    cmd.Parameters.AddWithValue("@Supplier", cbxSupply.Text);
                    cmd.Parameters.AddWithValue("@Category", cbxCategory.Text);
                    cmd.Parameters.AddWithValue("@Size", cbxSize.Text);

                    cmd.Parameters.AddWithValue("@Description", txtProdDesc.Text);

                    cmd.Parameters.AddWithValue("@Cost", txtCost.Text);
                    cmd.Parameters.AddWithValue("@SellingPrice", txtSellingPrice.Text);
                    dtpLastUpdate.Enabled = true;
                    cmd.Parameters.AddWithValue("@LastUpdated", dtpLastUpdate.Value);
                    cmd.ExecuteNonQuery();
                    db.closeConnection();
                    //Sound
                    //sfx.SaveSupplier();
                    MessageBox.Show("Updated", "Message");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                retrieveItemInfo();
        }

        //_________________________________________DISPLAY INVENTORY_________________________________________
        private void retrieveItemInfo()
        {
                try
                {
                    DB db = new DB();
                    db.openConnection();
                    SqlCommand cmd = new SqlCommand("SELECT ItemImg, Barcode, ItemName, Supplier, Category, Size, Description, Cost, SellingPrice, Stock, DateAdded, Currency, LastUpdated, id FROM InventoryTbl", db.con);
                 
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    dt.Clear();
                    da.Fill(dt);
                    dgvInventory.RowTemplate.Height = 75;
                    dgvInventory.DataSource = dt;
                    DataGridViewImageColumn itemPic = new DataGridViewImageColumn();
                    itemPic = (DataGridViewImageColumn)dgvInventory.Columns[0];
                    itemPic.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    db.closeConnection();
                    styleDgvInventoryHeader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            lbProductHeader.Text = "Product";
        }

        //_________________________________________________FORMAT INVENTORY DGV___________________________
        public void styleDgvInventoryHeader()
        {
            dgvInventory.Columns[13].Visible = false;
            dgvInventory.EnableHeadersVisualStyles = false;
            dgvInventory.ColumnHeadersDefaultCellStyle.BackColor = Color.SeaGreen;
            dgvInventory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvInventory.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11.0f, FontStyle.Bold, GraphicsUnit.Point);
        }
//________________________________________________________________________UPDATE__________
        private void btnUpdateProd_Click(object sender, EventArgs e)
        {
           // lbProductHeader.Text = "Product";
            updateItemInfo();
            btnEditProd.Text = "Edit";
            btnUpdateProd.Enabled = false;
            disableProdEntry();
        }
        //_______________________________________________POPULATE___Inventory___Entry____________
        public void popInvEntry()
        {
            try
            {
                lbProdDesc.Visible = false;
                lbProdDesc2.Visible = false;

                txtProd_id.Text = dgvInventory.CurrentRow.Cells["id"].Value.ToString();
                txtBarcode.Text = dgvInventory.CurrentRow.Cells["Barcode"].Value.ToString();
                txtProdName.Text = dgvInventory.CurrentRow.Cells["ItemName"].Value.ToString();
                cbxSupply.Text = dgvInventory.CurrentRow.Cells["Supplier"].Value.ToString();
                cbxCategory.Text = dgvInventory.CurrentRow.Cells["Category"].Value.ToString();
                cbxSize.Text = dgvInventory.CurrentRow.Cells["Size"].Value.ToString();
                txtProdDesc.Text = dgvInventory.CurrentRow.Cells["Description"].Value.ToString();
                txtCost.Text = dgvInventory.CurrentRow.Cells["Cost"].Value.ToString();
                txtSellingPrice.Text = dgvInventory.CurrentRow.Cells["SellingPrice"].Value.ToString();
                txtQinS.Text = dgvInventory.CurrentRow.Cells["Stock"].Value.ToString();
                //DateAddded
                DateTime dateAdded;
                if (DateTime.TryParse(dgvInventory.CurrentRow.Cells["DateAdded"].Value.ToString(), out dateAdded))
                {
                    dtpDateAdded.Value = dateAdded;
                }
                //LastUpdate
                DateTime lastUpdated;
                if (DateTime.TryParse(dgvInventory.CurrentRow.Cells["LastUpdated"].Value.ToString(), out lastUpdated))
                {
                    dtpLastUpdate.Value = lastUpdated;
                }
                txtCurrency.Text = dgvInventory.CurrentRow.Cells["Currency"].Value.ToString();
                //Load Item Image Into Picture Box
                MemoryStream ms = new MemoryStream((byte[])dgvInventory.CurrentRow.Cells["ItemImg"].Value);
                pbxItemImage.Image = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //________________________________________________________________________________________

        private void panCam_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|All Files(*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = ofd.OpenFile()) != null)
                    {
                        string picLocation = ofd.FileName.ToString();
                        if (myStream.Length > 512000)
                        {
                            MessageBox.Show("File size is too large!", "Message");
                        }
                        else
                        {
                            txtPicLocation.Text = picLocation;
                            pbxItemImage.ImageLocation = picLocation;
                        }
                    }
                    pbxItemImage.SizeMode = PictureBoxSizeMode.StretchImage;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnSaveCat_Click(object sender, EventArgs e)
        {
            if (txtNewCat.Text  != String.Empty)
            {
                try
                {
                    DB db = new DB();
                    db.openConnection();
                    SqlCommand cmd = new SqlCommand("insert into ProdCategoryTbl values (@Category)", db.con);
                    cmd.Parameters.AddWithValue("@Category", txtNewCat.Text);
                    cmd.ExecuteNonQuery();
                    db.closeConnection();
                    fillDgvCat();

                    //Sound
                    sfx.CategoryCreated();

                    btnSaveCat.Enabled = false;
                    txtEditCat.Enabled = false;
                    btnUpdateCat.Enabled = false;
                    txtNewCat.Clear();
                    lbCatName.Visible = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
                populateCatEntry();
        }

        private void fillDgvCat()
        {
            try
            {
                //Get Data..................
                DB db = new DB();
                db.openConnection();
                SqlCommand cmd = new SqlCommand("Select * from ProdCategoryTbl", db.con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCat.DataSource = dt;
                db.closeConnection();
                styleDGVCatHeader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void styleDGVCatHeader()
        {
            //Set Column size........
            dgvCat.Columns[1].Width = 296;
            dgvCat.Columns[0].Visible = false;
            dgvCat.EnableHeadersVisualStyles = false;
            dgvCat.ColumnHeadersDefaultCellStyle.BackColor = Color.Maroon;
            dgvCat.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCat.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10.0f, FontStyle.Bold, GraphicsUnit.Point);
        }

        private void dgvCat_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            populateCatEntry();
        }

        public void populateCatEntry()
        {
            try
            {
                
                txtCatId.Text = dgvCat.CurrentRow.Cells["id"].Value.ToString();
                txtEditCat.Text = dgvCat.CurrentRow.Cells["Category"].Value.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void txtEditCat_TextChanged(object sender, EventArgs e)
        {
            if (txtEditCat.Text == String.Empty)
            {
                btnUpdateCat.Enabled = false;
            }
            else
            {
                btnUpdateCat.Enabled = true;
            }
        }

        private void panCat_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtCatSearch_TextChanged(object sender, EventArgs e)
        {
            DB db = new DB();
            DataTable dt = new DataTable();

            if (txtCatSearch.Text != String.Empty)
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM ProdCategoryTbl WHERE [Category] LIKE '" + txtCatSearch.Text + "%'", db.con);
                sda.Fill(dt);
                dgvCat.DataSource = dt;
            }
            else
            {
                fillDgvCat();
            }
        }

        private void dgvSize_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            populateSizeEntry();
        }
        public void populateSizeEntry()
        {
            try
            {
                txtSizeId.Text = dgvSize.CurrentRow.Cells["id"].Value.ToString();
                txtEditSize.Text = dgvSize.CurrentRow.Cells["Size"].Value.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void txtSizeSearch_TextChanged(object sender, EventArgs e)
        {
            DB db = new DB();
            DataTable dt = new DataTable();

            if (txtSizeSearch.Text != String.Empty)
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM ProdSizeTbl WHERE [Size] LIKE '" + txtSizeSearch.Text + "%'", db.con);
                sda.Fill(dt);
                dgvSize.DataSource = dt;
            }
            else
            {
                fillDgvSize();
            }
        }

        private void dgvInventory_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            popInvEntry();
        }

        private void txtCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Allow only one dot character
            if (e.KeyChar == '.' && ((TextBox)sender).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txtSellingPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Allow only one dot character
            if (e.KeyChar == '.' && ((TextBox)sender).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void lbProductHeader_TextChanged(object sender, EventArgs e)
        {
          Label lb = new Label();
          lb.AutoSize = false;
          lb.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void btnRemoveProd_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Remove Product! Are you sure?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {                      
                try
                {
                    //Get Data..................
                    DB db = new DB();
                    db.openConnection();
                    SqlCommand cmd = new SqlCommand("Delete from InventoryTbl Where id=@id", db.con);
                    cmd.Parameters.AddWithValue("@id", int.Parse(txtProd_id.Text));
                    cmd.ExecuteNonQuery();
                    db.closeConnection();
                    retrieveItemInfo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                return;
            }
        }
//SEARCH____________________________________________________________________________________________________START_______
        private void txtSearchProd_TextChanged(object sender, EventArgs e)
        {
                try
                {
                    DB db = new DB();
                    DataTable dt = new DataTable();

                    if (radProdName.Checked)
                    {
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemImg, Barcode, ItemName, Supplier, Category, Size, Description, Cost, SellingPrice, Stock, DateAdded, Currency, LastUpdated, id FROM InventoryTbl WHERE ItemName LIKE '" + txtSearchProd.Text + "%'", db.con);
                        sda.Fill(dt);
                        dgvInventory.DataSource = dt;
                    }
                    else if (radBarcode.Checked)
                    {
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT ItemImg, Barcode, ItemName, Supplier, Category, Size, Description, Cost, SellingPrice, Stock, DateAdded, Currency, LastUpdated, id FROM InventoryTbl WHERE Barcode LIKE '" + txtSearchProd.Text + "%'", db.con);
                        sda.Fill(dt);
                        dgvInventory.DataSource = dt;
                    }
                    else if (radProdName.Checked || radBarcode.Checked && txtSearchProd.Text != String.Empty)
                    {
                        retrieveItemInfo();
                    }
                        styleDgvInventoryHeader(); 
                }

            catch (Exception ex)
                {

                    MessageBox.Show("Unable to Search Item" + ex.Message);
                }
            //SEARCH____________________________________________________________________________________________________END_______
        }

        private void panCam_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
