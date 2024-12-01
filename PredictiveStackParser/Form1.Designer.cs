namespace PredictiveStackParser
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            richTextBox1 = new RichTextBox();
            label1 = new Label();
            dataGridView1 = new DataGridView();
            lineaError = new DataGridViewTextBoxColumn();
            codigoError = new DataGridViewTextBoxColumn();
            descripcion = new DataGridViewTextBoxColumn();
            label2 = new Label();
            dataGridView2 = new DataGridView();
            parApertura = new DataGridViewTextBoxColumn();
            op = new DataGridViewTextBoxColumn();
            parCerradura = new DataGridViewTextBoxColumn();
            or = new DataGridViewTextBoxColumn();
            puntoComa = new DataGridViewTextBoxColumn();
            indicadorPila = new DataGridViewTextBoxColumn();
            label4 = new Label();
            button1 = new Button();
            label3 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox1.ForeColor = SystemColors.WindowFrame;
            richTextBox1.Location = new Point(17, 111);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(659, 124);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.ControlDarkDark;
            label1.Location = new Point(17, 91);
            label1.Name = "label1";
            label1.Size = new Size(54, 17);
            label1.TabIndex = 1;
            label1.Text = "Entrada";
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = SystemColors.ScrollBar;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { lineaError, codigoError, descripcion });
            dataGridView1.Location = new Point(17, 320);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(364, 222);
            dataGridView1.TabIndex = 2;
            // 
            // lineaError
            // 
            lineaError.HeaderText = "LÍNEA";
            lineaError.Name = "lineaError";
            // 
            // codigoError
            // 
            codigoError.HeaderText = "CÓDIGO";
            codigoError.Name = "codigoError";
            // 
            // descripcion
            // 
            descripcion.HeaderText = "DESCRIPCIÓN";
            descripcion.Name = "descripcion";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ControlDarkDark;
            label2.Location = new Point(388, 301);
            label2.Name = "label2";
            label2.Size = new Size(105, 17);
            label2.TabIndex = 3;
            label2.Text = "Tabla Sintáctica";
            label2.Click += label2_Click;
            // 
            // dataGridView2
            // 
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.BackgroundColor = SystemColors.ScrollBar;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { parApertura, op, parCerradura, or, puntoComa, indicadorPila });
            dataGridView2.Location = new Point(388, 320);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(290, 222);
            dataGridView2.TabIndex = 4;
            // 
            // parApertura
            // 
            parApertura.HeaderText = "(";
            parApertura.Name = "parApertura";
            // 
            // op
            // 
            op.HeaderText = "OP";
            op.Name = "op";
            // 
            // parCerradura
            // 
            parCerradura.HeaderText = ")";
            parCerradura.Name = "parCerradura";
            // 
            // or
            // 
            or.HeaderText = "OR";
            or.Name = "or";
            // 
            // puntoComa
            // 
            puntoComa.HeaderText = ";";
            puntoComa.Name = "puntoComa";
            // 
            // indicadorPila
            // 
            indicadorPila.HeaderText = "$";
            indicadorPila.Name = "indicadorPila";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = SystemColors.ControlDarkDark;
            label4.Location = new Point(17, 301);
            label4.Name = "label4";
            label4.Size = new Size(114, 17);
            label4.TabIndex = 7;
            label4.Text = "Módulo de Errores";
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ActiveCaptionText;
            button1.ForeColor = SystemColors.ButtonHighlight;
            button1.Location = new Point(17, 243);
            button1.Name = "button1";
            button1.Size = new Size(661, 37);
            button1.TabIndex = 8;
            button1.Text = "Ejecutar";
            button1.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(297, 12);
            label3.Name = "label3";
            label3.Size = new Size(119, 25);
            label3.TabIndex = 9;
            label3.Text = "Parser PR08";
            label3.Click += label3_Click_1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Century Gothic", 8.25F);
            label5.ForeColor = SystemColors.WindowFrame;
            label5.Location = new Point(303, 36);
            label5.Name = "label5";
            label5.Size = new Size(100, 16);
            label5.TabIndex = 10;
            label5.Text = "Kristán Ruíz Limón";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Century Gothic", 8.25F);
            label6.ForeColor = SystemColors.ControlDarkDark;
            label6.Location = new Point(279, 53);
            label6.Name = "label6";
            label6.Size = new Size(150, 16);
            label6.TabIndex = 11;
            label6.Text = "Tomás Aiden Mejía Ortega";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Century Gothic", 8.25F);
            label7.ForeColor = SystemColors.ControlDarkDark;
            label7.Location = new Point(292, 72);
            label7.Name = "label7";
            label7.Size = new Size(128, 16);
            label7.TabIndex = 12;
            label7.Text = "Luis Angel Covarrubias";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.HighlightText;
            ClientSize = new Size(696, 554);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(dataGridView2);
            Controls.Add(label2);
            Controls.Add(dataGridView1);
            Controls.Add(label1);
            Controls.Add(richTextBox1);
            Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox richTextBox1;
        private Label label1;
        private DataGridView dataGridView1;
        private Label label2;
        private DataGridView dataGridView2;
        private Label label4;
        private Button button1;
        private DataGridViewTextBoxColumn parApertura;
        private DataGridViewTextBoxColumn op;
        private DataGridViewTextBoxColumn parCerradura;
        private DataGridViewTextBoxColumn or;
        private DataGridViewTextBoxColumn puntoComa;
        private DataGridViewTextBoxColumn indicadorPila;
        private DataGridViewTextBoxColumn lineaError;
        private DataGridViewTextBoxColumn codigoError;
        private DataGridViewTextBoxColumn descripcion;
        private Label label3;
        private Label label5;
        private Label label6;
        private Label label7;
    }
}
