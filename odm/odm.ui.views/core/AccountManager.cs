using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using utils;
using System.Xml.Serialization;

namespace odm.ui.core
{
    [XmlRootAttribute(ElementName = "Account", IsNullable = false)]
    public struct Account 
    {
        string _password;
        public string Password { get { return _password ?? string.Empty; } set { _password = value; } }
        string _name;
        public string Name { get { return _name ?? string.Empty; } set { _name = value; } }

        public static readonly Account Anonymous = new Account() { Name=string.Empty, Password = string.Empty };
        public bool IsAnonymous { get { return Anonymous.Equals(this); } }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Account another = (Account)obj;
            return this.Name == another.Name;
        }

        public static bool operator == (Account that, Account another)
        {
            return that.Equals(another);
        }
        public static bool operator !=(Account that, Account another)
        {
            return !(that == another);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }


    

    public sealed class AccountManager
    {

        static readonly AccountManager _instance = new AccountManager();
        public static AccountManager Instance { get { return _instance; } }

        private AccountManager()
        {
            _currentAccount = Load();
        }

        public event EventHandler CurrentAccountChanged;
        Account _currentAccount = Account.Anonymous;
        public Account CurrentAccount
        {
            get { return _currentAccount; }
            private set 
            {
                if (_currentAccount == value)
                    return;
                _currentAccount = value;
                
                if (this.CurrentAccountChanged != null)
                    this.CurrentAccountChanged(this, EventArgs.Empty);
            }
        }

        public bool Autorized
        {
            get { return Account.Anonymous != this.CurrentAccount; }
        }

        public void SetCurrentAccount(Account account, bool remember)
        {
            this.CurrentAccount = account;
            Save(remember ? account : Account.Anonymous);
        }

        private Account Load()
        {
            if (!File.Exists(settingsPath))
                return Account.Anonymous;

            try
            {
                using (var sr = File.OpenText(settingsPath))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(Account));
                    return (Account)deserializer.Deserialize(sr);
                }
            }
            catch (Exception err)
            {
                dbg.Error(err);
                return Account.Anonymous;
            }
        }

        private void Save(Account account)
        {
            try
            {
                if (File.Exists(settingsPath))
                    File.Delete(settingsPath);
                
                using (var sr = File.CreateText(settingsPath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Account));
                    serializer.Serialize(sr, account);
                }
            }
            catch (Exception err)
            {
                dbg.Error(err);
            }
        }
        
        readonly string settingsPath = AppDefaults.ConfigFolderPath + "account.def.xml";
        
        
    }
}
