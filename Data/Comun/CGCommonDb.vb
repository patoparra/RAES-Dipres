Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data

' CLASE: Dipres.Data.Comun.CGCommonDbT x
' AUTOR: DARMIN CID (dcid@tutopia.com)
' FECHA: (c) 2005-2006
' BITACORA DE MODIFICACIONES:
'  [FECHA],[AUTOR]: [Lo realizado]
'  21/04/2006, DCID : se agrega metodo ExecuteNonQueryWithReturnValue ...-> PRIMERO EN USARLO -> LeyMIIndicadores.Insertar

Namespace Comun


    Public Class CGCommonDb
        Public Const KEY_CONFIG_DB As String = "StringConexion"

        Private Sub New()
        End Sub

        Public Shared Function CopyDataRowToAnotherEmptyDataRow(ByVal oDtRowToBeCopied As DataRow) As DataRow
            Dim oDtTableOfNewRow As DataTable
            Dim oDtNewRow As DataRow
            Dim i As Integer

            oDtTableOfNewRow = oDtRowToBeCopied.Table.Clone
            oDtNewRow = oDtTableOfNewRow.NewRow()

            For i = 0 To oDtRowToBeCopied.ItemArray.Length - 1
                oDtNewRow(i) = oDtRowToBeCopied(i)
            Next

            Return oDtNewRow
        End Function

        Public Shared Function ExtraeDatosTablaDatasetCVS(ByRef DS As DataSet, ByVal nomTabla As String, ByVal nomColumna As String) As String
            Dim myRow As DataRow
            Dim strDatos As String = ""

            For Each myRow In DS.Tables(nomTabla).Rows
                strDatos = strDatos & myRow(nomColumna) & ","
            Next
            If strDatos <> "" Then
                strDatos = strDatos.Substring(0, strDatos.Length - 1)
            End If
            ExtraeDatosTablaDatasetCVS = strDatos
        End Function

        Public Shared Function ExtraeDatosTablaDatasetCVS(ByRef dtTabla As DataTable, ByVal nomColumna As String) As String
            Dim myRow As DataRow
            Dim strDatos As String = ""

            For Each myRow In dtTabla.Rows
                strDatos = strDatos & myRow(nomColumna) & ","
            Next
            If strDatos <> "" Then
                strDatos = strDatos.Substring(0, strDatos.Length - 1)
            End If
            ExtraeDatosTablaDatasetCVS = strDatos
        End Function

        Public Shared Function ExtraeDatosNoNulosTablaDatasetCVS(ByRef DS As DataSet, ByVal nomTabla As String, ByVal nomColumna As String) As String
            Dim myRow As DataRow
            Dim strDatos As String = ""

            For Each myRow In DS.Tables(nomTabla).Rows
                If Not myRow(nomColumna) Is DBNull.Value Then
                    strDatos = strDatos & myRow(nomColumna) & ","
                End If
            Next
            If strDatos <> "" Then
                strDatos = strDatos.Substring(0, strDatos.Length - 1)
            End If
            Return strDatos
        End Function

        Public Shared Function ExtraeDatosTablaDatasetCVS(ByRef DS As DataSet, ByVal nomTabla As String, ByVal nomColumna As String, ByVal nomColumnaFiltro As String, ByVal valFiltro As String) As String
            Dim myRow As DataRow
            Dim strDatos As String = ""

            For Each myRow In DS.Tables(nomTabla).Rows
                If myRow(nomColumnaFiltro).ToString() = valFiltro Then
                    strDatos = strDatos & myRow(nomColumna) & ","
                End If
            Next
            If strDatos <> "" Then
                strDatos = strDatos.Substring(0, strDatos.Length - 1)
            End If
            ExtraeDatosTablaDatasetCVS = strDatos
        End Function

        Public Shared Function GetConnectionStringByKeyConfig(ByVal sKeyConfig As String, ByVal bIsEncrypt As Boolean) As String

            Dim strConn As String
            Dim oEncrypt As New ClsEncriptar.Encripto

            strConn = ConfigurationManager.AppSettings(sKeyConfig)

            If bIsEncrypt Then strConn = oEncrypt.Desencriptar(strConn)

            Return strConn
        End Function


        Public Shared Function GetConnectionByKeyConfig(ByVal sKeyConfig As String, ByVal bIsEncrypt As Boolean) As SqlConnection

            Return GetConnectionByConnString(ConfigurationManager.AppSettings(sKeyConfig), bIsEncrypt)
        End Function

        Public Shared Function GetConnectionByConnString(ByVal strConn As String, ByVal bIsEncrypt As Boolean) As SqlConnection

            Dim objEncrypt As New ClsEncriptar.Encripto
            'Dim objConn As SqlConnection 
            Dim objConn As SqlConnection = New SqlConnection()

            Try
                If bIsEncrypt Then strConn = objEncrypt.Desencriptar(strConn)

                'objConn = New SqlConnection(strConn)

                objConn.ConnectionString = strConn

                objConn.Open()

                Return objConn
            Catch
                If Not objConn Is Nothing Then objConn.Dispose()
                Throw
            End Try
        End Function

        Public Overloads Shared Function AssignDefaultValuesToRow(ByVal oDtRow As DataRow) As DataRow
            Dim oDtColumn As DataColumn
            'Dim tipo As Type

            For Each oDtColumn In oDtRow.Table.Columns
                If oDtRow(oDtColumn.ColumnName) Is System.DBNull.Value Then
                    Select Case oDtColumn.DataType().Name
                        Case "Int64", "Int32", "Int16", "Decimal", "Byte"
                            oDtRow(oDtColumn.ColumnName) = 0
                        Case "String"
                            oDtRow(oDtColumn.ColumnName) = String.Empty
                    End Select
                End If
                oDtRow(oDtColumn.ColumnName).GetType()

                'If Not oDtColumn.AllowDBNull() Then
                '    oDtRow(oDtColumn.ColumnName) = oDtColumn.DefaultValue()
                'End If
            Next
            Return oDtRow
        End Function


