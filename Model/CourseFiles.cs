using COURSEPROJECT.Model;
using System.Text.Json.Serialization;

public class CourseFile
{
    public int ID { get; set; }

    public string FileName { get; set; }     
    public string FileType { get; set; }     
    public string FileUrl { get; set; }      

    public int CourseMaterialId { get; set; }
    [JsonIgnore] 
    public CourseMaterial CourseMaterial { get; set; }
}

