SfSdk
=====

Provides a .NET 4.5 API to easily communicate with Shakes &amp; Fidget servers.

Getting started
---------------

<h3>1. Setup the NuGet Package Manager</h3>
To keep the size of the repository as small as possible, the <a href="./packages/.gitignore" target="_blank">.gitignore</a> file in the packages folder includes all downloaded NuGet packages. As referenced packages cannot be initially resolved, you need to enable this option to keep things as easy as possible:

<img src="http://docs.nuget.org/docs/workflows/images/allow-package-restore-configuration.png" /><br />
<i>Source: <a href="http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages" target="_blank">NuGet Docs</a>

<h3>2. Strong-Name signing</h3>
The <i>SfSdk</i> assembly is strong-name signed in order to be able to test its internals. To make the internals of the <i>SfSdk</i> assembly visible to the <i>SfSdk.Tests</i> assembly, the ```InternalsVisibleToAttribute``` is set in the <i>SfSdk</i>'s <a href="./SfSdk/Properties/AssemblyInfo.cs" target="_blank">AssemblyInfo.cs</a> file. 

> To sign an assembly with a strong name, you must have a public/private key pair. This public and private cryptographic key pair is used during compilation to create a strong-named assembly. You can create a key pair using the <a href="http://msdn.microsoft.com/de-DE/library/k5b5tt23(v=vs.71).aspx" target="_blank">Strong Name tool (Sn.exe)</a>. Key pair files usually have an .snk extension. <i>Source: <a href="http://msdn.microsoft.com/de-DE/library/6f05ezxy(v=vs.71).aspx" target="_blank">MSDN</a></i>

In case you have not experienced strong-name signing of assemblies yet, check out <a href="http://msdn.microsoft.com/de-DE/library/xwb8f617(v=vs.71).aspx" target="_blank">this MSDN article</a> to get started. Usually typing this in the Windows command line should do just fine though (this was under Windows 8):

```
cd "C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools"
sn -k "C:\Your\SfSdk\Solution\Folder\SfSdk.snk"
```

All projects but <i>SfSdk.Tests</i> should compile now, as the projects contain just links to the <b>SfSdk.snk</b> file. These links are automatically resolved if the corresponding file exists.

If you'd like to compile <i>SfSdk.Tests</i> either, you have to change the PublicKey of the ```InternalsVisibleToAttribute``` in the <i>SfSdk</i>'s <a href="./SfSdk/Properties/AssemblyInfo.cs" target="_blank">AssemblyInfo.cs</a> file to the corresponding public key of <b>your SfSdk.snk</b> file. (Please undo changes to the PublicKey as you commit - I'm searching for a workaround for this!)

<a href="http://blogs.msdn.com/b/kaevans/archive/2008/06/18/getting-public-key-token-of-assembly-within-visual-studio.aspx" target="_blank">Getting Public Key Token of Assembly Within Visual Studio</a> is a great MSDN article worth checking out. The mentioned shortcut is a huge timesaver!

<h3>3. Add account credentials for unit tests</h3>

Some of the provided unit tests require a valid Shakes &amp; Fidget account to run properly. To prevent that any account information is stored in the repository just add <b>TestAccount.txt</b> file to the solution folder and put in your account credentials in the following format. The <i>SfSdk.Tests</i> project contains a link to this file which is automatically resolved if the corresponding file exists:
```
Username
Password
ServerUri
```
The TestAccount.txt file is listed in the .gitignore, so outgoing commits don't accidently include your account credentials.

<b>I highly recommend using a dummy account instead of using your active account as the unit tests do not try to behave as unobtrusive as the SfBot does.

Congrantulations! You have configured the solution properly and everything is ready to run.
