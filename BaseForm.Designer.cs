namespace ParserFedresource
{
    partial class BaseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.parse_btn = new System.Windows.Forms.Button();
            this.stop_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.forSearch_tb = new System.Windows.Forms.TextBox();
            this.beginning_cb = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // parse_btn
            // 
            this.parse_btn.Location = new System.Drawing.Point(13, 13);
            this.parse_btn.Name = "parse_btn";
            this.parse_btn.Size = new System.Drawing.Size(306, 29);
            this.parse_btn.TabIndex = 0;
            this.parse_btn.Text = "Начать парсинг";
            this.parse_btn.UseVisualStyleBackColor = true;
            this.parse_btn.Click += new System.EventHandler(this.parse_btn_Click);
            // 
            // stop_btn
            // 
            this.stop_btn.Enabled = false;
            this.stop_btn.Location = new System.Drawing.Point(13, 48);
            this.stop_btn.Name = "stop_btn";
            this.stop_btn.Size = new System.Drawing.Size(306, 29);
            this.stop_btn.TabIndex = 1;
            this.stop_btn.Text = "Остановить парсинг";
            this.stop_btn.UseVisualStyleBackColor = true;
            this.stop_btn.Click += new System.EventHandler(this.Stop_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Строка для поиска:";
            // 
            // forSearch_tb
            // 
            this.forSearch_tb.Location = new System.Drawing.Point(13, 100);
            this.forSearch_tb.Name = "forSearch_tb";
            this.forSearch_tb.Size = new System.Drawing.Size(306, 22);
            this.forSearch_tb.TabIndex = 3;
            // 
            // beginning_cb
            // 
            this.beginning_cb.AutoSize = true;
            this.beginning_cb.Location = new System.Drawing.Point(15, 128);
            this.beginning_cb.Name = "beginning_cb";
            this.beginning_cb.Size = new System.Drawing.Size(319, 21);
            this.beginning_cb.TabIndex = 4;
            this.beginning_cb.Text = "Продолжить поиск с последнего элемента?";
            this.beginning_cb.UseVisualStyleBackColor = true;
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 161);
            this.Controls.Add(this.beginning_cb);
            this.Controls.Add(this.forSearch_tb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.stop_btn);
            this.Controls.Add(this.parse_btn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "BaseForm";
            this.Text = "Parser for fedresurs.ru";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button parse_btn;
        private System.Windows.Forms.Button stop_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox forSearch_tb;
        private System.Windows.Forms.CheckBox beginning_cb;
    }
}

