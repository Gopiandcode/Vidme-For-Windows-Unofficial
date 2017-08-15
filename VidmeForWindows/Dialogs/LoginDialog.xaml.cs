using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VidmeForWindows.Dialogs
{

    public enum LoginDialogState {
        Login,
        Cancel,
        Failed,
        Nothing
    }
    

    public sealed partial class LoginDialog : ContentDialog
    {

        public LoginDialogState Result { get; set; }

        public LoginDialog()
        {
            this.InitializeComponent();
            this.Result = LoginDialogState.Nothing;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Debug.WriteLine(Username.Text + ", " + Password.Password + ", " + SaveCredentialsCheckBox.IsChecked);

            if (String.IsNullOrWhiteSpace(Username.Text) || String.IsNullOrWhiteSpace(Password.Password))
                this.Result = LoginDialogState.Failed;
            else
                this.Result = LoginDialogState.Login;

            this.Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Result = LoginDialogState.Cancel;

            this.Hide();
        }
    }
}
