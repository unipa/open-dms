using OpenDMS.Domain.Enumerators;
using System;

public class FileProperty
{
    public int Id { get; set; }
    public int ImageId { get; set; }
    public string Name { get; set; }
    public string DocType { get; set; }
    public string Nr { get; set; }
    public string Date { get; set; }
    public string Owner { get; set; }
    public string Size { get; set; }
    public JobStatus StatusCode { get; set; }
    public string[] SignFields { get; set; }
    public string SignRoom { get; set; }
    public string SignUser { get; set; }
    public bool Excluded { get; set; }
    public string Motivation { get; set; }


}
