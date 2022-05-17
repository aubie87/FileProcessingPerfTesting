using FileProcessingPerfTesting.Core.Models;
using System.Diagnostics;
using System.Xml;

namespace FileProcessingPerfTesting.XmlParser.Readers;

internal class TransactionReader
{
    /// <summary>
    /// Loads the Transactions collection.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal static List<Transaction> Load(XmlReader reader)
    {
        if (reader.NodeType != XmlNodeType.Element || reader.Name != "Transactions")
        {
            throw new InvalidOperationException($"Expected <Transactions> element, found {reader.NodeType}");
        }

        var transactions = new List<Transaction>();
        if (reader.IsEmptyElement)
        {
            return transactions;
        }

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element
                && reader.Name == "Transaction")
            {
                transactions.Add(LoadTransaction(reader));
            }
            else if (reader.NodeType is XmlNodeType.Element
                or XmlNodeType.EndElement)
            {
                break;
            }
        }

        return transactions;
    }

    private static Transaction LoadTransaction(XmlReader reader)
    {
        if (reader.NodeType != XmlNodeType.Element || reader.Name != "Transaction")
        {
            throw new InvalidOperationException($"Expected <Transactions> element, found {reader.NodeType}");
        }

        var transaction = new Transaction();
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                switch (reader.Name)
                {
                    case "Id":
                        transaction.OriginalId = reader.ReadElementContentAsInt();
                        break;
                    case "TransType":
                        transaction.TransactionType = reader.ReadElementContentAsString();
                        break;
                    case "Amount":
                        transaction.Amount = reader.ReadElementContentAsDecimal();
                        break;
                    case "Date":
                        transaction.Date1 = reader.ReadElementContentAsDateTime();
                        transaction.Date2 = DateOnly.FromDateTime(transaction.Date1);
                        break;
                    case "Description":
                        transaction.Description = reader.ReadElementContentAsString();
                        break;

                    default:
                        Debug.WriteLine($"Found unsupported element in transaction: {reader.Name}");
                        break;
                }
            }
            else if (reader.NodeType == XmlNodeType.EndElement)
            {
                break;
            }
        }

        return transaction;
    }
}