using System;
using System.IO;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace SfSdk.Tests
{
    public class TestAccountFixture
    {
        private readonly string _passwordHash;
        private readonly Uri _serverUri;
        private readonly string _username;

        public TestAccountFixture()
        {
            // Add this text file to the solution folder,
            // the first line containing the username,
            // the second line containing the password,
            // the third line containing the server URL
            string[] lines = File.ReadAllLines("TestAccount.txt");

            if (lines.Length != 3) throw new NotSupportedException("The TestAccount file has an invalid format.");

            _username = lines[0];
            _passwordHash = lines[1].ToMd5Hash();
            _serverUri = new UriBuilder(lines[2]).Uri;
        }

        public string Username
        {
            get { return _username; }
        }

        public string PasswordHash
        {
            get { return _passwordHash; }
        }

        public Uri ServerUri
        {
            get { return _serverUri; }
        }
    }
}