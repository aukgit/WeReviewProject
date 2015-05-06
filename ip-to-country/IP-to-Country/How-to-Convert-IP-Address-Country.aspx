<%@ Page Language="VB" AutoEventWireup="false" CodeFile="How-to-Convert-IP-Address-Country.aspx.vb" Inherits="How_to_Convert_IP_Address_Country" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Convert IPAddress To Country</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: left">
    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionStringIP %>" 
            DeleteCommand="DELETE FROM [ip-to-country] WHERE [ID] = ?" 
            InsertCommand="INSERT INTO [ip-to-country] ([ID], [BeginingIP], [EndingIP], [TwoCountryCode], [ThreeCountryCode], [CountryName]) VALUES (?, ?, ?, ?, ?, ?)" 
            ProviderName="<%$ ConnectionStrings:ConnectionStringIP.ProviderName %>" 
            SelectCommand="SELECT * FROM [ip-to-country] WHERE (([BeginingIP] &lt;= ?) AND ([EndingIP] &gt;= ?))" 
            UpdateCommand="UPDATE [ip-to-country] SET [BeginingIP] = ?, [EndingIP] = ?, [TwoCountryCode] = ?, [ThreeCountryCode] = ?, [CountryName] = ? WHERE [ID] = ?">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtIPNumber" DefaultValue="" 
                    Name="BeginingIP" PropertyName="Text" Type="Double" />
                <asp:ControlParameter ControlID="txtIPNumber" DefaultValue="" 
                    Name="EndingIP" PropertyName="Text" Type="Double" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="ID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="BeginingIP" Type="Double" />
                <asp:Parameter Name="EndingIP" Type="Double" />
                <asp:Parameter Name="TwoCountryCode" Type="String" />
                <asp:Parameter Name="ThreeCountryCode" Type="String" />
                <asp:Parameter Name="CountryName" Type="String" />
                <asp:Parameter Name="ID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="ID" Type="Int32" />
                <asp:Parameter Name="BeginingIP" Type="Double" />
                <asp:Parameter Name="EndingIP" Type="Double" />
                <asp:Parameter Name="TwoCountryCode" Type="String" />
                <asp:Parameter Name="ThreeCountryCode" Type="String" />
                <asp:Parameter Name="CountryName" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
        <br />
        IP Address :
        <asp:TextBox ID="txtIPAddress" runat="server" Wrap="False"></asp:TextBox>
        <br />
        IP Number :         <asp:TextBox ID="txtIPNumber" runat="server" 
            AutoPostBack="True" ReadOnly="True" Wrap="False"></asp:TextBox>
        <br />
        <br />
        <asp:DetailsView ID="DetailsView1" runat="server" BackColor="#DEBA84" 
            BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
            CellSpacing="2" DataSourceID="SqlDataSource1" Height="50px" Width="309px">
            <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
            <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
            <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
        </asp:DetailsView>
        <br />
        <asp:Button ID="Button1" runat="server" Text="Check To Country" />
        <br />
        <br />
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <br />
    
    </div>
    </form>
<p style="text-align: left">
    &nbsp;</p>
</body>
</html>