#Region "utiles para mejorar performance usando DataReaden en vez de Dataset"

        Public Shared Function ExecuteSpToDataTable(ByVal strConn As String, ByVal strSpName As String, ByVal ParamArray objParameterValues() As Object) As DataTable

            Dim oDataReader As SqlDataReader = Nothing
            Dim oDataTable As DataTable = Nothing

            Try

                oDataReader = SqlHelper.ExecuteReader(strConn, strSpName, objParameterValues)

                '' iterate through SqlDataReader
                'While oDataReader.Read()
                '    ' get the value of second column in the datareader (product description)
                '    'txtResults.Text = txtResults.Text + dr.GetValue(1) + Environment.NewLine
                'End While

                'Return oDataTable

                Return DataReaderToDataSet(oDataReader)

            Catch ex As Exception
                Throw ex
            Finally
                If Not oDataReader Is Nothing Then
                    CType(oDataReader, IDisposable).Dispose()
                    oDataReader.Close()
                    oDataReader = Nothing
                End If
            End Try

        End Function

        Public Shared Function ExecuteQryToDataTable(ByVal strConn As String, ByVal strSqlQry As String) As DataTable

            Dim oDataReader As SqlDataReader = Nothing
            Dim oDataTable As DataTable = Nothing

            Try

                oDataReader = SqlHelper.ExecuteReader(strConn, CommandType.Text, strSqlQry)

                '' iterate through SqlDataReader
                'While oDataReader.Read()
                '    ' get the value of second column in the datareader (product description)
                '    'txtResults.Text = txtResults.Text + dr.GetValue(1) + Environment.NewLine
                'End While

                'Return oDataTable

                Return DataReaderToDataSet(oDataReader)

            Catch ex As Exception
                Throw ex
            Finally
                If Not oDataReader Is Nothing Then
                    CType(oDataReader, IDisposable).Dispose()
                End If
            End Try

        End Function

        'Since IDataReader is also IDataRecord, it could be: 
        'static DataSet DataReaderToDataSet(IDataReader reader) 
        Private Shared Function DataReaderToDataSet(ByVal reader As IDataReader) As DataTable

            Dim table As DataTable = New DataTable
            Dim fieldCount As Integer = reader.FieldCount
            Dim i As Integer
            For i = 0 To (fieldCount - 1)
                table.Columns.Add(reader.GetName(i), reader.GetFieldType(i))
            Next
            table.BeginLoadData()

            Dim values() As Object = New Object(fieldCount - 1) {} ' -1

            While (reader.Read())
                reader.GetValues(values)
                table.LoadDataRow(values, True)
            End While

            table.EndLoadData()

            'Dim ds As DataSet = New DataSet()
            'ds.Tables.Add(table)
            Return table
        End Function

