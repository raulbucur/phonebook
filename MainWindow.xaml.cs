using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    enum ActionState { New, Edit, Delete, Nothing }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        PhoneNumbersDataSet phoneNumbersDataSet = new PhoneNumbersDataSet();
        PhoneNumbersDataSetTableAdapters.PhoneNumbersTableAdapter tblPhoneNumbersAdapter =
            new PhoneNumbersDataSetTableAdapters.PhoneNumbersTableAdapter();
        Binding txtPhoneNumberBinding = new Binding();
        Binding txtSubscriberBinding = new Binding();
        Binding txtContractBinding = new Binding();
        Binding txtContractDateBinding = new Binding();

        //aceasta colectie va fi Binding Source
        //pentru validarea datelor
        Numbers myNumbers = new Numbers();

        public MainWindow()
        {
            InitializeComponent();

            grdMain.DataContext = phoneNumbersDataSet.PhoneNumbers;
            txtPhoneNumberBinding.Path = new PropertyPath("Phonenum");
            txtSubscriberBinding.Path = new PropertyPath("Subscriber");
            txtContractBinding.Path = new PropertyPath("ContractValue");
            txtContractDateBinding.Path = new PropertyPath("ContractDate");
            txtContractDateBinding.StringFormat = "MM/dd/yyyy";

            txtPhoneNumber.SetBinding(TextBox.TextProperty, txtPhoneNumberBinding);
            txtSubscriber.SetBinding(TextBox.TextProperty, txtSubscriberBinding);
            txtContract.SetBinding(TextBox.TextProperty, txtContractBinding);
            txtContractDate.SetBinding(TextBox.TextProperty, txtContractDateBinding);
        }

        private void SetValidationBinding()
        {
            //seteaza Binding-ul pe formular pentru validare la introducere de date
            Binding txtPhoneNumberValidationBinding = new Binding(); //creem binding-uri noi
            Binding txtSubscriberValidationBinding = new Binding();
            //facem clear la binding-urile existente
            BindingOperations.ClearBinding(txtPhoneNumber, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtSubscriber, TextBox.TextProperty);
            //modificam binding-ul cu validatorul pentru input de date
            //pentru PhoneNumber - folosim IDataErrorInfo
            txtPhoneNumberValidationBinding.Source = myNumbers;
            txtPhoneNumberValidationBinding.Path = new PropertyPath("PhoneNumber");
            txtPhoneNumberValidationBinding.NotifyOnValidationError = true;
            txtPhoneNumberValidationBinding.UpdateSourceTrigger =
            UpdateSourceTrigger.LostFocus;
            txtPhoneNumberValidationBinding.ValidatesOnDataErrors = true;
            txtPhoneNumber.SetBinding(TextBox.TextProperty,
            txtPhoneNumberValidationBinding); //setare binding nou
                                              //pentru Subscriber - folosim Clasa String Validator
            txtSubscriberValidationBinding.Source = myNumbers;
            txtSubscriberValidationBinding.Path = new PropertyPath("Subscriber");
            txtSubscriberValidationBinding.NotifyOnValidationError = true;
            txtSubscriberValidationBinding.Mode = BindingMode.TwoWay;
            //string not emtpy validator
            txtSubscriberValidationBinding.ValidationRules.Add(new StringNotEmpty());
            //string min length validator
            txtSubscriberValidationBinding.ValidationRules.Add(new
            StringMinLengthValidator());
            txtSubscriber.SetBinding(TextBox.TextProperty,
            txtSubscriberValidationBinding); //setare binding nou
        }

        private void SetDbBinding()
        { //seteaza binding-ul inapoi cu baza de date
          //!!salvam starea celor doua casute de text inainte de refacerea Binding-ului
            string temp_nr = txtPhoneNumber.Text;
            string temp_subscr = txtSubscriber.Text;
            int selected_item = lstPhones.SelectedIndex;
            //refacem binding-ul cu BD
            txtPhoneNumber.SetBinding(TextBox.TextProperty, txtPhoneNumberBinding);
            txtSubscriber.SetBinding(TextBox.TextProperty, txtSubscriberBinding);
            //!!!refacem starea formularului
            txtPhoneNumber.Text = temp_nr;
            txtSubscriber.Text = temp_subscr;
            lstPhones.SelectedIndex = selected_item;
        }

        private void lstPhonesLoad()
        {
            tblPhoneNumbersAdapter.Fill(phoneNumbersDataSet.PhoneNumbers);
        }

        private void grdMain_Loaded(object sender, RoutedEventArgs e)
        {
            lstPhonesLoad();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Close Application?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            WpfApp1.PhoneNumbersDataSet phoneNumbersDataSet =
((WpfApp1.PhoneNumbersDataSet)(this.FindResource("phoneNumbersDataSet")));
            System.Windows.Data.CollectionViewSource phoneNumbersViewSource =
            ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneNumbersViewSource")));
            phoneNumbersViewSource.View.MoveCurrentToFirst();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            lstPhones.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
            txtPhoneNumber.IsEnabled = true;
            txtSubscriber.IsEnabled = true;
            txtContract.IsEnabled = true;
            txtContractDate.IsEnabled = true;
            BindingOperations.ClearBinding(txtPhoneNumber, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtSubscriber, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtContract, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtContractDate, TextBox.TextProperty);
            //setare binding pentru validare date
            SetValidationBinding();
            txtPhoneNumber.Text = "";
            txtSubscriber.Text = "";
            txtContract.Text = "";
            txtContractDate.Text = "";
            Keyboard.Focus(txtPhoneNumber);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempPhonenum = txtPhoneNumber.Text.ToString();
            string tempSubscriber = txtSubscriber.Text.ToString();
            string tempContract = txtContract.Text.ToString();
            string tempContractDate = txtContractDate.Text.ToString();
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            lstPhones.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
            txtPhoneNumber.IsEnabled = true;
            txtSubscriber.IsEnabled = true;
            txtContract.IsEnabled = true;
            txtContractDate.IsEnabled = true;
            BindingOperations.ClearBinding(txtPhoneNumber, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtSubscriber, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtContract, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtContractDate, TextBox.TextProperty);
            //setare binding pentru validare date
            SetValidationBinding();
            txtPhoneNumber.Text = tempPhonenum;
            txtSubscriber.Text = tempSubscriber;
            txtContract.Text = tempContract;
            txtContractDate.Text = tempContractDate;
            Keyboard.Focus(txtPhoneNumber);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
            lstPhones.IsEnabled = true;
            btnPrevious.IsEnabled = true;
            btnNext.IsEnabled = true;
            txtPhoneNumber.IsEnabled = false;
            txtSubscriber.IsEnabled = false;
            txtContract.IsEnabled = false;
            txtContractDate.IsEnabled = false;
            txtPhoneNumber.SetBinding(TextBox.TextProperty, txtPhoneNumberBinding);
            txtSubscriber.SetBinding(TextBox.TextProperty, txtSubscriberBinding);
            txtContract.SetBinding(TextBox.TextProperty, txtContractBinding);
            txtContractDate.SetBinding(TextBox.TextProperty, txtContractDateBinding);
            //setare binding cu BD
            SetDbBinding();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //setare binding cu BD
            bool hasErrors = Validation.GetHasError(txtPhoneNumber) || Validation.GetHasError(txtSubscriber);

            SetDbBinding();
            //using System.Data
            if (action == ActionState.New)
            {
                if(!hasErrors)
                try
                {
                    DataRow newRow = phoneNumbersDataSet.PhoneNumbers.NewRow();
                    newRow.BeginEdit();
                    newRow["Phonenum"] = txtPhoneNumber.Text.Trim();
                    newRow["Subscriber"] = txtSubscriber.Text.Trim();
                    newRow["ContractValue"] = txtContract.Text.Trim();
                    newRow["ContractDate"] = txtContractDate.Text.Trim();
                    newRow.EndEdit();
                    phoneNumbersDataSet.PhoneNumbers.Rows.Add(newRow);
                    tblPhoneNumbersAdapter.Update(phoneNumbersDataSet.PhoneNumbers);
                    phoneNumbersDataSet.AcceptChanges();
                }
                catch (DataException ex)
                {
                    phoneNumbersDataSet.RejectChanges();
                    MessageBox.Show(ex.Message);
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                lstPhones.IsEnabled = true;
                //btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;
                txtPhoneNumber.IsEnabled = false;
                txtSubscriber.IsEnabled = false;
                txtContract.IsEnabled = false;
                txtContractDate.IsEnabled = false;
                CollectionViewSource.GetDefaultView(phoneNumbersDataSet.PhoneNumbers).
                    MoveCurrentToLast();
            }
            else
            if (action == ActionState.Edit)
            {
                if (!hasErrors)
                    try
                {
                    DataRow editRow = phoneNumbersDataSet.PhoneNumbers.Rows[lstPhones.SelectedIndex];
                    editRow.BeginEdit();
                    editRow["Phonenum"] = txtPhoneNumber.Text.Trim();
                    editRow["Subscriber"] = txtSubscriber.Text.Trim();
                    editRow["ContractValue"] = txtContract.Text.Trim();
                    editRow["ContractDate"] = txtContractDate.Text.Trim();
                    editRow.EndEdit();
                    tblPhoneNumbersAdapter.Update(phoneNumbersDataSet.PhoneNumbers);
                    phoneNumbersDataSet.AcceptChanges();
                }
                catch (DataException ex)
                {
                    phoneNumbersDataSet.RejectChanges();
                    MessageBox.Show(ex.Message);
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                lstPhones.IsEnabled = true;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;
                txtPhoneNumber.IsEnabled = false;
                txtSubscriber.IsEnabled = false;
                txtContract.IsEnabled = false;
                txtContractDate.IsEnabled = false;
                txtPhoneNumber.SetBinding(TextBox.TextProperty, txtPhoneNumberBinding);
                txtSubscriber.SetBinding(TextBox.TextProperty, txtSubscriberBinding);
                txtContract.SetBinding(TextBox.TextProperty, txtContractBinding);
                txtContractDate.SetBinding(TextBox.TextProperty, txtContractDateBinding);
            }

        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView navigationView =
            CollectionViewSource.GetDefaultView(phoneNumbersDataSet.PhoneNumbers);
            if (navigationView.CurrentPosition > 0)
                navigationView.MoveCurrentToPrevious();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView navigationView =
            CollectionViewSource.GetDefaultView(phoneNumbersDataSet.PhoneNumbers);
            if (navigationView.CurrentPosition < phoneNumbersDataSet.PhoneNumbers.Count - 1)
                navigationView.MoveCurrentToNext();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete selected Contact?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                == MessageBoxResult.Yes)
            {
                try
                {
                    DataRow deleteRow = phoneNumbersDataSet.PhoneNumbers.Rows[lstPhones.SelectedIndex];
                    deleteRow.Delete();
                    tblPhoneNumbersAdapter.Update(phoneNumbersDataSet.PhoneNumbers);
                    phoneNumbersDataSet.AcceptChanges();
                }
                catch (DataException ex)
                {
                    phoneNumbersDataSet.RejectChanges();
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void grdMain_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) //daca exista o eroare
            {
                if (e.Source == txtPhoneNumber) //daca eroare este in txtPhoneNumber
                    this.txtPhoneNumber.ToolTip = e.Error.ErrorContent.ToString();
                if (e.Source == txtSubscriber) //daca eroarea este in txtSubscriber
                    this.txtSubscriber.ToolTip = e.Error.ErrorContent.ToString();
            }
        }
    }
}
