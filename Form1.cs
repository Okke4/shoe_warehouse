using MaterialSkin.Controls;
using MySqlConnector;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WindowsFormsApp1
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        readonly DB db = new DB();

        private void Form1_Load(object sender, EventArgs e)
        {
            updateGridProduct();
            updateGridCustomers();
            updateGridInvoice();
            updateGridWayBill();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox8.Enabled = true;
                textBox5.Enabled = false;
                textBox7.Enabled = false;
                textBox5.Text = string.Empty;
                textBox7.Text = string.Empty;
            }
            else
            {
                textBox8.Enabled = false;
                textBox5.Enabled = true;
                textBox7.Enabled = true;
                textBox8.Text = string.Empty;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                textBox12.Enabled = true;
                textBox15.Enabled = false;
                textBox13.Enabled = false;
                textBox15.Text = string.Empty;
                textBox13.Text = string.Empty;
            }
            else
            {
                textBox12.Enabled = false;
                textBox15.Enabled = true;
                textBox13.Enabled = true;
                textBox12.Text = string.Empty;
            }
        }

        private void button1_Click(object sender, EventArgs e) //добавление товара
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                db.openConnection();
                MySqlCommand command = new MySqlCommand("INSERT INTO `product` (`product_name`, `price`, `storage_rack`, `cell`) VALUES (@product_name, @price, @storage_rack, @cell)", db.getConnection());
                command.Parameters.Add("@product_name", MySqlDbType.Text).Value = textBox1.Text;
                command.Parameters.Add("@storage_rack", MySqlDbType.Text).Value = textBox2.Text;
                command.Parameters.Add("@cell", MySqlDbType.Text).Value = textBox3.Text;
                command.Parameters.Add("@price", MySqlDbType.Text).Value = textBox4.Text;
                command.ExecuteNonQuery();
                db.closeConnection();
                updateGridProduct();
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
            }
        }

        private void updateGridProduct() //обновление таблиы товары
        {
            try
            {
                db.openConnection();
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT id, product_name AS Название, price AS Цена, storage_rack AS Стеллаж, cell AS Ячейка FROM product", db.getConnection());
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateGridCustomers() //обновление таблиы покупатели
        {
            try
            {
                db.openConnection();
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT id, surname AS Фамилия, name AS Имя, lastname AS Отчество, TIN AS УПН FROM customers", db.getConnection());
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView2.DataSource = dt;
                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateGridInvoice()
        {
            try
            {
                db.openConnection();
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT id, customer AS Покупатель, time_start AS `Открытие счета`, time_lost AS `Закрытие счета`, paid AS Оплачен FROM invoice", db.getConnection());
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView4.DataSource = dt;
                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateGridWayBill()
        {
            try
            {
                db.openConnection();
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT id, customer AS Покупатель, time AS Дата FROM waybill", db.getConnection());
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView5.DataSource = dt;
                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void materialTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGridProduct();
            updateGridCustomers();
            updateGridInvoice();
            updateGridWayBill();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //выбор товара
        {
            int product_id = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `product_name` FROM `product` WHERE id = '" + product_id + "'", db.getConnection());
            textBox17.Text = command.ExecuteScalar().ToString();
            MySqlCommand command2 = new MySqlCommand("SELECT `storage_rack` FROM `product` WHERE id = '" + product_id + "'", db.getConnection());
            textBox18.Text = command2.ExecuteScalar().ToString();
            MySqlCommand command3 = new MySqlCommand("SELECT `cell` FROM `product` WHERE id = '" + product_id + "'", db.getConnection());
            textBox19.Text = command3.ExecuteScalar().ToString();
            MySqlCommand command4 = new MySqlCommand("SELECT `price` FROM `product` WHERE id = '" + product_id + "'", db.getConnection());
            textBox20.Text = command4.ExecuteScalar().ToString();
            db.closeConnection();
        }

        private void button9_Click(object sender, EventArgs e) //редактирование товара
        {
            if (textBox17.Text != "" && textBox18.Text != "" && textBox19.Text != "" && textBox20.Text != "")
            {
                int product_id = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
                MySqlCommand command = new MySqlCommand("UPDATE `product` SET `product_name` = @product_name, `price` = @price, `storage_rack` = @storage_rack, `cell` = @cell WHERE `id` = @product_id;", db.getConnection());
                command.Parameters.Add("@product_id", MySqlDbType.Int32).Value = product_id;
                command.Parameters.Add("@product_name", MySqlDbType.Text).Value = textBox17.Text;
                command.Parameters.Add("@storage_rack", MySqlDbType.Text).Value = textBox18.Text;
                command.Parameters.Add("@cell", MySqlDbType.Text).Value = textBox19.Text;
                command.Parameters.Add("@price", MySqlDbType.Text).Value = textBox20.Text;
                db.openConnection();
                command.ExecuteNonQuery();
                db.closeConnection();
                updateGridProduct();
                textBox17.Text = string.Empty; 
                textBox18.Text = string.Empty; 
                textBox19.Text = string.Empty;
                textBox20.Text = string.Empty;
            }
            else MessageBox.Show("Заполните все поля!");
        }

        private void button12_Click(object sender, EventArgs e) //удаление товара
        {
            if (dataGridView1.RowCount > 0)
            {
                int product_id = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
                db.openConnection();
                MySqlDataAdapter adapter = new MySqlDataAdapter("DELETE FROM product WHERE id = " + product_id, db.getConnection());
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                db.closeConnection();
                updateGridProduct();
            }
        }

        private void button2_Click(object sender, EventArgs e) //добавление пользователя
        {
            if ((textBox5.Text != "" && textBox6.Text != "" && textBox7.Text != "") || (textBox6.Text != "" && textBox8.Text != ""))
            {
                db.openConnection();
                if (checkBox1.Checked)
                {
                    MySqlCommand command = new MySqlCommand("INSERT INTO `customers` (`name`, `TIN`) VALUES (@name, @TIN)", db.getConnection());
                    command.Parameters.Add("@name", MySqlDbType.Text).Value = textBox6.Text;
                    command.Parameters.Add("@TIN", MySqlDbType.Text).Value = textBox8.Text;
                    command.ExecuteNonQuery();
                }
                else 
                {
                    MySqlCommand command = new MySqlCommand("INSERT INTO `customers` (`surname`, `name`, `lastname`) VALUES (@surname, @name, @lastname)", db.getConnection());
                    command.Parameters.Add("@surname", MySqlDbType.Text).Value = textBox5.Text;
                    command.Parameters.Add("@name", MySqlDbType.Text).Value = textBox6.Text;
                    command.Parameters.Add("@lastname", MySqlDbType.Text).Value = textBox7.Text;
                    command.ExecuteNonQuery();
                }
                db.closeConnection();
                updateGridCustomers();
                textBox5.Text = string.Empty;
                textBox6.Text = string.Empty;
                textBox7.Text = string.Empty;
                textBox8.Text = string.Empty;
                checkBox1.Checked = false;
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
            }
        }

        private void button8_Click(object sender, EventArgs e) //редактирование пользователя
        {
            if ((textBox15.Text != "" && textBox14.Text != "" && textBox13.Text != "") || (textBox14.Text != "" && textBox12.Text != ""))
            {
                db.openConnection();
                int customer_id = Convert.ToInt32(dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString());
                if (checkBox2.Checked)
                {
                    MySqlCommand command = new MySqlCommand("UPDATE `customers` SET `name` = @name, `TIN` = @TIN WHERE `id` = @product_id;", db.getConnection());
                    command.Parameters.Add("@product_id", MySqlDbType.Int32).Value = customer_id;
                    command.Parameters.Add("@name", MySqlDbType.Text).Value = textBox14.Text;
                    command.Parameters.Add("@TIN", MySqlDbType.Text).Value = textBox12.Text;
                    command.ExecuteNonQuery();
                }
                else
                {
                    MySqlCommand command = new MySqlCommand("UPDATE `customers` SET `surname` = @surname, `name` = @name, `lastname` = @lastname WHERE `id` = @customer_id;", db.getConnection());
                    command.Parameters.Add("@product_id", MySqlDbType.Int32).Value = customer_id;
                    command.Parameters.Add("@surname", MySqlDbType.Text).Value = textBox15.Text;
                    command.Parameters.Add("@name", MySqlDbType.Text).Value = textBox14.Text;
                    command.Parameters.Add("@lastname", MySqlDbType.Text).Value = textBox13.Text;
                    command.ExecuteNonQuery();
                }
                db.closeConnection();
                updateGridCustomers();
                textBox15.Text = string.Empty;
                textBox14.Text = string.Empty;
                textBox13.Text = string.Empty;
                textBox12.Text = string.Empty;
                checkBox2.Checked = false;
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
            }
        }

        private void button7_Click(object sender, EventArgs e) //удаление пользователя
        {
            if (dataGridView2.RowCount > 0)
            {
                int customer_id = Convert.ToInt32(dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString());
                db.openConnection();
                MySqlDataAdapter adapter = new MySqlDataAdapter("DELETE FROM customers WHERE id = " + customer_id, db.getConnection());
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                db.closeConnection();
                updateGridCustomers();
            }
        }

        private void numberCheck(object sender, KeyPressEventArgs e) //ввод только числовых значений
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void comboBox1_DropDown(object sender, EventArgs e) //выгрузка списка товаров из бд
        {
            comboBox1.Items.Clear();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM product", db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["product_name"].ToString());
            }
            db.closeConnection();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e) //выбор покупателя
        {
            int customer_id = Convert.ToInt32(dataGridView2[0, dataGridView2.CurrentRow.Index].Value.ToString());
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `surname` FROM `customers` WHERE id = '" + customer_id + "'", db.getConnection());
            textBox15.Text = command.ExecuteScalar().ToString();
            MySqlCommand command2 = new MySqlCommand("SELECT `name` FROM `customers` WHERE id = '" + customer_id + "'", db.getConnection());
            textBox14.Text = command2.ExecuteScalar().ToString();
            MySqlCommand command3 = new MySqlCommand("SELECT `lastname` FROM `customers` WHERE id = '" + customer_id + "'", db.getConnection());
            textBox13.Text = command3.ExecuteScalar().ToString();
            MySqlCommand command4 = new MySqlCommand("SELECT `TIN` FROM `customers` WHERE id = '" + customer_id + "'", db.getConnection());
            textBox12.Text = command4.ExecuteScalar().ToString();
            if (textBox12.Text != "0") checkBox2.Checked = true;
            else checkBox2.Checked = false;
            if (textBox12.Text == "0") textBox12.Text = "";
            db.closeConnection();
        }

        private void comboBox2_DropDown(object sender, EventArgs e) //выгрузка покупателей в список из бд
        {
            comboBox2.Items.Clear();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM customers", db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader["TIN"].ToString() != "0") comboBox2.Items.Add(reader["name"].ToString());
                else comboBox2.Items.Add(reader["surname"].ToString()+" "+reader["name"].ToString()+" "+reader["lastname"].ToString());
            }
            db.closeConnection();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) //открытие\закрытие полей
        {
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `TIN` FROM `customers` WHERE name = '" + (comboBox2.Text).Trim() + "'", db.getConnection());
            if (command.ExecuteReader().HasRows)
            {
                db.closeConnection();
                db.openConnection();
                if (command.ExecuteReader().ToString() != "0")
                {
                    comboBox4.SelectedIndex = 1;
                    comboBox4.Enabled = false;
                }
            }
            else 
            { 
                comboBox4.SelectedIndex = -1; 
                comboBox4.Enabled = true;
            }
            db.closeConnection();
        }

        private void button3_Click(object sender, EventArgs e) //добавление в корзину
        {
            if (dataGridView3.RowCount > 0)
            {
                db.openConnection();
                MySqlCommand command = new MySqlCommand("SELECT * FROM product WHERE product_name = '" + comboBox1.Text + "'", db.getConnection());
                MySqlDataReader dr = command.ExecuteReader();
                dr.Read();
                int rowNumber = dataGridView3.Rows.Add();
                dataGridView3.Rows[rowNumber].Cells[0].Value = dr.GetValue(0);
                dataGridView3.Rows[rowNumber].Cells[1].Value = dr.GetValue(1);
                dataGridView3.Rows[rowNumber].Cells[2].Value = dr.GetValue(2);
                dataGridView3.Rows[rowNumber].Cells[3].Value = numericUpDown1.Value;
                dataGridView3.Rows[rowNumber].Cells[4].Value = (Convert.ToInt32(dr.GetValue(2)) * numericUpDown1.Value);
                db.closeConnection();
                int sum = 0;
                for (int i = 0; i < dataGridView3.Rows.Count; ++i)
                {
                    sum += Convert.ToInt32(dataGridView3.Rows[i].Cells[4].Value);
                }
                textBox9.Text = sum.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e) //удаление корзины
        {
            if (dataGridView3.RowCount > 0)
            {
                dataGridView3.Rows.RemoveAt(dataGridView3.CurrentRow.Index);
                int sum = Convert.ToInt32(textBox9.Text);
                sum -= Convert.ToInt32(dataGridView3.CurrentRow.Cells[4].Value);
                textBox9.Text = sum.ToString();
            }
        }

        private void button13_Click(object sender, EventArgs e) //оплата
        {
            if (comboBox4.Text != "" && comboBox2.Text != "" && textBox9.Text != "")
            {
                if (comboBox4.SelectedIndex == 0) //наличка
                {
                    GenerateWayBillPDF(sender, e);
                }
                else if (comboBox4.SelectedIndex == 1) //безнал
                {
                    GenerateInvoicePDF(sender, e);
                    GenerateWayBillPDF(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
            }
        }

        protected void GenerateInvoicePDF(object sender, EventArgs e)
        {
            //Variables to create PDF document
            Document doc = new Document();
            DateTime date = DateTime.Now;
            string namefile = (date.ToString()).Replace(".","").Replace(":", "").Replace(" ", "");
            PdfWriter.GetInstance(doc, new FileStream(@".\PDFs\"+namefile+".pdf", FileMode.Create)); //Output PDF file
            PdfPTable table = new PdfPTable(5); //To create table inside the PDF

            //Width of each column on table
            float[] widths = { 50f, 150f, 70f, 100f, 110f};
            table.SetWidthPercentage(widths, PageSize.A4);

            //Font settings
            Font titleFont = FontFactory.GetFont("C:\\Windows\\Fonts\\arial.ttf", "Identity-H");
            titleFont.SetStyle("bold");
            Font textFont = FontFactory.GetFont("C:\\Windows\\Fonts\\arial.ttf", "Identity-H");
            titleFont.SetStyle("normal");

            doc.Open(); //Access the PDF Document to write data

            //Add table column headers
            table.AddCell(new Paragraph("id", titleFont));
            table.AddCell(new Paragraph("Название", titleFont));
            table.AddCell(new Paragraph("Цена", titleFont));
            table.AddCell(new Paragraph("Количество", titleFont));
            table.AddCell(new Paragraph("Всего", titleFont));

            //Read the data and store them in the list
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    table.AddCell(new Paragraph(cell.Value.ToString(), textFont));
                }
            }

            //Add data to PDF file
            DateTime date_start = DateTime.Now;
            Paragraph bill = new Paragraph("Карт-счет", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            Paragraph datetime = new Paragraph("Открытие счета: " + date_start.ToString(), textFont)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            Paragraph datetime2 = new Paragraph("Закрытие счета: " + date_start.AddDays(3).ToString(), textFont)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            Paragraph amount = new Paragraph("Итого: " + textBox9.Text, titleFont)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            db.openConnection();
            MySqlCommand command2 = new MySqlCommand("SELECT AUTO_INCREMENT FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'storage' AND TABLE_NAME = 'invoice';", db.getConnection());
            IDataReader reader = command2.ExecuteReader();
            if (reader != null && reader.Read())
            {
                long ai = reader.GetInt64(0);

                reader.Close();
                db.closeConnection();

                doc.Add(bill);
                doc.Add(datetime);
                doc.Add(datetime2);
                doc.Add(new Paragraph("Номер заказа: "+ai.ToString(), textFont));
                doc.Add(new Paragraph("Заказчик: " + comboBox2.Text, textFont));
                doc.Add(new Paragraph("Счет для оплаты : 50980932200000001704\n\n", textFont));
                doc.Add(table);
                doc.Add(amount);

                //Close PDF document
                doc.Close();

                //Open the Output file
                //Process.Start(new ProcessStartInfo(@"MySQL_to_PDF_table.pdf") { UseShellExecute = true });

                string filename = namefile + ".pdf";
                byte[] fileData;
                string path = @".\PDFs\" + filename;
                using (System.IO.FileStream fs = new System.IO.FileStream(path, FileMode.Open))
                {
                    fileData = new byte[fs.Length];
                    fs.Read(fileData, 0, fileData.Length);
                }
                db.openConnection();
                MySqlCommand command = new MySqlCommand("INSERT INTO invoice (customer, time_start, time_lost, filename, filedata) VALUES (@customer, @time_start, @time_lost, @filename, @filedata)", db.getConnection());
                command.Parameters.Add("@customer", MySqlDbType.VarChar).Value = comboBox2.Text;
                command.Parameters.Add("@time_start", MySqlDbType.VarChar).Value = date.ToString();
                command.Parameters.Add("@time_lost", MySqlDbType.VarChar).Value = date.AddDays(3).ToString();
                command.Parameters.Add("@filename", MySqlDbType.VarChar).Value = filename;
                command.Parameters.Add("@filedata", MySqlDbType.Blob).Value = fileData;
                command.ExecuteNonQuery();
                db.closeConnection();
                MessageBox.Show("Счет создан!");
            }
        }

        protected void GenerateWayBillPDF(object sender, EventArgs e)
        {
            //Variables to create PDF document
            Document doc = new Document();
            DateTime date = DateTime.Now;
            string namefile = (date.ToString()).Replace(".", "").Replace(":", "").Replace(" ", "");
            PdfWriter.GetInstance(doc, new FileStream(@".\PDFs\" + namefile + ".pdf", FileMode.Create)); //Output PDF file
            PdfPTable table = new PdfPTable(5); //To create table inside the PDF

            //Width of each column on table
            float[] widths = { 50f, 150f, 70f, 100f, 110f };
            table.SetWidthPercentage(widths, PageSize.A4);

            //Font settings
            Font titleFont = FontFactory.GetFont("C:\\Windows\\Fonts\\arial.ttf", "Identity-H");
            titleFont.SetStyle("bold");
            Font textFont = FontFactory.GetFont("C:\\Windows\\Fonts\\arial.ttf", "Identity-H");
            titleFont.SetStyle("normal");

            doc.Open(); //Access the PDF Document to write data

            //Add table column headers
            table.AddCell(new Paragraph("id", titleFont));
            table.AddCell(new Paragraph("Название", titleFont));
            table.AddCell(new Paragraph("Цена", titleFont));
            table.AddCell(new Paragraph("Количество", titleFont));
            table.AddCell(new Paragraph("Всего", titleFont));

            //Read the data and store them in the list
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    table.AddCell(new Paragraph(cell.Value.ToString(), textFont));
                }
            }

            //Add data to PDF file
            DateTime date_start = DateTime.Now;
            Paragraph bill = new Paragraph("Накладная", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            Paragraph datetime = new Paragraph(date_start.ToString(), textFont)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            Paragraph amount = new Paragraph("Итого: " + textBox9.Text + "\n\n", titleFont)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            db.openConnection();
            MySqlCommand command2 = new MySqlCommand("SELECT AUTO_INCREMENT FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'storage' AND TABLE_NAME = 'waybill';", db.getConnection());
            IDataReader reader = command2.ExecuteReader();
            if (reader != null && reader.Read())
            {
                long ai = reader.GetInt64(0);
            
            reader.Close();
            db.closeConnection();
            
            doc.Add(bill);
            doc.Add(datetime);
            doc.Add(new Paragraph("Номер накладной: "+ai.ToString(), textFont));
            doc.Add(new Paragraph("Заказчик: " + comboBox2.Text + "\n\n", textFont));
            doc.Add(table);
            doc.Add(amount);
            doc.Add(new Paragraph("Принял ___________________________________               __________", textFont));

            //Close PDF document
            doc.Close();

            //Open the Output file
            //Process.Start(new ProcessStartInfo(@"MySQL_to_PDF_table.pdf") { UseShellExecute = true });

            string filename = namefile + ".pdf";
            byte[] fileData;
            string path = @".\PDFs\" + filename;
            using (System.IO.FileStream fs = new System.IO.FileStream(path, FileMode.Open))
            {
                fileData = new byte[fs.Length];
                fs.Read(fileData, 0, fileData.Length);
            }
            db.openConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO waybill (customer, time, filename, filedata) VALUES (@customer, @time, @filename, @filedata)", db.getConnection());
            command.Parameters.Add("@customer", MySqlDbType.VarChar).Value = comboBox2.Text;
            command.Parameters.Add("@time", MySqlDbType.VarChar).Value = date.ToString();
            command.Parameters.Add("@filename", MySqlDbType.VarChar).Value = filename;
            command.Parameters.Add("@filedata", MySqlDbType.Blob).Value = fileData;
            command.ExecuteNonQuery();
            db.closeConnection();
            MessageBox.Show("Накладная создана!");
            }
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int invoice_id = Convert.ToInt32(dataGridView4[0, dataGridView4.CurrentRow.Index].Value.ToString());
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `customer` FROM `invoice` WHERE id = '" + invoice_id + "'", db.getConnection());
            textBox10.Text = command.ExecuteScalar().ToString();
            MySqlCommand command2 = new MySqlCommand("SELECT `time_start` FROM `invoice` WHERE id = '" + invoice_id + "'", db.getConnection());
            textBox11.Text = command2.ExecuteScalar().ToString();
            MySqlCommand command3 = new MySqlCommand("SELECT `time_lost` FROM `invoice` WHERE id = '" + invoice_id + "'", db.getConnection());
            textBox16.Text = command3.ExecuteScalar().ToString();
            db.closeConnection();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView4.Rows.Count > 0)
            {
                int invoice_id = Convert.ToInt32(dataGridView4[0, dataGridView4.CurrentRow.Index].Value.ToString());
                db.openConnection();
                MySqlCommand command = new MySqlCommand("UPDATE invoice SET paid = 1 WHERE id = " + invoice_id, db.getConnection());
                command.ExecuteNonQuery();
                db.closeConnection();
                updateGridInvoice();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (dataGridView4.RowCount > 0)
            {
                int invoice_id = Convert.ToInt32(dataGridView4[0, dataGridView4.CurrentRow.Index].Value.ToString());
                db.openConnection();
                MySqlCommand command = new MySqlCommand("DELETE FROM invoice WHERE id = " + invoice_id, db.getConnection());
                command.ExecuteNonQuery();
                db.closeConnection();
                textBox10.Text = string.Empty;
                textBox11.Text = string.Empty;
                textBox16.Text = string.Empty;
                updateGridInvoice();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView4.RowCount > 0)
            {
                int invoice_id = Convert.ToInt32(dataGridView4[0, dataGridView4.CurrentRow.Index].Value.ToString());
                db.openConnection();
                MySqlCommand command = new MySqlCommand("SELECT * FROM invoice  WHERE id = " + invoice_id, db.getConnection());
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string filename = reader.GetString(4);
                    byte[] data = (byte[])reader.GetValue(5);
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        FileName = filename.ToString(),
                        Filter = "PDF документ(*.pdf)|*.pdf",
                        DefaultExt = ".pdf",
                        InitialDirectory = @".\PDFs\"
                    };
                    saveFileDialog.ShowDialog();
                    string filepath = saveFileDialog.FileName;
                    File.WriteAllBytes(filepath, data);
                }
                db.closeConnection();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridView5.RowCount > 0)
            {
                int waybill_id = Convert.ToInt32(dataGridView5[0, dataGridView5.CurrentRow.Index].Value.ToString());
                db.openConnection();
                MySqlCommand command = new MySqlCommand("DELETE FROM waybill WHERE id = " + waybill_id, db.getConnection());
                command.ExecuteNonQuery();
                db.closeConnection();
                updateGridWayBill();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView5.RowCount > 0)
            {
                int waybill_id = Convert.ToInt32(dataGridView5[0, dataGridView5.CurrentRow.Index].Value.ToString());
                db.openConnection();
                MySqlCommand command = new MySqlCommand("SELECT * FROM waybill  WHERE id = " + waybill_id, db.getConnection());
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string filename = reader.GetString(3);
                    byte[] data = (byte[])reader.GetValue(4);
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        FileName = filename.ToString(),
                        Filter = "PDF документ(*.pdf)|*.pdf",
                        DefaultExt = ".pdf",
                        InitialDirectory = @".\PDFs\"
                    };
                    saveFileDialog.ShowDialog();
                    string filepath = saveFileDialog.FileName;

                    File.WriteAllBytes(filepath, data);
                }
                db.closeConnection();
            }
        }

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int bill_id = Convert.ToInt32(dataGridView5[0, dataGridView5.CurrentRow.Index].Value.ToString());
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `customer` FROM `waybill` WHERE id = '" + bill_id + "'", db.getConnection());
            textBox21.Text = command.ExecuteScalar().ToString();
            MySqlCommand command2 = new MySqlCommand("SELECT `time` FROM `waybill` WHERE id = '" + bill_id + "'", db.getConnection());
            textBox22.Text = command2.ExecuteScalar().ToString();
            db.closeConnection();
        }
    }
}
