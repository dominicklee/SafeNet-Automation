# SafeNet Automation
An easy to use VB.NET console solution for automating SafeNet password prompt for Industrial Server Automation

## Overview ##
As a cloud systems admin, programmer, and inventor, I enjoy automation as it helps keep processes more efficient. As such, I have a dedicated Windows workstation that takes the role of a code-signing server. When signing code, the SafeNet Authentication Client would show a "password prompt" asking the user to enter their password and click "OK" in order to proceed with the code-signing. This may be fine for signing a single binary, but often becomes tedious when signing hundreds of binaries. 

As some community users mentioned before, one can technically enable "Single Logon" which tells SafeNet to **prompt the user only once per session** for password. However, if you keep your USB hardware token permanently plugged into the computer and leave it online (like a typical server), this potentially allows **all** applications on that computer to sign code without your attention. The purpose of this code, the SafeNet Automation, is **not** to enable "Single Logon" but only **allow exclusive access to one authorized program**, your program, to enter the password on your behalf when the SafeNet prompt shows up.

![](https://raw.githubusercontent.com/dominicklee/SafeNet-Automation/main/img/safenetdemo.gif)

## Disclaimer ##
The author is not affiliated with Thales SafeNet. By using any part of the code in this repository, you agree that the author is not responsible nor liable for any damages, loss, or undesired consequences as a result of your usage. You agree to use this code responsibly at your own risk. You agree to use this code only in a lawful manner (i.e. on a computer and certificate that you own). Finally, you understand that misuse may result in the USB token becoming permanently locked due to failed login attempts. This solution is for free and personal use only and must not be commercialized.

## Usage ##
There are two ways to use this: As a console application, or by add the released `AutoSafeNet.dll` file as a reference in your VB.NET program.

Both solutions uses Windows' `UIAutomationClient` and `UIAutomationTypes`

**Method 1: Console Application** 
1. Open the `TestSafeNet.sln` project using Visual Studio 2017 or later.
2. Open the `Module1.vb` file and change the `password` to the one for your USB token.
3. Build the solution and run it on the computer with your SafeNet Client.
4. Keep the console open and run your code-signing as usual. This app will enter your password when prompted.

**Method 2: Add signed DLL into your project**
1. Open your custom project which you'll use for code-signing.
2. Click `Project` -> `Add Reference...` and browse for `AutoSafeNet.dll`.
3. In your code, add `Imports AutoSafeNet`.
4. Declare this global variable: `Dim sfa As SafeNetAutomator`
5. Right before you start the code-signing process, run this code:
```vbnet
sfa = New SafeNetAutomator("yourPassword")
```
6. After your code-signing process, run this to dispose the automation:
```vbnet
sfa.Dispose()
```

Putting it all together, your code would look like this:

```vbnet
Imports AutoSafeNet

Public Class Form1

    Dim sfa As SafeNetAutomator

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        sfa = New SafeNetAutomator("yourPassword")
        MsgBox("Started!")
    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        sfa.Dispose()
        MsgBox("Stopped!")
    End Sub
End Class

```

In my professional opinion, I believe this is a safer option than using SafeNet's "Single Logon" since it doesn't let the guard down, and still ensures that **only your** program has access for the length of time that you need. This also helps reduce failed password attempts by the legitimate signer.

## Additional Info ##
Special thanks to the authors who provided a very helpful C# solution (Simon Mourier and Martin Prikryl) on StackOverflow: [https://stackoverflow.com/a/26126701](https://stackoverflow.com/a/26126701)

There are many parts of that C# solution that have been re-factored to provide something clean and usable in a custom code-signing application.

**Compatibility:** Tested with Windows 10 verion 22H2, running SafeNet Authentication Client 10.8 R8 for Windows (10.8.2210.0). [Click here](https://comodoca.my.salesforce.com/sfc/p/1N000002Ljih/a/3l000000GBJW/HrxYd4uTf3JVk3dvth9gjOtikQXeF6_dgu_FTEyqk8A) to download the SafeNet Auth Client.