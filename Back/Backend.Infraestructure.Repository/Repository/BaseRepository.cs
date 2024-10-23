using Backend.CrossCuting.Helpers;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Infraestructure.Repository.Repository
{
    public class BaseRepository
    {
        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection { get { return Transaction.Connection; } }

        private readonly AppConfiguration _configuration;
        public BaseRepository(IDbTransaction transaction)
        {
            Transaction = transaction;
            _configuration = new AppConfiguration();
        }
        public static string Sha256(string randomString)
        {
            SHA256Managed crypt = new();
            string hash = string.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }
    }
}
