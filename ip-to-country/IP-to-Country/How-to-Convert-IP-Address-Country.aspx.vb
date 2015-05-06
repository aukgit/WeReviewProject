
Partial Class How_to_Convert_IP_Address_Country
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim nowip As String
        nowip = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If nowip = "" Then
            nowip = Request.ServerVariables("REMOTE_ADDR")
        End If

        If txtIPAddress.Text = "" Then
            txtIPAddress.Text = nowip
        End If
        lblError.Text = ""
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        On Error GoTo HandleError

        Dim dottedip As String
        Dim Dot2LongIP As Double
        Dim PrevPos As Double
        Dim pos As Double
        Dim num As Double

        dottedip = txtIPAddress.Text

            For i = 1 To 4
                pos = InStr(PrevPos + 1, dottedip, ".", 1)
                If i = 4 Then
                    pos = Len(dottedip) + 1
                End If
                num = Int(Mid(dottedip, PrevPos + 1, pos - PrevPos - 1))
                PrevPos = pos
                Dot2LongIP = ((num Mod 256) * (256 ^ (4 - i))) + Dot2LongIP
            Next

        txtIPNumber.Text = Dot2LongIP

HandleError:
        lblError.Text = Err.Description
    End Sub

    Protected Sub SqlDataSource1_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs) Handles SqlDataSource1.Selecting

    End Sub
End Class
