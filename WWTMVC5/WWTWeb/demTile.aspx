<%@ Page Language="C#" ContentType="application/octet-stream" %>

<%@ Import Namespace="WWT.Providers" %>
<%
    RequestProvider.Get<DemTileProvider>().Run(this);
%>