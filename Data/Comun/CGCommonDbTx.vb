Imports Microsoft.ApplicationBlocks.data
Imports System.Data
Imports System.Data.SqlClient

' CLASE: Dipres.Data.Comun.CGCommonDbTx
' AUTOR: DARMIN CID (dcid@tutopia.com)
' FECHA: (c) 2005-2006
' BITACORA DE MODIFICACIONES:
'  [FECHA],[AUTOR]: [Lo realizado]
'  19/04/2006, DCID : se agrega metodo sobrecargado : ExecuteNonQueryTx(ByVal objSqlTrans As SqlTransaction, ...-> PRIMERO EN USARLO -> DATA.FLM

Namespace Comun

    Public Class CGCommonDbTx

        Private Sub New()
        End Sub

        Public Shared Function ExecuteNonQueryTx(ByVal spName As String, ByVal oTablaElementos As DataTable) As Integer
            ' retorna estados de error
            ' 0: Ok , -1: Algun registro no encontrado

            Dim objSqlConn As SqlConnection = Nothing
            Dim objSqlTrans As SqlTransaction = Nothing

            Dim iRowAffected As Integer
            Dim intErrCode As Integer
            Dim oDataRow As DataRow

            Try

                objSqlConn = CGCommonDb.GetConnectionByKeyConfig(CGCommonDb.KEY_CONFIG_DB, True)

                objSqlTrans = objSqlConn.BeginTransaction()

                Try

                    For Each oDataRow In oTablaElementos.Rows

                        iRowAffected = SqlHelper.ExecuteNonQuery(objSqlTrans, spName, oDataRow.ItemArray)

                        If iRowAffected <> 1 Then
                            intErrCode = -1
                            Exit For
                        End If
                    Next

                    If (intErrCode = 0) Then
                        objSqlTrans.Commit()
                    Else
                        objSqlTrans.Rollback()
                    End If

                Catch ex As Exception
                    'objSqlTrans.Rollback()
                    Throw ex
                End Try

            Catch ex As Exception

                Throw ex
            Finally

                If Not objSqlTrans Is Nothing Then objSqlTrans.Dispose()
                If Not objSqlConn Is Nothing Then objSqlConn.Dispose()
            End Try

            Return intErrCode

        End Function

        Public Shared Function ExecuteNonQueryTxForMoreRowAffected(ByVal spName As String, ByVal oTablaElementos As DataTable) As Integer
            ' UTIL CUANDO EL SP AFECTA A MAS DE UN REGISTRO

            Dim objSqlConn As SqlConnection = Nothing
            Dim objSqlTrans As SqlTransaction = Nothing


            Dim iRowAffected As Integer
            Dim intErrCode As Integer = 0
            Dim oDataRow As DataRow

            Try

                objSqlConn = CGCommonDb.GetConnectionByKeyConfig(CGCommonDb.KEY_CONFIG_DB, True)

                objSqlTrans = objSqlConn.BeginTransaction()

                Try
                    For Each oDataRow In oTablaElementos.Rows

                        iRowAffected = SqlHelper.ExecuteNonQuery(objSqlTrans, spName, oDataRow.ItemArray)





                    Next

                    objSqlTrans.Commit()
                Catch ex As Exception ' AQUI OCURRE UN ROLLBACK
                    Throw ex
                End Try

            Catch ex As Exception
                Throw ex
            Finally

                If Not objSqlTrans Is Nothing Then objSqlTrans.Dispose()
                If Not objSqlConn Is Nothing Then objSqlConn.Dispose()





            End Try

            Return intErrCode

        End Function

        Public Shared Function ExecuteNonQueryTxForMoreRowAffected(ByVal objSqlTrans As SqlTransaction, ByVal spName As String, ByVal oTablaElementos As DataTable) As Integer
            ' UTIL CUANDO EL SP AFECTA A MAS DE UN REGISTRO


            Dim iRowAffected As Integer
            Dim intErrCode As Integer = 0





            Dim oDataRow As DataRow


            Try
                For Each oDataRow In oTablaElementos.Rows
                    iRowAffected = SqlHelper.ExecuteNonQuery(objSqlTrans, spName, oDataRow.ItemArray)








                Next
            Catch ex As Exception ' AQUI OCURRE UN ROLLBACK
                Throw ex
            End Try



            Return intErrCode

        End Function

        Public Shared Function ExecuteNonQueryTx(ByVal objSqlTrans As SqlTransaction, ByVal spName As String, ByVal oTablaElementos As DataTable) As Integer

            ' IMPORTANTE:  ESTE METODO NO CIERRA LA TRANSACCION NI HACE COMMITS .. SOLO ROLLBACKS.. CUIDADO AL USARLO !!!
            ' PRIMERO EN USARLO -> DATA.FLM (atraves de entity.LeyMDFormulacion)
            ' retorna estados de error
            ' 0: Ok , -1: Algun registro no encontrado, -2: Registro Duplicado (error de constrain al insertar nuevos)

            Dim iRowAffected As Integer
            Dim intErrCode As Integer
            Dim oDataRow As DataRow

            Try

                For Each oDataRow In oTablaElementos.Rows

                    iRowAffected = SqlHelper.ExecuteNonQuery(objSqlTrans, spName, oDataRow.ItemArray)

                    If iRowAffected <> 1 Then
                        intErrCode = -1
                        Exit For
                    End If
                Next

                'If (intErrCode <> 0) Then
                '    objSqlTrans.Rollback()
                'End If

            Catch ex As SqlException

                'objSqlTrans.Rollback()

                If ex.Number = 2627 Then
                    intErrCode = -2     ' registro duplicado
                Else
                    Throw ex
                End If
            Catch ex As Exception
                'objSqlTrans.Rollback()
                Throw ex
            End Try

            Return intErrCode

        End Function

        ' DEPRECATED - DCID 05/05/2006
        Public Shared Function DeleteAndInsertTx(ByVal spNameDelete As String, ByVal oDeleteParamsArray() As Object, ByVal spNameInsert As String, ByVal oTablaElementosAInsertar As DataTable) As Integer
            ' retorna estados de error
            ' 0: Ok , -1: Algun registro no encontrado (al borrar viejos) , -2: Registro Duplicado (al insertar nuevos)

            Dim objSqlConn As SqlConnection = Nothing
            Dim objSqlTrans As SqlTransaction = Nothing

            Dim iRowAffected As Integer = 0
            Dim iErrorCode As Integer = 0

            Dim oDataRow As DataRow

            Try

                objSqlConn = CGCommonDb.GetConnectionByKeyConfig(CGCommonDb.KEY_CONFIG_DB, True)
                objSqlTrans = objSqlConn.BeginTransaction()

                Try
                    ' Eliminamos todos elementos dado un PK
                    iRowAffected = SqlHelper.ExecuteNonQuery(objSqlTrans, spNameDelete, oDeleteParamsArray)

                    ' Insertamos
                    For Each oDataRow In oTablaElementosAInsertar.Rows
                        iRowAffected = SqlHelper.ExecuteNonQuery(objSqlTrans, spNameInsert, oDataRow.ItemArray)
                    Next

                    objSqlTrans.Commit()


                Catch ex As SqlException

                    'objSqlTrans.Rollback()

                    If ex.Number = 2627 Then
                        iErrorCode = -2     ' registro duplicado
                    Else
                        Throw ex
                    End If

                Catch ex As Exception

                    'objSqlTrans.Rollback()
                    Throw ex
                End Try

            Catch ex As Exception

                Throw ex
            Finally

                If Not objSqlTrans Is Nothing Then objSqlTrans.Dispose()
                If Not objSqlConn Is Nothing Then objSqlConn.Dispose()
            End Try

            Return iErrorCode

        End Function

        Public Shared Function DeleteAndInsertTx(ByVal spNameDelete As String, ByVal oTablaElementosABorrar As DataTable, ByVal spNameInsert As String, ByVal oTablaElementosAInsertar As DataTable) As Integer
            ' retorna estados de error
            ' 0: Ok , -1: Algun registro no encontrado (al borrar viejos) , -2: Registro Duplicado (al insertar nuevos)

            'Dim objSqlConn As SqlConnection = Nothing
            'Dim objSqlTrans As SqlTransaction = Nothing

            'Dim iErrorCode As Integer = 0

            'Try

            '    objSqlConn = CGCommonDb.GetConnectionByKeyConfig(CGCommonDb.KEY_CONFIG_DB, True)
            '    objSqlTrans = objSqlConn.BeginTransaction()

            '    ' Eliminamos todos elementos dado un PK
            '    ' 0: Ok , -1: Algun registro no encontrado, -2: Registro Duplicado (error de constrain al insertar nuevos)
            '    iErrorCode = ExecuteNonQueryTx(objSqlTrans, spNameDelete, oTablaElementosABorrar)

            '    If iErrorCode < 0 Then
            '        objSqlTrans.Rollback()
            '    Else
            '        ' Insertamos nuevos elementos
            '        ' 0: Ok , -1: Algun registro no encontrado, -2: Registro Duplicado (error de constrain al insertar nuevos)
            '        iErrorCode = ExecuteNonQueryTx(objSqlTrans, spNameInsert, oTablaElementosAInsertar)

            '        If iErrorCode < 0 Then
            '            objSqlTrans.Rollback()
            '        Else
            '            objSqlTrans.Commit() ' <-- COMMIT SOLO AQUI
            '        End If
            '    End If

            'Catch ex As Exception

            '    Throw ex
            'Finally

            '    If Not objSqlTrans Is Nothing Then objSqlTrans.Dispose()
            '    If Not objSqlConn Is Nothing Then objSqlConn.Dispose()
            'End Try

            'Return iErrorCode
            Return ExecTwoSpsInTxMode(spNameDelete, oTablaElementosABorrar, spNameInsert, oTablaElementosAInsertar)
        End Function

        Public Shared Function DeleteAndInsertTx(ByVal objSqlTrans As SqlTransaction, ByVal spNameDelete As String, ByVal oTablaElementosABorrar As DataTable, ByVal spNameInsert As String, ByVal oTablaElementosAInsertar As DataTable) As Integer
            Return ExecTwoSpsInTxMode(objSqlTrans, spNameDelete, oTablaElementosABorrar, spNameInsert, oTablaElementosAInsertar)
        End Function

        Public Shared Function DeleteAndInsertTxForZeroOrMoreRowsAffected(ByVal objSqlTrans As SqlTransaction, ByVal spNameDelete As String, ByVal oTablaElementosABorrar As DataTable, ByVal spNameInsert As String, ByVal oTablaElementosAInsertar As DataTable) As Integer
            Return ExecTwoSpsInTxModeForZeroOrMoreRowsAffected(objSqlTrans, spNameDelete, oTablaElementosABorrar, spNameInsert, oTablaElementosAInsertar)
        End Function

        Public Shared Function DeleteAndInsertTxForMoreRowAffected(ByVal spNameDelete As String, ByVal oTablaElementosABorrar As DataTable, ByVal spNameInsert As String, ByVal oTablaElementosAInsertar As DataTable) As Integer
            Return ExecTwoSpsInTxModeForMoreRowAffected(spNameDelete, oTablaElementosABorrar, spNameInsert, oTablaElementosAInsertar)
        End Function

        Public Shared Function UpdateAndInsertTx(ByVal spNameUpdate As String, ByVal oTablaElementosAActualizar As DataTable, ByVal spNameInsert As String, ByVal oTablaElementosAInsertar As DataTable) As Integer
            Return ExecTwoSpsInTxMode(spNameUpdate, oTablaElementosAActualizar, spNameInsert, oTablaElementosAInsertar)
        End Function

        Public Shared Function UpdateAndInsertTxForMoreRowAffected(ByVal spNameUpdate As String, ByVal oTablaElementosAActualizar As DataTable, ByVal spNameInsert As String, ByVal oTablaElementosAInsertar As DataTable) As Integer
            Return ExecTwoSpsInTxModeForMoreRowAffected(spNameUpdate, oTablaElementosAActualizar, spNameInsert, oTablaElementosAInsertar)
        End Function

        Private Shared Function ExecTwoSpsInTxModeForMoreRowAffected(ByVal spName1 As String, ByVal oDtTbElemsToSp1 As DataTable, ByVal spName2 As String, ByVal oDtTbElemsToSp2 As DataTable) As Integer

            ' retorna estados de error
            ' 0: Ok , -1: Algun registro no encontrado (al borrar viejos) , -2: Registro Duplicado (al insertar nuevos)

            Dim objSqlConn As SqlConnection = Nothing
            Dim objSqlTrans As SqlTransaction = Nothing

            Dim iErrorCode As Integer = 0

            Try

                objSqlConn = CGCommonDb.GetConnectionByKeyConfig(CGCommonDb.KEY_CONFIG_DB, True)
                objSqlTrans = objSqlConn.BeginTransaction()

                ' Eliminamos todos elementos dado un PK
                ' 0: Ok , -1: Algun registro no encontrado, -2: Registro Duplicado (error de constrain al insertar nuevos)
                iErrorCode = ExecuteNonQueryTxForMoreRowAffected(objSqlTrans, spName1, oDtTbElemsToSp1)

                If iErrorCode < 0 Then
                    objSqlTrans.Rollback()
                Else
                    ' Insertamos nuevos elementos
                    ' 0: Ok , -1: Algun registro no encontrado, -2: Registro Duplicado (error de constrain al insertar nuevos)
                    iErrorCode = ExecuteNonQueryTxForMoreRowAffected(objSqlTrans, spName2, oDtTbElemsToSp2)

                    If iErrorCode < 0 Then
                        objSqlTrans.Rollback()
                    Else
                        objSqlTrans.Commit() ' <-- COMMIT SOLO AQUI
                    End If
                End If

            Catch ex As Exception

                Throw ex
            Finally

                If Not objSqlTrans Is Nothing Then objSqlTrans.Dispose()
                If Not objSqlConn Is Nothing Then objSqlConn.Dispose()
            End Try

            Return iErrorCode

        End Function

        Private Shared Function ExecTwoSpsInTxMode(ByVal spName1 As String, ByVal oDtTbElemsToSp1 As DataTable, ByVal spName2 As String, ByVal oDtTbElemsToSp2 As DataTable) As Integer
            ' retorna estados de error
            ' 0: Ok , -1: Algun registro no encontrado (al borrar viejos) , -2: Registro Duplicado (al insertar nuevos)

            Dim objSqlConn As SqlConnection = Nothing
            Dim objSqlTrans As SqlTransaction = Nothing

            Dim iErrorCode As Integer = 0

            Try

                objSqlConn = CGCommonDb.GetConnectionByKeyConfig(CGCommonDb.KEY_CONFIG_DB, True)
                objSqlTrans = objSqlConn.BeginTransaction()

                ' Eliminamos todos elementos dado un PK
                ' 0: Ok , -1: Algun registro no encontrado, -2: Registro Duplicado (error de constrain al insertar nuevos)
                iErrorCode = ExecuteNonQueryTx(objSqlTrans, spName1, oDtTbElemsToSp1)

                If iErrorCode < 0 Then
                    objSqlTrans.Rollback()
                Else
                    ' Insertamos nuevos elementos
                    ' 0: Ok , -1: Algun registro no encontrado, -2: Registro Duplicado (error de constrain al insertar nuevos)
                    iErrorCode = ExecuteNonQueryTx(objSqlTrans, spName2, oDtTbElemsToSp2)

                    If iErrorCode < 0 Then
                        objSqlTrans.Rollback()
                    Else
                        objSqlTrans.Commit() ' <-- COMMIT SOLO AQUI
                    End If
                End If

            Catch ex As Exception

                Throw ex
            Finally

                If Not objSqlTrans Is Nothing Then objSqlTrans.Dispose()
                If Not objSqlConn Is Nothing Then objSqlConn.Dispose()
            End Try

            Return iErrorCode

        End Function

        Private Shared Function ExecTwoSpsInTxMode(ByVal objSqlTrans As SqlTransaction, ByVal spName1 As String, ByVal oDtTbElemsToSp1 As DataTable, ByVal spName2 As String, ByVal oDtTbElemsToSp2 As DataTable) As Integer
            Dim iErrorCode As Integer = 0

            Try
                iErrorCode = ExecuteNonQueryTx(objSqlTrans, spName1, oDtTbElemsToSp1)

                If iErrorCode < 0 Then
                    objSqlTrans.Rollback()
                Else

                    iErrorCode = ExecuteNonQueryTx(objSqlTrans, spName2, oDtTbElemsToSp2)

                    If iErrorCode < 0 Then
                        objSqlTrans.Rollback()
                    Else
                        objSqlTrans.Commit()
                    End If
                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return iErrorCode

        End Function

        Private Shared Function ExecTwoSpsInTxModeForZeroOrMoreRowsAffected(ByVal objSqlTrans As SqlTransaction, ByVal spName1 As String, ByVal oDtTbElemsToSp1 As DataTable, ByVal spName2 As String, ByVal oDtTbElemsToSp2 As DataTable) As Integer
            Dim iErrorCode As Integer = 0

            Try
                iErrorCode = ExecuteNonQueryTxForZeroOrMoreRowsAffected(objSqlTrans, spName1, oDtTbElemsToSp1)

                If iErrorCode = 0 Then
                    iErrorCode = ExecuteNonQueryTxForZeroOrMoreRowsAffected(objSqlTrans, spName2, oDtTbElemsToSp2)
                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return iErrorCode

        End Function

        Public Overloads Shared Function ExecuteNonQueryWithReturnValueTx(ByVal objSqlTrans As SqlTransaction, ByVal spName As String, ByRef dataRow As DataRow) As Integer

            If (objSqlTrans Is Nothing OrElse objSqlTrans.Connection.State = ConnectionState.Closed) Then Throw New ArgumentNullException("SqlTransaction.objSqlTrans")
            If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

            Dim iRowAffected As Integer
            Dim iErrCode As Integer
            Dim commandParameters As SqlParameter()

            ' If the row has values, the store procedure parameters must be initialized
            If (Not dataRow Is Nothing) Then  ' AndAlso dataRow.ItemArray.Length > 0) Then
                Try
                    ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                    commandParameters = SqlHelperParameterCache.GetSpParameterSet(objSqlTrans.Connection, spName, True)


                    ' Set the parameters values
                    Comun.CGCommonDb.AssignParameterValues(commandParameters, dataRow)

                    iRowAffected = SqlHelper.ExecuteNonQuery(objSqlTrans, CommandType.StoredProcedure, spName, commandParameters)

                    ' iErrCode  = 0: Ok , -1: No encontrado, -2: duplicado
                    If iRowAffected = 0 Then
                        iErrCode = -1 ' no encontrado
                    ElseIf iRowAffected > 1 Then
                        iErrCode = -2 'duplicado pero sin CONSTRAIN en la tabla
                    Else
                        iErrCode = Integer.Parse(commandParameters(0).Value) ' el return value
                    End If

                Catch ex As SqlException

                    objSqlTrans.Rollback()

                    If ex.Number = 2627 Then
                        iErrCode = -2     ' registro duplicado
                    Else
                        Throw ex
                    End If

                Catch ex As Exception

                    objSqlTrans.Rollback()
                    Throw ex
                End Try
            End If

            Return iErrCode

        End Function

        Public Overloads Shared Function ExecuteNonQueryWithReturnValueTx(ByVal objSqlTrans As SqlTransaction, ByVal spName As String, ByVal ParamArray parameterValues() As Object) As Integer

            If (objSqlTrans Is Nothing OrElse objSqlTrans.Connection.State = ConnectionState.Closed) Then Throw New ArgumentNullException("SqlTransaction.objSqlTrans")
            If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

            Dim iRowAffected As Integer
            Dim iErrCode As Integer
            Dim commandParameters As SqlParameter()
            'Dim commandParameterAux As SqlParameter

            ' If the row has values, the store procedure parameters must be initialized
            Try
                ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(objSqlTrans.Connection, spName, True)

                ' Set the parameters values
                Comun.CGCommonDb.AssignParameterValues(commandParameters, parameterValues)

                iRowAffected = SqlHelper.ExecuteNonQuery(objSqlTrans, CommandType.StoredProcedure, spName, commandParameters)

                ' iErrCode  = 0: Ok , -1: No encontrado, -2: duplicado
                If iRowAffected = 0 Then
                    iErrCode = -1 ' no encontrado
                ElseIf iRowAffected > 1 Then
                    iErrCode = -2 'duplicado pero sin CONSTRAIN en la tabla
                Else
                    iErrCode = Integer.Parse(commandParameters(0).Value) ' el return value
                End If

            Catch ex As SqlException

                objSqlTrans.Rollback()

                If ex.Number = 2627 Then
                    iErrCode = -2     ' registro duplicado
                Else
                    Throw ex
                End If

            Catch ex As Exception

                objSqlTrans.Rollback()
                Throw ex
            End Try

            Return iErrCode

        End Function

        Public Overloads Shared Function ExecuteNonQueryWithOneInputOutputParameterTx(ByVal objSqlTrans As SqlTransaction, ByVal spName As String, ByVal ParamArray parameterValues() As Object) As Integer

            If (objSqlTrans Is Nothing OrElse objSqlTrans.Connection.State = ConnectionState.Closed) Then Throw New ArgumentNullException("SqlTransaction.objSqlTrans")
            If (spName Is Nothing OrElse spName.Length = 0) Then Throw New ArgumentNullException("spName")

            Dim iRowAffected As Integer
            Dim iErrCode As Integer
            Dim commandParameters As SqlParameter()
            Dim commandParameterAux As SqlParameter
            Dim iInputOutputParameters As Integer = 0
            Dim iIndexOfInputOutputParameter As Integer
            Dim i As Integer


            ' If the row has values, the store procedure parameters must be initialized
            Try
                ' Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(objSqlTrans.Connection, spName, True)

                ' Set the parameters values
                Comun.CGCommonDb.AssignParameterValues(commandParameters, parameterValues)

                For i = 0 To commandParameters.Length - 1
                    commandParameterAux = commandParameters(i)
                    If commandParameterAux.Direction = ParameterDirection.InputOutput Then
                        iIndexOfInputOutputParameter = i
                        iInputOutputParameters = iInputOutputParameters + 1
                    End If
                Next

                If iInputOutputParameters > 1 Then
                    iErrCode = -3
                    Exit Try
                ElseIf iInputOutputParameters = 0 Then
                    iErrCode = -4
                    Exit Try
                End If

                iRowAffected = SqlHelper.ExecuteNonQuery(objSqlTrans, CommandType.StoredProcedure, spName, commandParameters)

                ' iErrCode  = 0: Ok , -1: No encontrado, -2: duplicado
                If iRowAffected = 0 Then
                    iErrCode = -1 ' no encontrado
                ElseIf iRowAffected > 1 Then
                    iErrCode = -2 'duplicado pero sin CONSTRAIN en la tabla
                Else
                    'iErrCode = Integer.Parse(commandParameters(commandParameters.Length - 1).Value) ' valor 
                    iErrCode = Integer.Parse(commandParameters(iIndexOfInputOutputParameter).Value)
                End If

            Catch ex As SqlException

                objSqlTrans.Rollback()

                If ex.Number = 2627 Then
                    iErrCode = -2     ' registro duplicado
                Else
                    Throw ex
                End If

            Catch ex As Exception

                objSqlTrans.Rollback()
                Throw ex
            End Try

            Return iErrCode

        End Function

        Public Shared Function ExecuteNonQueryTx(ByVal objSqlTrans As SqlTransaction, ByVal spName As String, ByVal ParamArray parameterValues() As Object) As Integer
            Dim iRowAffected As Integer
            Dim intErrCode As Integer
            'Dim oDataRow As DataRow

            Try
                iRowAffected = SqlHelper.ExecuteNonQuery(objSqlTrans, spName, parameterValues)

                If iRowAffected <> 1 Then
                    intErrCode = -1
                End If
            Catch ex As SqlException
                If ex.Number = 2627 Then
                    intErrCode = -2     ' registro duplicado
                Else
                    Throw ex
                End If
            Catch ex As Exception
                Throw ex
            End Try

            Return intErrCode

        End Function

        Public Shared Function ExecuteNonQueryTxForZeroOrMoreRowsAffected(ByVal objSqlTrans As SqlTransaction, ByVal spName As String, ByVal ParamArray parameterValues() As Object) As Integer
            Dim iRowAffected As Integer
            Dim intErrCode As Integer
            'Dim oDataRow As DataRow

            Try
                iRowAffected = SqlHelper.ExecuteNonQuery(objSqlTrans, spName, parameterValues)

                If iRowAffected < 0 Then
                    intErrCode = -1
                End If

            Catch ex As SqlException
                If ex.Number = 2627 Then
                    intErrCode = -2     ' registro duplicado
                Else
                    Throw ex
                End If
            Catch ex As Exception
                Throw ex
            End Try

            Return intErrCode

        End Function

        Public Shared Function ExecuteNonQueryTxForZeroOrMoreRowsAffected(ByVal objSqlTrans As SqlTransaction, ByVal spName As String, ByVal oTablaElementos As DataTable) As Integer
            Dim iRowAffected As Integer
            Dim intErrCode As Integer
            Dim oDataRow As DataRow

            Try

                For Each oDataRow In oTablaElementos.Rows

                    iRowAffected = SqlHelper.ExecuteNonQuery(objSqlTrans, spName, oDataRow.ItemArray)

                    If iRowAffected < 0 Then
                        intErrCode = -1
                        Exit For
                    End If
                Next

            Catch ex As Exception
                Throw ex
            End Try

            Return intErrCode

        End Function
    End Class
End Namespace
