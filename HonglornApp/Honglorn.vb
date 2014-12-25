﻿Public Class Honglorn
  Private Shared _MySingletonInstance As Honglorn

  Private Const CsDataBaseName As String = "bjs"

  Private oMySqlHandler As MySqlHandler
  Private oExcelImporter As ExcelImporter

  ''' <summary>
  ''' Returns the singleton instance or creates a new one, if none is available.
  ''' </summary>
  ''' <param name="sServer">Name or IP address of the database server.</param>
  ''' <param name="sUser">Username used for login to the database server.</param>
  ''' <param name="sPassword">Password used for login to the database server.</param>
  ''' <returns>A singleton instance of type Honglorn.</returns>
  Public Shared ReadOnly Property Instance(sServer As String, sUser As String, sPassword As String) As Honglorn
    Get
      If IsNothing(_MySingletonInstance) Then
        Instance = New Honglorn(sServer, sUser, sPassword)
      Else
        Instance = _MySingletonInstance
      End If
    End Get
  End Property

  Private Sub New(sServer As String, sUser As String, sPassword As String)
    oMySqlHandler = New MySqlHandler(sServer, sUser, sPassword, CsDataBaseName)
    oExcelImporter = ExcelImporter.Instance
  End Sub

  ''' <summary>
  ''' Imports an Excel sheet containing data for multiple students into the database.
  ''' </summary>
  ''' <param name="sFilePath">The full path to the Excel file to be imported.</param>
  ''' <param name="iYear">The year in which the imported data is valid (relevant for mapping the courses).</param>
  ''' <remarks>todo: currently only works with a "perfect" Excel sheet</remarks>
  Public Sub ImportStudentCourseExcelSheet(sFilePath As String, iYear As Integer)
    Dim sCurSurname As String
    Dim sCurForename As String
    Dim sCurCourseName As String
    Dim eCurrentSex As MySqlHandler.Sex
    Dim iCurYearOfBirth As Integer

    Dim oDataTable As DataTable = oExcelImporter.GetStudentCourseDataTable(sFilePath)

    For Each oRow As DataRow In oDataTable.Rows
      sCurSurname = CStr(oRow(0))
      sCurForename = CStr(oRow(1))
      sCurCourseName = CStr(oRow(2))

      Select Case (CStr(oRow(3)))
        Case "M"
          eCurrentSex = MySqlHandler.Sex.Male
        Case "W"
          eCurrentSex = MySqlHandler.Sex.Female
      End Select

      iCurYearOfBirth = CInt(oRow(4))

      oMySqlHandler.ImportStudentData(sCurSurname, sCurForename, sCurCourseName, eCurrentSex, iCurYearOfBirth, iYear)
    Next
  End Sub

  Public Sub ImportSingleStudent(sSurname As String, sForename As String, sCourseName As String, eSex As MySqlHandler.Sex, iYearOfBirth As Integer, iYear As Integer)
    oMySqlHandler.ImportStudentData(sSurname, sForename, sCourseName, eSex, iYearOfBirth, iYear)
  End Sub
End Class
