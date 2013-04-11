using System;
using System.IO;

namespace SfSdk.Tests
{
    public class TestAccountFixture
    {
        public TestAccountFixture()
        {
            // Add this text file to the solution folder,
            // the first line containing the username,
            // the second line containing the password,
            // the third line containing the server URL
            string[] lines = File.ReadAllLines("TestAccount.txt");

            Username = lines[0];
            PasswordHash = lines[1].ToMd5Hash();
            ServerUri = new UriBuilder(lines[2]).Uri;
        }

        public Uri ServerUri { get; private set; }

        public string PasswordHash { get; private set; }

        public string Username { get; private set; }
    }
}