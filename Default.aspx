<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Core_Elms._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div>

        <asp:Table ID="Table1" runat="server" Width="100%" BorderWidth="2" BorderStyle="Solid" BorderColor="Black">
            <asp:TableHeaderRow BorderWidth="2" BorderStyle="Solid" BorderColor="Black">

                <asp:TableHeaderCell BorderWidth="2" BorderStyle="Solid" BorderColor="Black">
                    <asp:Label ID="Label5" runat="server" Text="File Name"></asp:Label>
                </asp:TableHeaderCell>
                <asp:TableHeaderCell Width="50%" BorderWidth="2" BorderStyle="Solid" BorderColor="Black">
                    <asp:Label ID="Label2" runat="server" Text="File Picker"></asp:Label>
                </asp:TableHeaderCell>

                <asp:TableHeaderCell BorderWidth="2" BorderStyle="Solid" BorderColor="Black">

                    <asp:Label ID="Label6" runat="server" Text="Load Button"></asp:Label>
                </asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow BorderWidth="2" BorderStyle="Solid" BorderColor="Black">
                <asp:TableCell BorderWidth="2" BorderStyle="Solid" BorderColor="Black">

                    <asp:Label ID="lblFindCOREELMSFile" runat="server" Text="Find CORE ELMS Output File"></asp:Label><br />
                    <asp:Label ID="Label3" runat="server" Text="Last Upload:  "></asp:Label>
                    <asp:Label ID="lblLastCOREELMsDownload" runat="server" Text="Some Date"></asp:Label>
                </asp:TableCell>
                <asp:TableCell BorderWidth="2" BorderStyle="Solid" BorderColor="Black">
                    <asp:FileUpload ID="fuCOREElmsFile" runat="server" Width="500px" />

                    <asp:Label ID="lblCOREELMSFileName" runat="server" Text=" "></asp:Label>
                </asp:TableCell>
                <asp:TableCell BorderWidth="2" BorderStyle="Solid" BorderColor="Black">
                    <asp:Button runat="server" ID="CoreELMSUploadButton" Text="Create SONIS File" OnClick="btnUploadCoreELMSFile_Click" /><br />
                    <asp:Label ID="lblCOREELMSResults" runat="server" Text=""></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <%--      <asp:Table ID="Table2" runat="server" Width="100%" BorderWidth="2" BorderStyle="Solid" BorderColor="Black">
            <asp:TableRow BorderWidth="2" BorderStyle="Solid" BorderColor="Black">
                <asp:TableCell BorderWidth="2" BorderStyle="Solid" BorderColor="Black">
                     <asp:Button runat="server" ID="btnCreateOutputFile" Text="Create Output File" OnClick="btnCreateOutputFiles_Click" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>--%>
    </div>
</asp:Content>