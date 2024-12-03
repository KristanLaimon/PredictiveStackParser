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
            richTextBoxInput = new RichTextBox();
            label1 = new Label();
            btnEjecutar = new Button();
            label3 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label2 = new Label();
            labelMessage = new Label();
            SuspendLayout();
            // 
            // richTextBoxInput
            // 
            richTextBoxInput.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBoxInput.ForeColor = SystemColors.WindowFrame;
            richTextBoxInput.Location = new Point(17, 111);
            richTextBoxInput.Name = "richTextBoxInput";
            richTextBoxInput.Size = new Size(659, 163);
            richTextBoxInput.TabIndex = 0;
            richTextBoxInput.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.ControlDarkDark;
            label1.Location = new Point(17, 91);
            label1.Name = "label1";
            label1.Size = new Size(66, 20);
            label1.TabIndex = 1;
            label1.Text = "Entrada";
            // 
            // btnEjecutar
            // 
            btnEjecutar.BackColor = SystemColors.ActiveCaptionText;
            btnEjecutar.ForeColor = SystemColors.ButtonHighlight;
            btnEjecutar.Location = new Point(17, 321);
            btnEjecutar.Name = "btnEjecutar";
            btnEjecutar.Size = new Size(661, 37);
            btnEjecutar.TabIndex = 8;
            btnEjecutar.Text = "Ejecutar";
            btnEjecutar.UseVisualStyleBackColor = false;
            btnEjecutar.Click += btnEjecutar_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(268, 9);
            label3.Name = "label3";
            label3.Size = new Size(151, 32);
            label3.TabIndex = 9;
            label3.Text = "Parser PR08";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Century Gothic", 8.25F);
            label5.ForeColor = SystemColors.WindowFrame;
            label5.Location = new Point(272, 45);
            label5.Name = "label5";
            label5.Size = new Size(128, 19);
            label5.TabIndex = 10;
            label5.Text = "Kristán Ruíz Limón";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Century Gothic", 8.25F);
            label6.ForeColor = SystemColors.ControlDarkDark;
            label6.Location = new Point(248, 62);
            label6.Name = "label6";
            label6.Size = new Size(190, 19);
            label6.TabIndex = 11;
            label6.Text = "Tomás Aiden Mejía Ortega";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Century Gothic", 8.25F);
            label7.ForeColor = SystemColors.ControlDarkDark;
            label7.Location = new Point(261, 81);
            label7.Name = "label7";
            label7.Size = new Size(162, 19);
            label7.TabIndex = 12;
            label7.Text = "Luis Angel Covarrubias";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ControlDarkDark;
            label2.Location = new Point(17, 277);
            label2.Name = "label2";
            label2.Size = new Size(86, 20);
            label2.TabIndex = 13;
            label2.Text = "Mensaje |";
            // 
            // labelMessage
            // 
            labelMessage.AutoSize = true;
            labelMessage.ForeColor = SystemColors.ControlDarkDark;
            labelMessage.Location = new Point(99, 277);
            labelMessage.Name = "labelMessage";
            labelMessage.Size = new Size(103, 20);
            labelMessage.TabIndex = 14;
            labelMessage.Text = "DEBUGGING";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.HighlightText;
            ClientSize = new Size(696, 442);
            Controls.Add(labelMessage);
            Controls.Add(label2);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(btnEjecutar);
            Controls.Add(label1);
            Controls.Add(richTextBoxInput);
            Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox richTextBoxInput;
        private Label label1;
        private Button btnEjecutar;
        private Label label3;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label2;
        private Label labelMessage;
    }
}
