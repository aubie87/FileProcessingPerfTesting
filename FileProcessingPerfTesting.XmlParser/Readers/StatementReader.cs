using FileProcessingPerfTesting.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileProcessingPerfTesting.XmlParser.Readers;
public class StatementReader
{
    internal static Statement Load(XmlReader reader)
    {
        if (reader.NodeType != XmlNodeType.Element || reader.Name != "Statement")
        {
            throw new InvalidOperationException($"Expected <Statement> element, found {reader.NodeType}");
        }

        var statement = new Statement();
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                switch (reader.Name)
                {
                    case "Name":
                        statement.Name = reader.ReadElementContentAsString();
                        break;
                    case "Id":
                        statement.OriginalId = reader.ReadElementContentAsInt();
                        break;
                    case "AccountName":
                        statement.AccountName = reader.ReadElementContentAsString();
                        break;
                    case "AccountNumber":
                        statement.AccountNumber = reader.ReadElementContentAsString();
                        break;
                    case "Address1":
                        statement.Address1 = reader.ReadElementContentAsString();
                        break;
                    case "Address2":
                        statement.Address2 = reader.ReadElementContentAsString();
                        break;
                    case "Address3":
                        statement.Address3 = reader.ReadElementContentAsString();
                        break;
                    case "Address4":
                        statement.Address4 = reader.ReadElementContentAsString();
                        break;
                    case "Message":
                        statement.Message = reader.ReadElementContentAsString();
                        break;

                    case "Transactions":
                        statement.Transactions = TransactionReader.Load(reader);
                        if (!statement.Transactions.Any())
                        {
                            Debug.WriteLine("No transaction found.");
                        }
                        break;

                    case "Page":
                        var page = PageReader.Load(reader);
                        if(page.Lines.Any())
                        {
                            statement.Lines.AddRange(page.Lines);
                        }
                        else
                        {
                            Debug.WriteLine("No page lines found.");
                        }
                        break;

                    default:
                        Debug.WriteLine($"Uknown element: {reader.Name}");
                        break;
                }
            }
            else if (reader.NodeType == XmlNodeType.EndElement
                && reader.Name == "Statement")
            {
                break;
            }
        }

        return statement;
    }
}
