namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ToJSon = new System.Windows.Forms.Button();
            this.ToObject = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ToJSon
            // 
            this.ToJSon.ForeColor = System.Drawing.Color.Red;
            this.ToJSon.Location = new System.Drawing.Point(60, 175);
            this.ToJSon.Name = "ToJSon";
            this.ToJSon.Size = new System.Drawing.Size(138, 25);
            this.ToJSon.TabIndex = 0;
            this.ToJSon.Tag = "";
            this.ToJSon.Text = "Usuń zapytania z bazy";
            this.ToJSon.UseVisualStyleBackColor = true;
            this.ToJSon.Click += new System.EventHandler(this.ToJSon_Click);
            // 
            // ToObject
            // 
            this.ToObject.Location = new System.Drawing.Point(60, 40);
            this.ToObject.Name = "ToObject";
            this.ToObject.Size = new System.Drawing.Size(138, 34);
            this.ToObject.TabIndex = 1;
            this.ToObject.Text = "Pobierz zapytania z bazy";
            this.ToObject.UseVisualStyleBackColor = true;
            this.ToObject.Click += new System.EventHandler(this.ToObject_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button4.ForeColor = System.Drawing.Color.Green;
            this.button4.Location = new System.Drawing.Point(60, 85);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(138, 52);
            this.button4.TabIndex = 6;
            this.button4.Text = "Stwórz wizytówki";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 235);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.ToObject);
            this.Controls.Add(this.ToJSon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Formularze";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ToJSon;
        private System.Windows.Forms.Button ToObject;
        private System.Windows.Forms.Button button4;
    }
}

