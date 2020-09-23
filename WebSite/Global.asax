<%@ Application Language="VB" %>

<script runat="server">
    
    Const TimedOutExceptionCode As Int32 = -2147467259
    
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim context As System.Web.HttpContext = DirectCast(sender, System.Web.HttpApplication).Context
        Dim peso As Int32 = context.Request.ContentLength
        If IsMaxRequestExceededException(Me.Server.GetLastError()) Then
            Me.Server.ClearError()
            'HttpContext.Current.ClearError()
            'Response.Clear()
            'Response.Write("Ha sobrepasado el límite del peso en la carga de archivo, por favor seleccione un archivo válido.&nbsp;&nbsp;<input type='button' Value='Volver' Onclick='history.back();'/>")
            Response.Write("<p>Ha sobrepasado el límite del peso en la carga de archivo</p>")
            'Response.End()
            
            'Response.Redirect("Error.aspx")
            'Response.Redirect("http://localhost:8653/WebSite/CargaArchivo.aspx",false)
            'Server.Transfer("General/CargarArchivo.aspx")
            'Response.Redirect("http://localhost:8653/WebSite/CargaArchivo.aspx",true)
        End If
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
    
    Private Shared Function IsMaxRequestExceededException(ByVal e As Exception) As Boolean
    
        'unhandled errors = caught at global.ascx level
        'http exception = caught at page level
 
        Dim main As Exception
        Dim unhandled As HttpUnhandledException = e
 
        If Not IsNothing(unhandled) And unhandled.ErrorCode = TimedOutExceptionCode Then
            main = unhandled.InnerException
        Else
            main = e
        End If
  
        Dim http As HttpException = main
 
        If Not IsNothing(http) And http.ErrorCode = TimedOutExceptionCode Then
        
            'hack: no real method of identifying if the error is max request exceeded as
            'it is treated as a timeout exception
            If http.StackTrace.Contains("GetEntireRawContent") Then
                ' MAX REQUEST HAS BEEN EXCEEDED
                Return True
            End If
        End If
        Return False
    End Function
</script>