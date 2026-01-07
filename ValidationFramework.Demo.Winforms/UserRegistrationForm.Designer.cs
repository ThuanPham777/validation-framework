namespace ValidationFramework.Demo.Winforms
{
    partial class UserRegistrationForm
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
            components = new System.ComponentModel.Container();
            
            // Labels for fields
            lblUsername = new Label();
            lblEmail = new Label();
            lblPhone = new Label();
            lblPassword = new Label();
            lblConfirmPassword = new Label();
            
            // TextBoxes
            txtUsername = new TextBox();
            txtEmail = new TextBox();
            txtPhone = new TextBox();
            txtPassword = new TextBox();
            txtConfirmPassword = new TextBox();
            
            // Error labels
            lblUsernameError = new Label();
            lblEmailError = new Label();
            lblPhoneError = new Label();
            lblPasswordError = new Label();
            lblConfirmPasswordError = new Label();
            
            // Buttons
            btnValidate = new Button();
            btnClear = new Button();
            
            // Summary label
            lblSummary = new Label();
            
            // GroupBox
            grpUserInfo = new GroupBox();
            grpNotifierOptions = new GroupBox();
        
            // CheckBoxes for notifier options
            chkMessageBox = new CheckBox();
            chkHighlight = new CheckBox();
            chkErrorProvider = new CheckBox();
            chkToolTip = new CheckBox();
            chkSummaryLabel = new CheckBox();
            
            // ErrorProvider and ToolTip
            errorProvider = new ErrorProvider(components);
            toolTip = new ToolTip(components);
        
            // Title
            lblTitle = new Label();
            
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            grpUserInfo.SuspendLayout();
            grpNotifierOptions.SuspendLayout();
            SuspendLayout();
            
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.DarkBlue;
            lblTitle.Location = new Point(200, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(350, 30);
            lblTitle.Text = "ValidationFramework WinForms Demo";
            
            // 
            // grpUserInfo
            // 
            grpUserInfo.Controls.Add(lblUsername);
            grpUserInfo.Controls.Add(txtUsername);
            grpUserInfo.Controls.Add(lblUsernameError);
            grpUserInfo.Controls.Add(lblEmail);
            grpUserInfo.Controls.Add(txtEmail);
            grpUserInfo.Controls.Add(lblEmailError);
            grpUserInfo.Controls.Add(lblPhone);
            grpUserInfo.Controls.Add(txtPhone);
            grpUserInfo.Controls.Add(lblPhoneError);
            grpUserInfo.Controls.Add(lblPassword);
            grpUserInfo.Controls.Add(txtPassword);
            grpUserInfo.Controls.Add(lblPasswordError);
            grpUserInfo.Controls.Add(lblConfirmPassword);
            grpUserInfo.Controls.Add(txtConfirmPassword);
            grpUserInfo.Controls.Add(lblConfirmPasswordError);
            grpUserInfo.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            grpUserInfo.Location = new Point(20, 55);
            grpUserInfo.Name = "grpUserInfo";
            grpUserInfo.Size = new Size(500, 320);
            grpUserInfo.TabIndex = 0;
            grpUserInfo.Text = "User Registration";
            
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(20, 35);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(75, 19);
            lblUsername.Text = "Username:";
        
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(150, 32);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(300, 25);
            txtUsername.TabIndex = 1;
        
            // 
            // lblUsernameError
            // 
            lblUsernameError.AutoSize = true;
            lblUsernameError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblUsernameError.ForeColor = Color.Red;
            lblUsernameError.Location = new Point(150, 58);
            lblUsernameError.Name = "lblUsernameError";
            lblUsernameError.Size = new Size(0, 13);
            lblUsernameError.Visible = false;
            
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(20, 90);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(46, 19);
            lblEmail.Text = "Email:";
            
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(150, 87);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(300, 25);
            txtEmail.TabIndex = 2;
            
            // 
            // lblEmailError
            // 
            lblEmailError.AutoSize = true;
            lblEmailError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblEmailError.ForeColor = Color.Red;
            lblEmailError.Location = new Point(150, 113);
            lblEmailError.Name = "lblEmailError";
            lblEmailError.Size = new Size(0, 13);
            lblEmailError.Visible = false;
            
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(20, 145);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(52, 19);
            lblPhone.Text = "Phone:";

            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(150, 142);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(300, 25);
            txtPhone.TabIndex = 3;

            // 
            // lblPhoneError
            // 
            lblPhoneError.AutoSize = true;
            lblPhoneError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblPhoneError.ForeColor = Color.Red;
            lblPhoneError.Location = new Point(150, 168);
            lblPhoneError.Name = "lblPhoneError";
            lblPhoneError.Size = new Size(0, 13);
            lblPhoneError.Visible = false;
            
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(20, 200);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(73, 19);
            lblPassword.Text = "Password:";
        
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(150, 197);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.Size = new Size(300, 25);
            txtPassword.TabIndex = 4;
            
            // 
            // lblPasswordError
            // 
            lblPasswordError.AutoSize = true;
            lblPasswordError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblPasswordError.ForeColor = Color.Red;
            lblPasswordError.Location = new Point(150, 223);
            lblPasswordError.Name = "lblPasswordError";
            lblPasswordError.Size = new Size(0, 13);
            lblPasswordError.Visible = false;
            
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Location = new Point(20, 255);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(125, 19);
            lblConfirmPassword.Text = "Confirm Password:";
            
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Location = new Point(150, 252);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '●';
            txtConfirmPassword.Size = new Size(300, 25);
            txtConfirmPassword.TabIndex = 5;
        
            // 
            // lblConfirmPasswordError
            // 
            lblConfirmPasswordError.AutoSize = true;
            lblConfirmPasswordError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblConfirmPasswordError.ForeColor = Color.Red;
            lblConfirmPasswordError.Location = new Point(150, 278);
            lblConfirmPasswordError.Name = "lblConfirmPasswordError";
            lblConfirmPasswordError.Size = new Size(0, 13);
            lblConfirmPasswordError.Visible = false;
            
            // 
            // grpNotifierOptions
            // 
            grpNotifierOptions.Controls.Add(chkMessageBox);
            grpNotifierOptions.Controls.Add(chkHighlight);
            grpNotifierOptions.Controls.Add(chkErrorProvider);
            grpNotifierOptions.Controls.Add(chkToolTip);
            grpNotifierOptions.Controls.Add(chkSummaryLabel);
            grpNotifierOptions.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            grpNotifierOptions.Location = new Point(540, 55);
            grpNotifierOptions.Name = "grpNotifierOptions";
            grpNotifierOptions.Size = new Size(220, 180);
            grpNotifierOptions.TabIndex = 1;
            grpNotifierOptions.Text = "Notifier Options";
            
            // 
            // chkMessageBox
            // 
            chkMessageBox.AutoSize = true;
            chkMessageBox.Location = new Point(20, 30);
            chkMessageBox.Name = "chkMessageBox";
            chkMessageBox.Size = new Size(146, 23);
            chkMessageBox.Text = "MessageBox Notifier";
            chkMessageBox.Checked = false;
            
            // 
            // chkHighlight
            // 
            chkHighlight.AutoSize = true;
            chkHighlight.Location = new Point(20, 60);
            chkHighlight.Name = "chkHighlight";
            chkHighlight.Size = new Size(145, 23);
            chkHighlight.Text = "Highlight TextBoxes";
            chkHighlight.Checked = true;
        
            // 
            // chkErrorProvider
            // 
            chkErrorProvider.AutoSize = true;
            chkErrorProvider.Location = new Point(20, 90);
            chkErrorProvider.Name = "chkErrorProvider";
            chkErrorProvider.Size = new Size(107, 23);
            chkErrorProvider.Text = "ErrorProvider";
            chkErrorProvider.Checked = true;
            
            // 
            // chkToolTip
            // 
            chkToolTip.AutoSize = true;
            chkToolTip.Location = new Point(20, 120);
            chkToolTip.Name = "chkToolTip";
            chkToolTip.Size = new Size(70, 23);
            chkToolTip.Text = "ToolTip";
            chkToolTip.Checked = true;
        
            // 
            // chkSummaryLabel
            // 
            chkSummaryLabel.AutoSize = true;
            chkSummaryLabel.Location = new Point(20, 150);
            chkSummaryLabel.Name = "chkSummaryLabel";
            chkSummaryLabel.Size = new Size(118, 23);
            chkSummaryLabel.Text = "Summary Label";
            chkSummaryLabel.Checked = true;
            
            // 
            // btnValidate
            // 
            btnValidate.BackColor = Color.DodgerBlue;
            btnValidate.FlatStyle = FlatStyle.Flat;
            btnValidate.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnValidate.ForeColor = Color.White;
            btnValidate.Location = new Point(540, 250);
            btnValidate.Name = "btnValidate";
            btnValidate.Size = new Size(100, 40);
            btnValidate.TabIndex = 6;
            btnValidate.Text = "Validate";
            btnValidate.UseVisualStyleBackColor = false;
            
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.Gray;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(660, 250);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(100, 40);
            btnClear.TabIndex = 7;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = false;
        
            // 
            // lblSummary
            // 
            lblSummary.BorderStyle = BorderStyle.FixedSingle;
            lblSummary.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSummary.Location = new Point(20, 390);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(740, 100);
            lblSummary.Text = "Enter data and click Validate to check...";
        
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 100;
        
            // 
            // UserRegistrationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(780, 510);
            Controls.Add(lblTitle);
            Controls.Add(grpUserInfo);
            Controls.Add(grpNotifierOptions);
            Controls.Add(btnValidate);
            Controls.Add(btnClear);
            Controls.Add(lblSummary);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "UserRegistrationForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ValidationFramework Demo - WinForms";
            
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            grpUserInfo.ResumeLayout(false);
            grpUserInfo.PerformLayout();
            grpNotifierOptions.ResumeLayout(false);
            grpNotifierOptions.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private GroupBox grpUserInfo;
        private GroupBox grpNotifierOptions;
        
        private Label lblUsername;
        private Label lblEmail;
        private Label lblPhone;
        private Label lblPassword;
        private Label lblConfirmPassword;
 
        private TextBox txtUsername;
        private TextBox txtEmail;
        private TextBox txtPhone;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        
        private Label lblUsernameError;
        private Label lblEmailError;
        private Label lblPhoneError;
        private Label lblPasswordError;
        private Label lblConfirmPasswordError;
      
        private CheckBox chkMessageBox;
        private CheckBox chkHighlight;
        private CheckBox chkErrorProvider;
        private CheckBox chkToolTip;
        private CheckBox chkSummaryLabel;
        
        private Button btnValidate;
        private Button btnClear;
        private Label lblSummary;
        
        private ErrorProvider errorProvider;
        private ToolTip toolTip;
    }
}
