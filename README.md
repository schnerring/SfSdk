SfSdk
=====

Provides a .NET 4.5 API to easily communicate with Shakes &amp; Fidget servers.

Unit Tests
----------

Some of the provided unit tests require a valid Shakes &amp; Fidget account to run properly. To prevent that any account information is stored in the repository just add TestAccount.txt file to the solution folder and put in your account credentials in the following format:
```
Username
Password
ServerUri
```
The TestAccount.txt file is listed in the .gitignore, so outgoing commits don't accidently include your account credentials.