#End Region

#Region "Puente para que bussines use el SQLHelper DataAppBlock"

        '*** ExecuteNonQuery *** :: metodos ExecuteNonQuery: solo dos definiciones las otras estan prohibidas de usar en busssines
        Public Shared Function ExecuteNonQuery(ByVal connectionString As String, ByVal commandType As System.Data.CommandType, ByVal spName As String, ByVal ParamArray commandParameters() As SqlParameter) As Integer
            ' Retorna registros Insert/Update o Estados de error -2: Duplicado (Constrain violated al insertar)

            Dim iRowAffected As Integer

            Try
                iRowAffected = SqlHelper.ExecuteNonQuery(connectionString, commandType, spName, commandParameters)

                If iRowAffected = 0 Then
                    iRowAffected = -1 ' registro no existe
                End If

            Catch ex As SqlException

                If ex.Number = 2627 Then
                    iRowAffected = -2  ' registro duplicado 
                Else
                    Throw ex
                End If

            Catch ex As Exception
                Throw ex
            End Try
            Return iRowAffected

        End Function

        '*** ExecuteNonQuery *** :: metodos ExecuteNonQuery: solo dos definiciones las otras estan prohibidas de usar en busssines
        Public Shared Function ExecuteNonQuery(ByVal connectionString As String, ByVal spName As String, ByVal ParamArray parameterValues As Object()) As Integer
            ' Retorna registros Insert/Update o Estados de error -2: Duplicado (Constrain violated al insertar)

            Dim iRowAffected As Integer

            Try
                iRowAffected = SqlHelper.ExecuteNonQuery(connectionString, spName, parameterValues)

                If iRowAffected = 0 Then
                    iRowAffected = -1 ' registro no existe
                End If

            Catch ex As SqlException

                If ex.Number = 2627 Then
                    iRowAffected = -2  ' registro duplicado 
                Else
                    Throw ex
                End If

            Catch ex As Exception
                Throw ex
            End Try
            Return iRowAffected

        End Function

        Public Shared Function ExecuteNonQuery(ByVal connectionString As String, ByVal commandType As System.Data.CommandType, ByVal commandText As String) As Integer
            ' Retorna registros Insert/Update o Estados de error -2: Duplicado (Constrain violated al insertar)

            Dim iRowAffected As Integer

            Try
                iRowAffected = SqlHelper.ExecuteNonQuery(connectionString, commandType, commandText)

            Catch ex As SqlException

                If ex.Number = 2627 Then
                    iRowAffected = -2     ' registro duplicado 
                Else
                    Throw ex
                End If

            Catch ex As Exception
                Throw ex
            End Try
            Return iRowAffected
        End Function

        ' Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
        ' the connection string using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure' s return value parameter.
        ' e.g.:  
        ' Dim orderCount As Integer = CInt(ExecuteScalar(connString, "GetOrderCount", 24, 36))
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection 
        ' -spName - the name of the stored procedure 
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        ' Returns: An object containing the value in the 1x1 resultset generated by the command 
        Public Overloads Shared Function ExecuteScalar(ByVal connectionString As String, _
                                                       ByVal spName As String, _
                                                       ByVal ParamArray parameterValues() As Object) As Object
            Return SqlHelper.ExecuteScalar(connectionString, spName, parameterValues)
        End Function ' ExecuteScalar

        ' Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
        ' the connection string. 
        ' e.g.:  
        ' Dim orderCount As Integer = CInt(ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount"))
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection 
        ' -commandType - the CommandType (stored procedure, text, etc.) 
        ' -commandText - the stored procedure name or T-SQL command 
        ' Returns: An object containing the value in the 1x1 resultset generated by the command
        Public Overloads Shared Function ExecuteScalar(ByVal connectionString As String, _
                                                       ByVal commandType As CommandType, _
                                                       ByVal commandText As String) As Object
            Return SqlHelper.ExecuteScalar(connectionString, commandType, commandText)
        End Function ' ExecuteScalar

        'Public Shared Function ExecuteNonQueryWithOutputParams(ByVal connectionString As String, ByVal spName As String, ByVal ParamArray parameterValues As Object()) As Integer

        '    ' DCID SAY ... COMMING SOON!

        '    Dim commandParameters As SqlParameter()
        '    ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
        '    commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName)

        '    ' Assign the provided values to these parameters based on parameter order
        '    AssignParameterValues(commandParameters, parameterValues)

        '    Return -1

        'End Function


        ' Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in 
        ' the connection string using the dataRow column values as the stored procedure' s parameters values.
        ' This method will query the database to discover the parameters for the 
        ' stored procedure (the first time each stored procedure is called), and assign the values based on row values.
        ' Parameters:
        ' -connectionString: A valid connection string for a SqlConnection
        ' -spName: the name of the stored procedure
        ' -dataRow: The dataRow used to hold the stored procedure' s parameter values
        ' Returns:
        ' an int representing the Return value or an negative int for error code
        Public Overloads Shared Function ExecuteNonQueryWithReturnValue(ByVal connectionString As String, ByVal spName As String, ByRef dataRow As DataRow) As Integer

            If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
            If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

            Dim iRowAffected As Integer
            Dim iErrCode As Integer
            Dim commandParameters As SqlParameter()

            ' If the row has values, the store procedure parameters must be initialized
            If (Not dataRow Is Nothing) Then  ' AndAlso dataRow.ItemArray.Length > 0) Then

                ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName, True)


                ' Set the parameters values
                AssignParameterValues(commandParameters, dataRow)

                iRowAffected = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters)

                ' iErrCode  = 0: Ok , -1: No encontrado
                If iRowAffected <> 1 Then
                    iErrCode = -1
                Else
                    iErrCode = Integer.Parse(commandParameters(0).Value)
                End If

            End If

            Return iErrCode

        End Function

        ' Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in 
        ' the connection string using the provided parameter values.  This method will discover the parameters for the 
        ' stored procedure, and assign the values based on parameter order.
        ' This method provides no access to output parameters or the stored procedure' s return value parameter.
        ' e.g.:  
        '  Dim result As Integer = ExecuteNonQuery(connString, "PublishOrders", 24, 36)
        ' Parameters:
        ' -connectionString - a valid connection string for a SqlConnection
        ' -spName - the name of the stored procedure
        ' -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        ' Returns:
        ' an int representing the Return value or an negative int for error code
        Public Overloads Shared Function ExecuteNonQueryWithReturnValue(ByVal connectionString As String, _
                                                     ByVal spName As String, _
                                                     ByVal ParamArray parameterValues() As Object) As Integer

            If (connectionString Is Nothing OrElse connectionString.Length = 0) Then Throw New ArgumentNullException("connectionString")
            If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

            Dim iRowAffected As Integer
            Dim iErrCode As Integer
            Dim commandParameters As SqlParameter()

            ' If we receive parameter values, we need to figure out where they go
            If Not (parameterValues Is Nothing) AndAlso parameterValues.Length > 0 Then
                Try

                    ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                    commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName, True)

                    ' Assign the provided values to these parameters based on parameter order
                    AssignParameterValues(commandParameters, parameterValues)

                    ' Call the overload that takes an array of SqlParameters
                    iRowAffected = ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters)

                    ' iErrCode  = 0: Ok , -1: No encontrado, -2: duplicado
                    If iRowAffected = 0 Then
                        iErrCode = -1 ' no encontrado
                    ElseIf iRowAffected > 1 Then
                        iErrCode = -2 'duplicado pero sin CONSTRAIN en la tabla
                    Else
                        iErrCode = Integer.Parse(commandParameters(0).Value) ' el return value
                    End If

                Catch ex As SqlException

                    If ex.Number = 2627 Then
                        iErrCode = -2     ' registro duplicado
                    Else
                        Throw ex
                    End If

                Catch ex As Exception
                    Throw ex
                End Try
            End If

            Return iErrCode

        End Function ' ExecuteNonQuery


