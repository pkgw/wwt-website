<%@ Page Language="C#" ContentType="text/plain" %> 

<%@ Import Namespace="WWT.Providers" %>
<%
	RequestProvider.Get<postmarsProvider>().Run(this);
%>
