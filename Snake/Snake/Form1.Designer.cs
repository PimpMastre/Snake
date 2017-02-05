namespace Snake
{
    partial class Snake
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelArrowTutorial = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelExit = new System.Windows.Forms.Label();
            this.labelHighScores = new System.Windows.Forms.Label();
            this.labelPlay = new System.Windows.Forms.Label();
            this.labelGameOver = new System.Windows.Forms.Label();
            this.timerGameSpeed = new System.Windows.Forms.Timer(this.components);
            this.timerWriting = new System.Windows.Forms.Timer(this.components);
            this.timerBlinkRate = new System.Windows.Forms.Timer(this.components);
            this.timerGameOver = new System.Windows.Forms.Timer(this.components);
            this.labelGOScore = new System.Windows.Forms.Label();
            this.labelGOScoreNumber = new System.Windows.Forms.Label();
            this.labelGOReplay = new System.Windows.Forms.Label();
            this.labelGOMainMenu = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.labelArrowTutorial);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelExit);
            this.panel1.Controls.Add(this.labelHighScores);
            this.panel1.Controls.Add(this.labelPlay);
            this.panel1.Controls.Add(this.labelGameOver);
            this.panel1.Controls.Add(this.labelGOMainMenu);
            this.panel1.Controls.Add(this.labelGOReplay);
            this.panel1.Controls.Add(this.labelGOScoreNumber);
            this.panel1.Controls.Add(this.labelGOScore);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.MaximumSize = new System.Drawing.Size(653, 656);
            this.panel1.MinimumSize = new System.Drawing.Size(653, 656);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(653, 656);
            this.panel1.TabIndex = 4;
            // 
            // labelArrowTutorial
            // 
            this.labelArrowTutorial.AutoSize = true;
            this.labelArrowTutorial.BackColor = System.Drawing.Color.Black;
            this.labelArrowTutorial.Font = new System.Drawing.Font("8BIT WONDER", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelArrowTutorial.ForeColor = System.Drawing.Color.Transparent;
            this.labelArrowTutorial.Location = new System.Drawing.Point(61, 9);
            this.labelArrowTutorial.Name = "labelArrowTutorial";
            this.labelArrowTutorial.Size = new System.Drawing.Size(514, 24);
            this.labelArrowTutorial.TabIndex = 8;
            this.labelArrowTutorial.Text = "USE ARROW KEYS TO MOVE";
            this.labelArrowTutorial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Enabled = false;
            this.label1.Font = new System.Drawing.Font("8BIT WONDER", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(146, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(348, 64);
            this.label1.TabIndex = 7;
            this.label1.Text = "snake";
            this.label1.Visible = false;
            // 
            // labelExit
            // 
            this.labelExit.AutoSize = true;
            this.labelExit.BackColor = System.Drawing.Color.Black;
            this.labelExit.Font = new System.Drawing.Font("8BIT WONDER", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelExit.ForeColor = System.Drawing.Color.Transparent;
            this.labelExit.Location = new System.Drawing.Point(232, 577);
            this.labelExit.Name = "labelExit";
            this.labelExit.Size = new System.Drawing.Size(188, 48);
            this.labelExit.TabIndex = 6;
            this.labelExit.Text = "exit";
            this.labelExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // labelHighScores
            // 
            this.labelHighScores.AutoSize = true;
            this.labelHighScores.BackColor = System.Drawing.Color.Black;
            this.labelHighScores.Font = new System.Drawing.Font("8BIT WONDER", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHighScores.ForeColor = System.Drawing.Color.Transparent;
            this.labelHighScores.Location = new System.Drawing.Point(223, 313);
            this.labelHighScores.Name = "labelHighScores";
            this.labelHighScores.Size = new System.Drawing.Size(207, 64);
            this.labelHighScores.TabIndex = 5;
            this.labelHighScores.Text = "HIGH\r\nSCORES";
            this.labelHighScores.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelHighScores.Click += new System.EventHandler(this.buttonHighScores_Click);
            // 
            // labelPlay
            // 
            this.labelPlay.AutoSize = true;
            this.labelPlay.BackColor = System.Drawing.Color.Black;
            this.labelPlay.Font = new System.Drawing.Font("8BIT WONDER", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlay.ForeColor = System.Drawing.Color.Transparent;
            this.labelPlay.Location = new System.Drawing.Point(220, 202);
            this.labelPlay.Name = "labelPlay";
            this.labelPlay.Size = new System.Drawing.Size(212, 48);
            this.labelPlay.TabIndex = 4;
            this.labelPlay.Text = "PLAY";
            this.labelPlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // labelGameOver
            // 
            this.labelGameOver.AutoSize = true;
            this.labelGameOver.BackColor = System.Drawing.Color.Black;
            this.labelGameOver.Font = new System.Drawing.Font("8BIT WONDER", 62.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGameOver.ForeColor = System.Drawing.Color.Transparent;
            this.labelGameOver.Location = new System.Drawing.Point(124, 5);
            this.labelGameOver.Name = "labelGameOver";
            this.labelGameOver.Size = new System.Drawing.Size(409, 166);
            this.labelGameOver.TabIndex = 9;
            this.labelGameOver.Text = "GAME\r\nOVER";
            this.labelGameOver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerGameSpeed
            // 
            this.timerGameSpeed.Interval = 250;
            this.timerGameSpeed.Tick += new System.EventHandler(this.timerGameSpeed_Tick);
            // 
            // timerWriting
            // 
            this.timerWriting.Interval = 150;
            this.timerWriting.Tick += new System.EventHandler(this.timerWriting_Tick);
            // 
            // timerBlinkRate
            // 
            this.timerBlinkRate.Interval = 500;
            this.timerBlinkRate.Tick += new System.EventHandler(this.timerBlinkRate_Tick);
            // 
            // timerGameOver
            // 
            this.timerGameOver.Interval = 1000;
            this.timerGameOver.Tick += new System.EventHandler(this.timerGameOver_Tick);
            // 
            // labelGOScore
            // 
            this.labelGOScore.AutoSize = true;
            this.labelGOScore.BackColor = System.Drawing.Color.Black;
            this.labelGOScore.Font = new System.Drawing.Font("8BIT WONDER", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGOScore.ForeColor = System.Drawing.Color.Transparent;
            this.labelGOScore.Location = new System.Drawing.Point(196, 274);
            this.labelGOScore.Name = "labelGOScore";
            this.labelGOScore.Size = new System.Drawing.Size(260, 48);
            this.labelGOScore.TabIndex = 10;
            this.labelGOScore.Text = "SCORE";
            this.labelGOScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGOScoreNumber
            // 
            this.labelGOScoreNumber.AutoSize = true;
            this.labelGOScoreNumber.BackColor = System.Drawing.Color.Black;
            this.labelGOScoreNumber.Font = new System.Drawing.Font("8BIT WONDER", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGOScoreNumber.ForeColor = System.Drawing.Color.Transparent;
            this.labelGOScoreNumber.Location = new System.Drawing.Point(172, 329);
            this.labelGOScoreNumber.Name = "labelGOScoreNumber";
            this.labelGOScoreNumber.Size = new System.Drawing.Size(308, 48);
            this.labelGOScoreNumber.TabIndex = 11;
            this.labelGOScoreNumber.Text = "[SCORE]";
            this.labelGOScoreNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGOReplay
            // 
            this.labelGOReplay.AutoSize = true;
            this.labelGOReplay.BackColor = System.Drawing.Color.Black;
            this.labelGOReplay.Font = new System.Drawing.Font("8BIT WONDER", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGOReplay.ForeColor = System.Drawing.Color.Transparent;
            this.labelGOReplay.Location = new System.Drawing.Point(172, 455);
            this.labelGOReplay.Name = "labelGOReplay";
            this.labelGOReplay.Size = new System.Drawing.Size(308, 48);
            this.labelGOReplay.TabIndex = 12;
            this.labelGOReplay.Text = "REPLAY";
            this.labelGOReplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelGOReplay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // labelGOMainMenu
            // 
            this.labelGOMainMenu.AutoSize = true;
            this.labelGOMainMenu.BackColor = System.Drawing.Color.Black;
            this.labelGOMainMenu.Font = new System.Drawing.Font("8BIT WONDER", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGOMainMenu.ForeColor = System.Drawing.Color.Transparent;
            this.labelGOMainMenu.Location = new System.Drawing.Point(208, 529);
            this.labelGOMainMenu.Name = "labelGOMainMenu";
            this.labelGOMainMenu.Size = new System.Drawing.Size(236, 96);
            this.labelGOMainMenu.TabIndex = 13;
            this.labelGOMainMenu.Text = "MAIN\r\nMENU";
            this.labelGOMainMenu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelGOMainMenu.Click += new System.EventHandler(this.Snake_Load);
            // 
            // Snake
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 640);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(656, 679);
            this.MinimumSize = new System.Drawing.Size(656, 679);
            this.Name = "Snake";
            this.Text = "Snake";
            this.Load += new System.EventHandler(this.Snake_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Snake_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timerGameSpeed;
        private System.Windows.Forms.Label labelPlay;
        private System.Windows.Forms.Label labelHighScores;
        private System.Windows.Forms.Label labelExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelArrowTutorial;
        private System.Windows.Forms.Timer timerBlinkRate;
        private System.Windows.Forms.Timer timerWriting;
        private System.Windows.Forms.Label labelGameOver;
        private System.Windows.Forms.Timer timerGameOver;
        private System.Windows.Forms.Label labelGOScore;
        private System.Windows.Forms.Label labelGOScoreNumber;
        private System.Windows.Forms.Label labelGOMainMenu;
        private System.Windows.Forms.Label labelGOReplay;
    }
}

