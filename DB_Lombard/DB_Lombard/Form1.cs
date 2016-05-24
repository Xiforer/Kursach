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

namespace DB_Lombard
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        //КНОПКА ДОБАВИТЬ ТОВАР.
       
        private void button1_Click(object sender, EventArgs e)
        {

            string name = FullName.Text;
            string address = Address.Text;
            string item_name = ItemName.Text;
            string item_price = ItemPrice.Text;
            //string sum = Sum.Text ;
            string start = Start.Text;
            string finish = Finish.Text;
           
            //Сумма
            int anInteger;
            anInteger = Convert.ToInt32(ItemPrice.Text);
            anInteger = int.Parse(ItemPrice.Text);
            int comeback = anInteger+(anInteger/10);
            int sum = anInteger/2;
            
            
            //Срок хранения.
            DateTime d1 = Start.Value;
            DateTime d2 = Finish.Value;
            TimeSpan time = d2 - d1;
           //проверка на заполнение полей.
           if (FullName.TextLength == 0 || FullName.Text.Trim()=="" || Address.TextLength == 0 || Address.Text.Trim() == "" || ItemName.TextLength == 0 || ItemName.Text.Trim() == "" || ItemPrice.TextLength == 0 || ItemPrice.Text.Trim() == "") { MessageBox.Show("Заполните все поля!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information); }


           //Поля в красный цвет.

            if (FullName.TextLength == 0|| FullName.Text.Trim() == "") { MessageBox.Show("Поле \" Ф.И.О\" не заполнено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information); FullName.BackColor = Color.Red; } else { FullName.BackColor = Color.Ivory; }
            if (Address.TextLength == 0|| Address.Text.Trim() == "") { MessageBox.Show("Поле \"Адрес\" не заполнено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information); Address.BackColor = Color.Red; } else { Address.BackColor = Color.Ivory; }
            if (ItemName.TextLength == 0|| ItemName.Text.Trim() == "") { MessageBox.Show("Поле \"Наименование товара\" не заполнено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information); ItemName.BackColor = Color.Red; } else { ItemName.BackColor = Color.Ivory; }
            if (ItemPrice.TextLength == 0|| ItemPrice.Text.Trim() == "") { MessageBox.Show("Поле \"Оценочная стоимость\" не заполнено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information); ItemPrice.BackColor = Color.Red; } else { ItemPrice.BackColor = Color.Ivory; }
            //if (Sum.TextLength == 0|| Sum.Text.Trim() == "") { MessageBox.Show("Поле \"Выданная сумма\" не заполнено" , "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information); Sum.BackColor = Color.Red; } else { Sum.BackColor = Color.Ivory; }
            if (d1 > d2) { MessageBox.Show("Дата сдачи не может быть больше или равной даты выдачи.Пожалуйста, измените дату сдачи", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            //Добавление в БД.

            if (FullName.TextLength != 0 && FullName.Text.Trim() != "" && Address.TextLength != 0 && Address.Text.Trim() != "" && ItemName.TextLength != 0 && ItemName.Text.Trim() != "" && ItemPrice.TextLength != 0 && ItemPrice.Text.Trim() != "" && d1 < d2 ) { MessageBox.Show("Товар успешно добавлен!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information); dataGridView.Rows.Add(name, address, item_name, item_price,sum,comeback, start, finish, time.Days); }
            
           
        }
  //КНОПКА ВОЗВРАТИТЬ ТОВАР.
        private void Trade_Click(object sender, EventArgs e)
        {
            int ind = dataGridView.SelectedCells[0].RowIndex;
            dataGridView.Rows.RemoveAt(ind);
            MessageBox.Show("Товар возвращен владельцу!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    
   //ФАЙЛ-ОТКРЫТЬ.
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream mystr = null;
            if (Open.ShowDialog() == DialogResult.OK)
            {
                if ((mystr = Open.OpenFile()) != null)
                {
                    StreamReader myread = new StreamReader(mystr);
                    string[] str;
                    int num = 0;
                    try
                    {
                        string[] str1 = myread.ReadToEnd().Split('\n');
                        num = str1.Count();
                        dataGridView.RowCount = num;
                        for (int i = 0; i < num; i++)
                        {
                            str = str1[i].Split('*');
                            for (int j = 0; j < dataGridView.ColumnCount; j++)
                            {
                                try
                                {

                                    dataGridView.Rows[i].Cells[j].Value = str[j];
                                }
                                catch
                                {

                                }
                            }
                        }

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                    finally { myread.Close(); }
                }
            }
        }
//ФАЙЛ - СОХРАНИТЬ.
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            if (Save.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = Save.OpenFile()) != null)
                {
                    StreamWriter myWritet = new StreamWriter(myStream);
                    try
                    {
                        for (int i = 0; i < dataGridView.RowCount - 1; i++)
                        {
                            myWritet.Write(Environment.NewLine);
                            for (int j = 0; j < dataGridView.ColumnCount; j++)
                            {
                                myWritet.Write(dataGridView.Rows[i].Cells[j].Value.ToString() + "*");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally { myWritet.Close(); }
                    myStream.Close();
                }
            }
        }
  //КНОПКА ВЫСТАВИТЬ НА ПРОДАЖУ.
        private void Auction_Click(object sender, EventArgs e)
        {
            int ind = dataGridView.SelectedCells[0].RowIndex;
            if (MessageBox.Show("Выставить товар на продажу?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dataGridView.Rows.RemoveAt(ind);
                MessageBox.Show(String.Format("Товар успешно выставлен на продажу!"));
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form_Load(object sender, EventArgs e)
        {
        }
//СПРАВКА.
        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная программа была создана студентом кафедры Программная Инженерия,1-го курса,группы ПИ-15-5 - Янченко Александром Алексеевичем.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.F1)
            { MessageBox.Show("Данная программа была создана студентом кафедры Программная Инженерия,1-го курса,группы ПИ-15-5 - Янченко Александром Алексеевичем.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); }
             return base.ProcessDialogKey(keyData);
        }            
        private void FullName_TextChanged(object sender, EventArgs e)
        {
        }
        //Запрещаем ввод букв в поля "Оценочная стоимость" и "Выданная сумма".
        private void ItemPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
             if(!char.IsDigit(ch) && ch !=8)
            { e.Handled = true; }
          
        }

        private void Sum_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            { e.Handled = true; }
        }
    }
}