#End Region

        ' This method assigns dataRow column values to an array of SqlParameters.
        ' Parameters:
        ' -commandParameters: Array of SqlParameters to be assigned values
        ' -dataRow: the dataRow used to hold the stored procedure' s parameter values
        Public Overloads Shared Sub AssignParameterValues(ByVal commandParameters() As SqlParameter, ByVal dataRow As DataRow)

            If commandParameters Is Nothing OrElse dataRow Is Nothing Then
                ' Do nothing if we get no data    
                Exit Sub
            End If

            ' Set the parameters values
            Dim commandParameter As SqlParameter
            Dim i As Integer

            For Each commandParameter In commandParameters
                ' Check the parameter name
                If (commandParameter.ParameterName Is Nothing OrElse commandParameter.ParameterName.Length <= 1) Then
                    Throw New Exception(String.Format("Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: ' {1}' .", i, commandParameter.ParameterName))
                End If
                If dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) <> -1 Then
                    commandParameter.Value = dataRow(commandParameter.ParameterName.Substring(1))
                End If
                i = i + 1
            Next
        End Sub

        ' This method assigns an array of values to an array of SqlParameters.
        ' Parameters:
        ' -commandParameters - array of SqlParameters to be assigned values
        ' -array of objects holding the values to be assigned
        Public Overloads Shared Sub AssignParameterValues(ByVal commandParameters() As SqlParameter, ByVal parameterValues() As Object)

            Dim i As Integer
            Dim j As Integer

            If (commandParameters Is Nothing) AndAlso (parameterValues Is Nothing) Then
                ' Do nothing if we get no data
                Return
            End If

            ' We must have the same number of values as we pave parameters to put them in
            If commandParameters(0).Direction = ParameterDirection.ReturnValue Then
                If commandParameters.Length - 1 <> parameterValues.Length Then
                    Throw New ArgumentException("Parameter count does not match Parameter Value count.")
                End If

                ' Value array
                j = parameterValues.Length - 1
                For i = 0 To j
                    ' If the current array value derives from IDbDataParameter, then assign its Value property
                    If TypeOf parameterValues(i) Is IDbDataParameter Then
                        Dim paramInstance As IDbDataParameter = CType(parameterValues(i), IDbDataParameter)
                        If (paramInstance.Value Is Nothing) Then
                            commandParameters(i + 1).Value = DBNull.Value
                        Else
                            commandParameters(i + 1).Value = paramInstance.Value
                        End If
                    ElseIf (parameterValues(i) Is Nothing) Then
                        commandParameters(i + 1).Value = DBNull.Value
                    Else
                        commandParameters(i + 1).Value = parameterValues(i)
                    End If
                Next
            Else
                If commandParameters.Length <> parameterValues.Length Then
                    Throw New ArgumentException("Parameter count does not match Parameter Value count.")
                End If

                ' Value array
                j = commandParameters.Length - 1
                For i = 0 To j
                    ' If the current array value derives from IDbDataParameter, then assign its Value property
                    If TypeOf parameterValues(i) Is IDbDataParameter Then
                        Dim paramInstance As IDbDataParameter = CType(parameterValues(i), IDbDataParameter)
                        If (paramInstance.Value Is Nothing) Then
                            commandParameters(i).Value = DBNull.Value
                        Else
                            commandParameters(i).Value = paramInstance.Value
                        End If
                    ElseIf (parameterValues(i) Is Nothing) Then
                        commandParameters(i).Value = DBNull.Value
                    Else
                        commandParameters(i).Value = parameterValues(i)
                    End If
                Next
            End If

        End Sub ' AssignParameterValues
    End Class
End Namespace