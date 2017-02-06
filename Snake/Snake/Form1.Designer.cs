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
            this.labelEdgeScrollingMP = new System.Windows.Forms.Label();
            this.labelEdgeScrolling = new System.Windows.Forms.Label();
            this.labelHardMP = new System.Windows.Forms.Label();
            this.labelExtremeMP = new System.Windows.Forms.Label();
            this.labelMediumMP = new System.Windows.Forms.Label();
            this.labelEasyMP = new System.Windows.Forms.Label();
            this.labelReqXP = new System.Windows.Forms.Label();
            this.labelGOXPNumberMultiplier = new System.Windows.Forms.Label();
            this.labelDifficultyExtreme = new System.Windows.Forms.Label();
            this.labelDifficultyHard = new System.Windows.Forms.Label();
            this.labelDifficultyMedium = new System.Windows.Forms.Label();
            this.labelDifficultyEasy = new System.Windows.Forms.Label();
            this.labelLevel = new System.Windows.Forms.Label();
            this.labelGOXPNumber = new System.Windows.Forms.Label();
            this.labelArrowTutorial = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelExit = new System.Windows.Forms.Label();
            this.labelHighScores = new System.Windows.Forms.Label();
            this.labelPlay = new System.Windows.Forms.Label();
            this.labelGameOver = new System.Windows.Forms.Label();
            this.labelGOMainMenu = new System.Windows.Forms.Label();
            this.labelGOReplay = new System.Windows.Forms.Label();
            this.labelGOScoreNumber = new System.Windows.Forms.Label();
            this.labelGOScore = new System.Windows.Forms.Label();
            this.labelGOXP = new System.Windows.Forms.Label();
            this.timerGameSpeed = new System.Windows.Forms.Timer(this.components);
            this.timerWriting = new System.Windows.Forms.Timer(this.components);
            this.timerBlinkRate = new System.Windows.Forms.Timer(this.components);
            this.timerGameOver = new System.Windows.Forms.Timer(this.components);
            this.timerBlinkXP = new System.Windows.Forms.Timer(this.components);
            this.timerBlinkLevel = new System.Windows.Forms.Timer(this.components);
            this.labelBack = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.labelBack);
            this.panel1.Controls.Add(this.labelEdgeScrollingMP);
            this.panel1.Controls.Add(this.labelEdgeScrolling);
            this.panel1.Controls.Add(this.labelHardMP);
            this.panel1.Controls.Add(this.labelExtremeMP);
            this.panel1.Controls.Add(this.labelMediumMP);
            this.panel1.Controls.Add(this.labelEasyMP);
            this.panel1.Controls.Add(this.labelReqXP);
            this.panel1.Controls.Add(this.labelGOXPNumberMultiplier);
            this.panel1.Controls.Add(this.labelDifficultyExtreme);
            this.panel1.Controls.Add(this.labelDifficultyHard);
            this.panel1.Controls.Add(this.labelDifficultyMedium);
            this.panel1.Controls.Add(this.labelDifficultyEasy);
            this.panel1.Controls.Add(this.labelLevel);
            this.panel1.Controls.Add(this.labelGOXPNumber);
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
            this.panel1.Controls.Add(this.labelGOXP);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.MaximumSize = new System.Drawing.Size(653, 656);
            this.panel1.MinimumSize = new System.Drawing.Size(653, 656);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(653, 656);
            this.panel1.TabIndex = 4;
            // 
            // labelEdgeScrollingMP
            // 
            this.labelEdgeScrollingMP.AutoSize = true;
            this.labelEdgeScrollingMP.BackColor = System.Drawing.Color.Black;
            this.labelEdgeScrollingMP.Font = new System.Drawing.Font("8BIT WONDER", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEdgeScrollingMP.ForeColor = System.Drawing.Color.Transparent;
            this.labelEdgeScrollingMP.Location = new System.Drawing.Point(143, 505);
            this.labelEdgeScrollingMP.Name = "labelEdgeScrollingMP";
            this.labelEdgeScrollingMP.Size = new System.Drawing.Size(367, 16);
            this.labelEdgeScrollingMP.TabIndex = 28;
            this.labelEdgeScrollingMP.Text = "(MULTIPLIER + 1 IF DISABLED)";
            this.labelEdgeScrollingMP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEdgeScrolling
            // 
            this.labelEdgeScrolling.AutoSize = true;
            this.labelEdgeScrolling.BackColor = System.Drawing.Color.Black;
            this.labelEdgeScrolling.Font = new System.Drawing.Font("8BIT WONDER", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEdgeScrolling.ForeColor = System.Drawing.Color.Transparent;
            this.labelEdgeScrolling.Location = new System.Drawing.Point(16, 470);
            this.labelEdgeScrolling.Name = "labelEdgeScrolling";
            this.labelEdgeScrolling.Size = new System.Drawing.Size(620, 29);
            this.labelEdgeScrolling.TabIndex = 27;
            this.labelEdgeScrolling.Text = "EDGE SCROLLING DISABLED";
            this.labelEdgeScrolling.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelEdgeScrolling.Click += new System.EventHandler(this.label6_Click);
            // 
            // labelHardMP
            // 
            this.labelHardMP.AutoSize = true;
            this.labelHardMP.BackColor = System.Drawing.Color.Black;
            this.labelHardMP.Font = new System.Drawing.Font("8BIT WONDER", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHardMP.ForeColor = System.Drawing.Color.Transparent;
            this.labelHardMP.Location = new System.Drawing.Point(476, 254);
            this.labelHardMP.Name = "labelHardMP";
            this.labelHardMP.Size = new System.Drawing.Size(40, 16);
            this.labelHardMP.TabIndex = 26;
            this.labelHardMP.Text = "X3";
            this.labelHardMP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelExtremeMP
            // 
            this.labelExtremeMP.AutoSize = true;
            this.labelExtremeMP.BackColor = System.Drawing.Color.Black;
            this.labelExtremeMP.Font = new System.Drawing.Font("8BIT WONDER", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelExtremeMP.ForeColor = System.Drawing.Color.Transparent;
            this.labelExtremeMP.Location = new System.Drawing.Point(476, 340);
            this.labelExtremeMP.Name = "labelExtremeMP";
            this.labelExtremeMP.Size = new System.Drawing.Size(40, 16);
            this.labelExtremeMP.TabIndex = 25;
            this.labelExtremeMP.Text = "X5";
            this.labelExtremeMP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMediumMP
            // 
            this.labelMediumMP.AutoSize = true;
            this.labelMediumMP.BackColor = System.Drawing.Color.Black;
            this.labelMediumMP.Font = new System.Drawing.Font("8BIT WONDER", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMediumMP.ForeColor = System.Drawing.Color.Transparent;
            this.labelMediumMP.Location = new System.Drawing.Point(476, 168);
            this.labelMediumMP.Name = "labelMediumMP";
            this.labelMediumMP.Size = new System.Drawing.Size(40, 16);
            this.labelMediumMP.TabIndex = 24;
            this.labelMediumMP.Text = "X2";
            this.labelMediumMP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEasyMP
            // 
            this.labelEasyMP.AutoSize = true;
            this.labelEasyMP.BackColor = System.Drawing.Color.Black;
            this.labelEasyMP.Font = new System.Drawing.Font("8BIT WONDER", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEasyMP.ForeColor = System.Drawing.Color.Transparent;
            this.labelEasyMP.Location = new System.Drawing.Point(476, 82);
            this.labelEasyMP.Name = "labelEasyMP";
            this.labelEasyMP.Size = new System.Drawing.Size(32, 16);
            this.labelEasyMP.TabIndex = 23;
            this.labelEasyMP.Text = "X1";
            this.labelEasyMP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReqXP
            // 
            this.labelReqXP.AutoSize = true;
            this.labelReqXP.BackColor = System.Drawing.Color.Black;
            this.labelReqXP.Font = new System.Drawing.Font("8BIT WONDER", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReqXP.ForeColor = System.Drawing.Color.Transparent;
            this.labelReqXP.Location = new System.Drawing.Point(281, 413);
            this.labelReqXP.Name = "labelReqXP";
            this.labelReqXP.Size = new System.Drawing.Size(90, 15);
            this.labelReqXP.TabIndex = 22;
            this.labelReqXP.Text = "REQ XP";
            this.labelReqXP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGOXPNumberMultiplier
            // 
            this.labelGOXPNumberMultiplier.AutoSize = true;
            this.labelGOXPNumberMultiplier.BackColor = System.Drawing.Color.Black;
            this.labelGOXPNumberMultiplier.Font = new System.Drawing.Font("8BIT WONDER", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGOXPNumberMultiplier.ForeColor = System.Drawing.Color.Transparent;
            this.labelGOXPNumberMultiplier.Location = new System.Drawing.Point(388, 319);
            this.labelGOXPNumberMultiplier.Name = "labelGOXPNumberMultiplier";
            this.labelGOXPNumberMultiplier.Size = new System.Drawing.Size(40, 16);
            this.labelGOXPNumberMultiplier.TabIndex = 21;
            this.labelGOXPNumberMultiplier.Text = "X6";
            this.labelGOXPNumberMultiplier.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDifficultyExtreme
            // 
            this.labelDifficultyExtreme.AutoSize = true;
            this.labelDifficultyExtreme.BackColor = System.Drawing.Color.Black;
            this.labelDifficultyExtreme.Font = new System.Drawing.Font("8BIT WONDER", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDifficultyExtreme.ForeColor = System.Drawing.Color.Transparent;
            this.labelDifficultyExtreme.Location = new System.Drawing.Point(179, 327);
            this.labelDifficultyExtreme.Name = "labelDifficultyExtreme";
            this.labelDifficultyExtreme.Size = new System.Drawing.Size(295, 37);
            this.labelDifficultyExtreme.TabIndex = 20;
            this.labelDifficultyExtreme.Text = "EXTREME";
            this.labelDifficultyExtreme.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelDifficultyExtreme.Click += new System.EventHandler(this.labelDifficultyExtreme_Click);
            // 
            // labelDifficultyHard
            // 
            this.labelDifficultyHard.AutoSize = true;
            this.labelDifficultyHard.BackColor = System.Drawing.Color.Black;
            this.labelDifficultyHard.Font = new System.Drawing.Font("8BIT WONDER", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDifficultyHard.ForeColor = System.Drawing.Color.Transparent;
            this.labelDifficultyHard.Location = new System.Drawing.Point(244, 242);
            this.labelDifficultyHard.Name = "labelDifficultyHard";
            this.labelDifficultyHard.Size = new System.Drawing.Size(165, 37);
            this.labelDifficultyHard.TabIndex = 19;
            this.labelDifficultyHard.Text = "HARD";
            this.labelDifficultyHard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelDifficultyHard.Click += new System.EventHandler(this.labelDifficultyHard_Click);
            // 
            // labelDifficultyMedium
            // 
            this.labelDifficultyMedium.AutoSize = true;
            this.labelDifficultyMedium.BackColor = System.Drawing.Color.Black;
            this.labelDifficultyMedium.Font = new System.Drawing.Font("8BIT WONDER", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDifficultyMedium.ForeColor = System.Drawing.Color.Transparent;
            this.labelDifficultyMedium.Location = new System.Drawing.Point(197, 157);
            this.labelDifficultyMedium.Name = "labelDifficultyMedium";
            this.labelDifficultyMedium.Size = new System.Drawing.Size(259, 37);
            this.labelDifficultyMedium.TabIndex = 18;
            this.labelDifficultyMedium.Text = "MEDIUM";
            this.labelDifficultyMedium.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelDifficultyMedium.Click += new System.EventHandler(this.labelDifficultyMedium_Click);
            // 
            // labelDifficultyEasy
            // 
            this.labelDifficultyEasy.AutoSize = true;
            this.labelDifficultyEasy.BackColor = System.Drawing.Color.Black;
            this.labelDifficultyEasy.Font = new System.Drawing.Font("8BIT WONDER", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDifficultyEasy.ForeColor = System.Drawing.Color.Transparent;
            this.labelDifficultyEasy.Location = new System.Drawing.Point(244, 72);
            this.labelDifficultyEasy.Name = "labelDifficultyEasy";
            this.labelDifficultyEasy.Size = new System.Drawing.Size(165, 37);
            this.labelDifficultyEasy.TabIndex = 17;
            this.labelDifficultyEasy.Text = "EASY";
            this.labelDifficultyEasy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelDifficultyEasy.Click += new System.EventHandler(this.labelDifficultyEasy_Click);
            // 
            // labelLevel
            // 
            this.labelLevel.AutoSize = true;
            this.labelLevel.BackColor = System.Drawing.Color.Black;
            this.labelLevel.Font = new System.Drawing.Font("8BIT WONDER", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLevel.ForeColor = System.Drawing.Color.Transparent;
            this.labelLevel.Location = new System.Drawing.Point(233, 380);
            this.labelLevel.Name = "labelLevel";
            this.labelLevel.Size = new System.Drawing.Size(186, 29);
            this.labelLevel.TabIndex = 16;
            this.labelLevel.Text = "LEVEL 1";
            this.labelLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGOXPNumber
            // 
            this.labelGOXPNumber.AutoSize = true;
            this.labelGOXPNumber.BackColor = System.Drawing.Color.Black;
            this.labelGOXPNumber.Font = new System.Drawing.Font("8BIT WONDER", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGOXPNumber.ForeColor = System.Drawing.Color.Transparent;
            this.labelGOXPNumber.Location = new System.Drawing.Point(262, 306);
            this.labelGOXPNumber.Name = "labelGOXPNumber";
            this.labelGOXPNumber.Size = new System.Drawing.Size(129, 37);
            this.labelGOXPNumber.TabIndex = 15;
            this.labelGOXPNumber.Text = "[XP]";
            this.labelGOXPNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.labelGameOver.Font = new System.Drawing.Font("8BIT WONDER", 51.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGameOver.ForeColor = System.Drawing.Color.Transparent;
            this.labelGameOver.Location = new System.Drawing.Point(158, 5);
            this.labelGameOver.Name = "labelGameOver";
            this.labelGameOver.Size = new System.Drawing.Size(340, 138);
            this.labelGameOver.TabIndex = 9;
            this.labelGameOver.Text = "GAME\r\nOVER";
            this.labelGameOver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGOMainMenu
            // 
            this.labelGOMainMenu.AutoSize = true;
            this.labelGOMainMenu.BackColor = System.Drawing.Color.Black;
            this.labelGOMainMenu.Font = new System.Drawing.Font("8BIT WONDER", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGOMainMenu.ForeColor = System.Drawing.Color.Transparent;
            this.labelGOMainMenu.Location = new System.Drawing.Point(234, 558);
            this.labelGOMainMenu.Name = "labelGOMainMenu";
            this.labelGOMainMenu.Size = new System.Drawing.Size(184, 74);
            this.labelGOMainMenu.TabIndex = 13;
            this.labelGOMainMenu.Text = "MAIN\r\nMENU";
            this.labelGOMainMenu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelGOMainMenu.Click += new System.EventHandler(this.Snake_Load);
            // 
            // labelGOReplay
            // 
            this.labelGOReplay.AutoSize = true;
            this.labelGOReplay.BackColor = System.Drawing.Color.Black;
            this.labelGOReplay.Font = new System.Drawing.Font("8BIT WONDER", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGOReplay.ForeColor = System.Drawing.Color.Transparent;
            this.labelGOReplay.Location = new System.Drawing.Point(207, 506);
            this.labelGOReplay.Name = "labelGOReplay";
            this.labelGOReplay.Size = new System.Drawing.Size(239, 37);
            this.labelGOReplay.TabIndex = 12;
            this.labelGOReplay.Text = "REPLAY";
            this.labelGOReplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelGOReplay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // labelGOScoreNumber
            // 
            this.labelGOScoreNumber.AutoSize = true;
            this.labelGOScoreNumber.BackColor = System.Drawing.Color.Black;
            this.labelGOScoreNumber.Font = new System.Drawing.Font("8BIT WONDER", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGOScoreNumber.ForeColor = System.Drawing.Color.Transparent;
            this.labelGOScoreNumber.Location = new System.Drawing.Point(206, 207);
            this.labelGOScoreNumber.Name = "labelGOScoreNumber";
            this.labelGOScoreNumber.Size = new System.Drawing.Size(240, 37);
            this.labelGOScoreNumber.TabIndex = 11;
            this.labelGOScoreNumber.Text = "[SCORE]";
            this.labelGOScoreNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGOScore
            // 
            this.labelGOScore.AutoSize = true;
            this.labelGOScore.BackColor = System.Drawing.Color.Black;
            this.labelGOScore.Font = new System.Drawing.Font("8BIT WONDER", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGOScore.ForeColor = System.Drawing.Color.Transparent;
            this.labelGOScore.Location = new System.Drawing.Point(225, 164);
            this.labelGOScore.Name = "labelGOScore";
            this.labelGOScore.Size = new System.Drawing.Size(202, 37);
            this.labelGOScore.TabIndex = 10;
            this.labelGOScore.Text = "SCORE";
            this.labelGOScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGOXP
            // 
            this.labelGOXP.AutoSize = true;
            this.labelGOXP.BackColor = System.Drawing.Color.Black;
            this.labelGOXP.Font = new System.Drawing.Font("8BIT WONDER", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGOXP.ForeColor = System.Drawing.Color.Transparent;
            this.labelGOXP.Location = new System.Drawing.Point(281, 263);
            this.labelGOXP.Name = "labelGOXP";
            this.labelGOXP.Size = new System.Drawing.Size(91, 37);
            this.labelGOXP.TabIndex = 14;
            this.labelGOXP.Text = "XP";
            this.labelGOXP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerGameSpeed
            // 
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
            // timerBlinkXP
            // 
            this.timerBlinkXP.Interval = 500;
            this.timerBlinkXP.Tick += new System.EventHandler(this.timerBlinkXP_Tick);
            // 
            // timerBlinkLevel
            // 
            this.timerBlinkLevel.Tick += new System.EventHandler(this.timerBlinkLevel_Tick);
            // 
            // labelBack
            // 
            this.labelBack.AutoSize = true;
            this.labelBack.BackColor = System.Drawing.Color.Black;
            this.labelBack.Font = new System.Drawing.Font("8BIT WONDER", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBack.ForeColor = System.Drawing.Color.Transparent;
            this.labelBack.Location = new System.Drawing.Point(244, 592);
            this.labelBack.Name = "labelBack";
            this.labelBack.Size = new System.Drawing.Size(165, 37);
            this.labelBack.TabIndex = 29;
            this.labelBack.Text = "BACK";
            this.labelBack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelBack.Click += new System.EventHandler(this.Snake_Load);
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
        private System.Windows.Forms.Label labelGOXPNumber;
        private System.Windows.Forms.Label labelGOXP;
        private System.Windows.Forms.Timer timerBlinkXP;
        private System.Windows.Forms.Label labelLevel;
        private System.Windows.Forms.Label labelDifficultyExtreme;
        private System.Windows.Forms.Label labelDifficultyHard;
        private System.Windows.Forms.Label labelDifficultyMedium;
        private System.Windows.Forms.Label labelDifficultyEasy;
        private System.Windows.Forms.Label labelGOXPNumberMultiplier;
        private System.Windows.Forms.Label labelReqXP;
        private System.Windows.Forms.Label labelHardMP;
        private System.Windows.Forms.Label labelExtremeMP;
        private System.Windows.Forms.Label labelMediumMP;
        private System.Windows.Forms.Label labelEasyMP;
        private System.Windows.Forms.Label labelEdgeScrolling;
        private System.Windows.Forms.Label labelEdgeScrollingMP;
        private System.Windows.Forms.Timer timerBlinkLevel;
        private System.Windows.Forms.Label labelBack;
    }
}

