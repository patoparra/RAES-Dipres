Imports System.Configuration
Imports System.Web.Configuration

Namespace Configuracion
    Public Enum FormatoArchivo
        NoDefinido
        Word
        Excel
        Pdf
    End Enum

    Public NotInheritable Class Configuracion
        Inherits ConfigurationSection



        Private Shared instancia As Configuracion
        Private Shared _Propiedades As ConfigurationPropertyCollection
        Private Shared _ReadOnly As Boolean
        Private Shared _LockConfig As New Object

        '(Propiedades).
        Private Shared ReadOnly _RutaReporte As ConfigurationProperty = New ConfigurationProperty("RutaReporte", GetType(String), Nothing, ConfigurationPropertyOptions.IsRequired)
        Private Shared ReadOnly _ProxyUser As ConfigurationProperty = New ConfigurationProperty("ProxyUser", GetType(String), Nothing, ConfigurationPropertyOptions.IsRequired)
        Private Shared ReadOnly _ProxyPassword As ConfigurationProperty = New ConfigurationProperty("ProxyPassword", GetType(String), Nothing, ConfigurationPropertyOptions.IsRequired)
        Private Shared ReadOnly _ProxyDomain As ConfigurationProperty = New ConfigurationProperty("ProxyDomain", GetType(String), Nothing, ConfigurationPropertyOptions.IsRequired)

        Private Shared ReadOnly _UrlwsReportExecution2005 As ConfigurationProperty = New ConfigurationProperty("UrlwsReportExecution2005", GetType(String), Nothing, ConfigurationPropertyOptions.IsRequired)
        Private Shared ReadOnly _UrlwsControlAcceso As ConfigurationProperty = New ConfigurationProperty("UrlwsControlAcceso", GetType(String), Nothing, ConfigurationPropertyOptions.IsRequired)
        Private Shared ReadOnly _UrlwsSendMail As ConfigurationProperty = New ConfigurationProperty("UrlwsSendMail", GetType(String), Nothing, ConfigurationPropertyOptions.IsRequired)
        Private Shared ReadOnly _UrlwsNormalizaArchivo As ConfigurationProperty = New ConfigurationProperty("UrlwsNormalizaArchivo", GetType(String), Nothing, ConfigurationPropertyOptions.IsRequired)

        Public Sub New()
            MyBase.New()
            _Propiedades = New ConfigurationPropertyCollection()
            _Propiedades.Add(_RutaReporte)
            _Propiedades.Add(_ProxyUser)
            _Propiedades.Add(_ProxyPassword)
            _Propiedades.Add(_ProxyDomain)
            _Propiedades.Add(_UrlwsReportExecution2005)
            _Propiedades.Add(_UrlwsControlAcceso)
            _Propiedades.Add(_UrlwsSendMail)
            _Propiedades.Add(_UrlwsNormalizaArchivo)
        End Sub

        Public Shared Function getInstance() As Configuracion
            Try

                If instancia Is Nothing Then
                    SyncLock (_LockConfig)
                        instancia = New Configuracion()
                        Dim configPath As String = "~/Configuracion"
                        Dim config As Configuration = WebConfigurationManager.OpenWebConfiguration(configPath)
                        instancia = CType(config.GetSection("Configuracion"), Configuracion)
                    End SyncLock

                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return instancia
        End Function

        ' This is a key customization. 
        ' It returns the initialized property bag.
        Protected Overrides ReadOnly Property Properties() As ConfigurationPropertyCollection
            Get
                Return _Propiedades
            End Get
        End Property

        ' A member of a derived class has the same name as a member of the same type in the base class.
        ' The member in the derived class entirely replaces all variations of the method from the base class.
        Private Shadows ReadOnly Property IsReadOnly() As Boolean
            Get
                Return _ReadOnly
            End Get
        End Property

        ' Use this to disable property setting.
        Private Sub ThrowIfReadOnly(ByVal propertyName As String)
            If (IsReadOnly) Then
                Throw New ConfigurationErrorsException("La propiedad " & propertyName & " es de solo lectura.")
            End If
        End Sub

        ' Customizes the use of CustomSection by setting _ReadOnly to false.
        ' Remember you must use it along with ThrowIfReadOnly.
        Protected Overrides Function GetRuntimeObject() As Object
            ' To enable property setting just assign true to the following flag.
            _ReadOnly = True
            Return MyBase.GetRuntimeObject()
        End Function
        

        <StringValidator(InvalidCharacters:="~!@#$%^&*()[]{};'\|\\", MinLength:=1, MaxLength:=200)> _
        Public Property ProxyUser() As String
            Get
                Return CStr(Me("ProxyUser"))
            End Get

            Set(ByVal value As String)
                ThrowIfReadOnly("ProxyUser")
                Me("ProxyUser") = value
            End Set
        End Property

        <StringValidator(InvalidCharacters:="~!@#$%^&*()[]{};'\|\\", MinLength:=1, MaxLength:=200)> _
        Public Property ProxyPassword() As String
            Get
                Return CStr(Me("ProxyPassword"))
            End Get

            Set(ByVal value As String)
                ThrowIfReadOnly("ProxyPassword")
                Me("ProxyPassword") = value
            End Set
        End Property

        <StringValidator(InvalidCharacters:="~!@#$%^&*()[]{};'\|\\", MinLength:=1, MaxLength:=200)> _
        Public Property ProxyDomain() As String
            Get
                Return CStr(Me("ProxyDomain"))
            End Get

            Set(ByVal value As String)
                ThrowIfReadOnly("ProxyDomain")
                Me("ProxyDomain") = value
            End Set
        End Property

        <StringValidator(InvalidCharacters:="~!@#$%^&*()[]{};'\|\\", MinLength:=1, MaxLength:=200)> _
        Public Property UrlwsReportExecution2005() As String
            Get
                Return CStr(Me("UrlwsReportExecution2005"))
            End Get

            Set(ByVal value As String)
                ThrowIfReadOnly("UrlwsReportExecution2005")
                Me("UrlwsReportExecution2005") = value
            End Set
        End Property

        <StringValidator(InvalidCharacters:="~!@#$%^&*()[]{};'\|\\", MinLength:=1, MaxLength:=200)> _
        Public Property UrlwsSendMail() As String
            Get
                Return CStr(Me("UrlwsSendMail"))
            End Get

            Set(ByVal value As String)
                ThrowIfReadOnly("UrlwsSendMail")
                Me("UrlwsSendMail") = value
            End Set
        End Property

        <StringValidator(InvalidCharacters:="~!@#$%^&*()[]{};'\|\\", MinLength:=1, MaxLength:=200)> _
        Public Property UrlwsNormalizaArchivo() As String
            Get
                Return CStr(Me("UrlwsNormalizaArchivo"))
            End Get

            Set(ByVal value As String)
                ThrowIfReadOnly("UrlwsNormalizaArchivo")
                Me("UrlwsNormalizaArchivo") = value
            End Set
        End Property

        <StringValidator(InvalidCharacters:="~!@#$%^&*()[]{};'\|\\", MinLength:=1, MaxLength:=200)> _
        Public Property UrlwsControlAcceso() As String
            Get
                Return CStr(Me("UrlwsControlAcceso"))
            End Get

            Set(ByVal value As String)
                ThrowIfReadOnly("UrlwsControlAcceso")
                Me("UrlwsControlAcceso") = value
            End Set
        End Property


        <StringValidator(InvalidCharacters:="~!@#$%^&*()[]{};'\|\\", MinLength:=1, MaxLength:=200)> _
           Public Property RutaReporte() As String
            Get
                Return CStr(Me("RutaReporte"))
            End Get

            Set(ByVal value As String)
                ThrowIfReadOnly("RutaReporte")
                Me("RutaReporte") = value
            End Set
        End Property

    End Class
End Namespace

