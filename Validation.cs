using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp1
{
    public class Number : System.ComponentModel.IDataErrorInfo, INotifyPropertyChanged
    {
        //creem o clasa model pentru formularul nostru de introducere date
        //clasa implementeaza interfetele IDataErrorInfo si INotifyPropertyChanged
        private string mPhoneNumber;
        private string mSubscriber;
        public event PropertyChangedEventHandler PropertyChanged;
        public Number(string nr, string subscr)
        {
            mPhoneNumber = nr;
            mSubscriber = subscr;
        }
        //proprietate ce coresounde campului txtPhoneNumber
        public string PhoneNumber
        {
            get
            {
                return mPhoneNumber;
            }
            set
            {
                mPhoneNumber = value;
                PropertyChanged(this, new
                PropertyChangedEventArgs("PhoneNumber"));
            }
        }

        //proprietate ce corespunde campului txtSubscriber
        public string Subscriber
        {
            get
            { return mSubscriber;
            }
            set
            {
                mSubscriber = value;
                PropertyChanged(this, new
                PropertyChangedEventArgs("Subscriber"));
            }
        }

        //proprietate pentru retinerea erorii
        public string Error
        {
            get { return null; }
        }
        //indexer care implementeaza regulile de validare
        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case "PhoneNumber": //reguli de validare pentru txtPhoneNumber
                        if (PhoneNumber == "")
                        {
                            result = "Phone Number cannot be empty!";
                        }
                        if (PhoneNumber.Length < 10)
                            result = "Phone Number must contain at least 10 digits! ";
                        char[] explode = PhoneNumber.ToCharArray();
                        for (int i = 0; i < explode.Length; i++)
                        {
                            if (explode[i] < '0' || explode[i] > '9')
                            {
                                result = "Phone Number must contain only numbers!";
                            }
                        }
                        break;
                    //aici putem adauga reguli si pentru txtSubscriber
                    //in acest exemplu am utilizat pentru celalalt camp o alternativa
                    //prin clase Validator
                    default:
                        break;
                }
                return result;
            }
        }
    }

    public class Numbers : System.Collections.ObjectModel.ObservableCollection<Number>
    {
        //colectie de obiecte Number - Observable
        public Numbers()
        {
            Number aName = new Number("PhoneNumber " + (this.Count + 1).ToString(),
            "Subscriber " + (this.Count + 1).ToString());
            this.Add(aName);
        }
    }

    //validator pentru camp required
    public class StringNotEmpty : System.Windows.Controls.ValidationRule
    {
        //mostenim din clasa ValidationRule (abstracta)
        //suprascriem metoda abstracta Validate ce returneaza un
        //ValidationResult
        public override ValidationResult Validate(object value,
        System.Globalization.CultureInfo cultureinfo)
        {
            string aString = value.ToString();
            if (aString == "")
                return new ValidationResult(false, "String cannot be empty");
            return new ValidationResult(true, null);
        }
    }

    //validator pentru lungime minima a string-ului
    public class StringMinLengthValidator : System.Windows.Controls.ValidationRule
    {
        public override ValidationResult Validate(object value,
        System.Globalization.CultureInfo cultureinfo)
        {
            string aString = value.ToString();
            if (aString.Length < 3)
                return new ValidationResult(false, "String must have at least 3 characters!");
        return new ValidationResult(true, null);
        }
    }
}
