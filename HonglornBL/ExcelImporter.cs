﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using HonglornBL.Models.Entities;
using Microsoft.Office.Interop.Excel;
using static HonglornBL.Prerequisites;

namespace HonglornBL
{
    static class ExcelImporter
    {
        const string SurnameHeaderColumn = "Nachname";
        const string ForenameHeaderColumn = "Vorname";
        const string CoursenameHeaderColumn = "Kursbezeichnung";
        const string SexHeaderColumn = "Geschlecht";
        const string YearofbirthHeaderColumn = "Geburtsjahr";

        static readonly Dictionary<string, Sex> SexDictionary = new Dictionary<string, Sex>
        {
            {"W", Sex.Female},
            {"M", Sex.Male}
        };

        /// <summary>
        ///     Takes a file path as a string as input and returns a DataTable containing the extracted data.
        ///     Designed to work together with the DBHandler to import the data into the database.
        /// </summary>
        /// <param name="filePath">The file path of the Excel-file containing the relevant data.</param>
        internal static ICollection<Tuple<Student, string>> GetStudentDataTableFromExcelFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path is null, empty or consist of only white-space characters.");
            }

            ICollection<Tuple<Student, string>> extractedStudents = new List<Tuple<Student, string>>();

            _Application excelInstance = null;
            _Workbook workbook = null;

            try
            {
                excelInstance = new Application();
                workbook = excelInstance.Workbooks.Open(filePath);
                _Worksheet worksheet = workbook.Worksheets[1];

                ValidateHeaderRow(worksheet);

                //import content
                //todo: handle half-empty rows correctly! (Error message and removal from DataTable, so it's not imported)
                int rowIdx = 2;
                bool rowIsEmpty = false;
                while (!rowIsEmpty)
                {
                    ICollection<string> row = worksheet.GetRow(rowIdx, 5);
                    rowIsEmpty = row.All(string.IsNullOrWhiteSpace);

                    if (!rowIsEmpty)
                    {
                        string surname = row.ElementAt(0);
                        string forename = row.ElementAt(1);
                        string courseName = row.ElementAt(2);
                        string rawSex = row.ElementAt(3);
                        string rawYearOfBirth = row.ElementAt(4);

                        Sex sex;
                        if (SexDictionary.TryGetValue(rawSex, out sex))
                        {
                            short yearOfBirth = Convert.ToInt16(rawYearOfBirth);
                            if (IsValidYear(yearOfBirth))
                            {
                                Student student = new Student
                                {
                                    Surname = surname,
                                    Forename = forename,
                                    Sex = sex,
                                    YearOfBirth = yearOfBirth
                                };

                                extractedStudents.Add(new Tuple<Student, string>(student, courseName));
                            }
                        }
                    }

                    rowIdx++;
                }
            }
            finally
            {
                workbook?.Close();
                excelInstance?.Quit();

                SafelyReleaseComObject(workbook);
                SafelyReleaseComObject(excelInstance);
            }

            return extractedStudents;
        }

        static void SafelyReleaseComObject(object obj)
        {
            if (obj != null)
            {
                Marshal.ReleaseComObject(obj);
            }
        }

        static string GetTextFromRange(this _Worksheet sheet, string range)
        {
            return sheet.Range[range].Text;
        }

        static char GetColumnName(int colIdx)
        {
            if (colIdx > 25)
            {
                throw new NotImplementedException($"The {nameof(GetColumnName)}-function is only designed for column indices up to 25.");
            }

            return Alphabet[colIdx];
        }

        static ICollection<string> GetRow(this _Worksheet sheet, int rowIdx, int length)
        {
            ICollection<string> row = new List<string>();

            for (int colIdx = 0; colIdx < length; colIdx++)
            {
                row.Add(sheet.GetTextFromCell(colIdx, rowIdx));
            }

            return row;
        }

        static string GetTextFromCell (this _Worksheet sheet, int colIdx, int rowIdx)
        {
            return sheet.Range[$"{GetColumnName(colIdx)}{rowIdx}"].Text;
        }

        static void ValidateHeaderRow(_Worksheet sheet)
        {
            ValidateString(SurnameHeaderColumn, sheet.GetTextFromRange("A1"));
            ValidateString(ForenameHeaderColumn, sheet.GetTextFromRange("B1"));
            ValidateString(CoursenameHeaderColumn, sheet.GetTextFromRange("C1"));
            ValidateString(SexHeaderColumn, sheet.GetTextFromRange("D1"));
            ValidateString(YearofbirthHeaderColumn, sheet.GetTextFromRange("E1"));
        }

        static void ValidateString(string expected, string actual)
        {
            if (!actual.Equals(expected, StringComparison.InvariantCulture))
            {
                throw new ArgumentException($"Header row was in unexpected condition. Expected {expected} but was {actual}.");
            }
        }
    }
}