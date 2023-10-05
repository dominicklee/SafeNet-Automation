Imports System.Windows.Automation

Module Module1
    Private password As String = "" 'Change password here
    Private count As Integer = 0

    Delegate Sub AutomationEventHandler(sender As Object, e As AutomationEventArgs)

    Sub Main()
        Dim handler As New AutomationEventHandler(AddressOf OnWindowOpened)
        Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, TreeScope.Children, New System.Windows.Automation.AutomationEventHandler(AddressOf OnWindowOpened))
        Console.WriteLine("Ready. Waiting for SafeNet prompt...")
        Do
            Dim k = Console.ReadKey(True)
            If k.Key = ConsoleKey.Q Then Exit Do
        Loop
        Automation.RemoveAllEventHandlers()
    End Sub

    Private Sub OnWindowOpened(ByVal sender As Object, ByVal e As AutomationEventArgs)
        Dim element = CType(sender, AutomationElement)
        If element.Current.Name = "Token Logon" Then
            Dim pattern = CType(element.GetCurrentPattern(WindowPattern.Pattern), WindowPattern)
            pattern.WaitForInputIdle(10000)

            Dim edit = element.FindFirst(TreeScope.Descendants, New AndCondition(
                    New PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit),
                    New PropertyCondition(AutomationElement.NameProperty, "Token Password:")))

            Dim ok = element.FindFirst(TreeScope.Descendants, New AndCondition(
                    New PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button),
                    New PropertyCondition(AutomationElement.NameProperty, "OK")))

            If edit IsNot Nothing AndAlso ok IsNot Nothing Then
                count += 1
                Dim vp = CType(edit.GetCurrentPattern(ValuePattern.Pattern), ValuePattern)
                vp.SetValue(password)
                Console.WriteLine("SafeNet window (count: " & count & " window(s)) detected. Setting password...")

                Dim ip = CType(ok.GetCurrentPattern(InvokePattern.Pattern), InvokePattern)
                ip.Invoke()
            Else
                Console.WriteLine("SafeNet window detected but not with edit and button...")
            End If
        End If
    End Sub
End Module
