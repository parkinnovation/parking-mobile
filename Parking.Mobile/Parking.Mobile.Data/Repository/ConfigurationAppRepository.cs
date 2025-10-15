using System;
using Parking.Mobile.Entity;
using SQLite;
using System.Text;

namespace Parking.Mobile.Data.Repository
{
    public class ConfigurationAppRepository
    {
        private SQLiteConnection _connection;

        public ConfigurationAppRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public ConfigurationApp Get()
        {
            return _connection.Table<ConfigurationApp>().FirstOrDefault();
        }

        public string EncodeToBase64(string texto)
        {
            try
            {
                byte[] textoAsBytes = Encoding.UTF8.GetBytes(texto);
                string resultado = System.Convert.ToBase64String(textoAsBytes);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Save(ConfigurationApp configurationApp)
        {
            if (_connection.Table<ConfigurationApp>().Where(x => x.IDConfigurationApp == configurationApp.IDConfigurationApp).Count() == 0)
            {
                _connection.Insert(configurationApp);
            }
            else
            {
                _connection.Update(configurationApp);
            }
        }
    }
}

