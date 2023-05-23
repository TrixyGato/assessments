using AssessmentsWebApp.Models;
using AssessmentsWebApp.Models.POSTModels;
using AssessmentsWebApp.Models.RESPONSEModel;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Stream = AssessmentsWebApp.Models.Stream;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();


app.UseCors(builder => builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod());

app.UseSwagger(c =>
{
    c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
});

app.UseSwaggerUI(options =>
{
    //options.DefaultModelsExpandDepth(-1);
    options.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Assessments API V1");
    options.RoutePrefix = "api/swagger";
    options.InjectStylesheet("https://drive.google.com/uc?export=view&id=1SLY7ryk-Cw-o46Cprb5hprRh9cKpxfXN");
});

app.UseHttpsRedirection();

app.UseRouting();

app.MapGet("api/student", () =>
{
    var context = new AssessmentsDbContext();
    return context.Students;
}).WithTags("Get");

app.MapGet("api/student/{id}", (string id) =>
{
    var context = new AssessmentsDbContext();

    var student = context.Students.Where(s => s.Id == id).FirstOrDefault();

    if (student is null)
        return Results.NotFound($"Student with id {id} doesnt't exist.");

    return Results.Ok(student);
}).WithTags("Get");

app.MapDelete("api/student/{id}", (string id) =>
{
    var context = new AssessmentsDbContext();

    var student = context.Students.Where(g => g.Id == id).FirstOrDefault();

    if (student is null)
        return Results.NotFound($"Grading with id {id} doesnt't exist.");

    context.Students.Remove(student);

    context.SaveChanges();
    return Results.Ok($"Student with id {id} was removed.");
}).WithTags("Delete");

app.MapPost("api/student/", (Student student) =>
{
    var context = new AssessmentsDbContext();

    var isStudentUnique = context.Students.Where(s => s.Username == student.Username).FirstOrDefault();

    if (isStudentUnique != null)
    {
        return Results.BadRequest($"Student with username {student.Username} already exist.");
    }

    var id = Guid.NewGuid().ToString();
    var newStudent = student;
    newStudent.Id = id;

    context.Students.Add(newStudent);
    context.SaveChanges();

    return Results.Ok(newStudent);
}).WithTags("Post");

app.MapGet("api/grading", () =>
{
    var context = new AssessmentsDbContext();
    return context.Gradings;
}).WithTags("Get");

app.MapGet("api/grading/{id}", (string id) =>
{
    var context = new AssessmentsDbContext();

    var grading = context.Gradings.Where(g => g.Id == id).FirstOrDefault();

    if (grading is null)
        return Results.NotFound($"Grading with id {id} doesnt't exist.");

    return Results.Ok(grading);
}).WithTags("Get");


app.MapGet("api/gradings/{studentId}", (string studentId) =>
{
    var context = new AssessmentsDbContext();

    var grading = context.Gradings.Where(g => g.StudentId == studentId).ToList();

    if (grading is null)
        return Results.NotFound($"Grading with id {studentId} doesnt't exist.");

    foreach (var g in grading) {
        g.SubjectId = context.Subjects.Where(s => s.Id == g.SubjectId).FirstOrDefault().Name;
    }

    return Results.Ok(grading);
}).WithTags("Get");

app.MapPost("api/grading/", (Grading grading) =>
{
    var context = new AssessmentsDbContext();

    grading.Id = Guid.NewGuid().ToString();

    context.Gradings.Add(grading);
    context.SaveChanges();

    return Results.Ok(grading);
}).WithTags("Post");

app.MapPut("api/grading/{gradingId}", (Grading updatedGrading, string gradingId) =>
{
    var context = new AssessmentsDbContext();

    var grading = context.Gradings.Where(g => g.Id == gradingId).FirstOrDefault();

    if (grading is null)
        return Results.NotFound($"Grading with id {gradingId} doesnt't exist.");


    grading.Grade = updatedGrading.Grade;
    grading.Comment = updatedGrading.Comment;

    context.Entry(grading).State = EntityState.Modified;
    context.SaveChanges();

    return Results.Ok(grading);
}).WithTags("Put");



app.MapDelete("api/grading/{id}", (string id) =>
{
    var context = new AssessmentsDbContext();

    var grading = context.Gradings.Where(g => g.Id == id).FirstOrDefault();

    if (grading is null)
        return Results.NotFound($"Grading with id {id} doesnt't exist.");

    context.Gradings.Remove(grading);

    context.SaveChanges();
    return Results.Ok($"Grading with id {id} was removed.");
}).WithTags("Delete");


app.MapDelete("api/stream/{id}", (string id) =>
{
    var context = new AssessmentsDbContext();

    var stream = context.Streams.Where(g => g.Id == id).FirstOrDefault();

    if (stream is null)
        return Results.NotFound($"Stream with id {id} doesnt't exist.");

    context.Streams.Remove(stream);

    context.SaveChanges();
    return Results.Ok($"Stream with id {id} was removed.");
}).WithTags("Delete");


app.MapPost("api/start_stream/", (Stream stream) =>
{
    var context = new AssessmentsDbContext();

    var newStream = new Stream();

    newStream.DateStart = DateTime.Now;
    newStream.Id = Guid.NewGuid().ToString();
    newStream.Name = stream.Name;


    context.Streams.Add(newStream);
    context.SaveChanges();

    return Results.Ok(newStream);
}).WithTags("Post");

app.MapPost("api/end_stream/", (Stream stream) =>
{
    var context = new AssessmentsDbContext();

    var foundStream = context.Streams.Where(g => g.Id == stream.Id).FirstOrDefault();

    if (foundStream is null)
        return Results.NotFound($"Stream with id {stream.Id} doesnt't exist.");


    foundStream.DateEnd = DateTime.Now;

    context.Entry(foundStream).State = EntityState.Modified;
    context.SaveChanges();

    return Results.Ok(foundStream);
}).WithTags("Post");


app.MapPost("api/teacher/", (Teacher teacher) =>
{
    var context = new AssessmentsDbContext();

    var isTeacherUnique = context.Teachers.Where(s => s.Username == teacher.Username).FirstOrDefault();

    if (isTeacherUnique != null)
    {
        return Results.BadRequest($"Teacher with username {teacher.Username} already exist.");
    }

    teacher.Id = Guid.NewGuid().ToString();  

    context.Teachers.Add(teacher);
    context.SaveChanges();

    return Results.Ok(teacher);
}).WithTags("Post");

app.MapPost("api/subject/", (Subject subject) =>
{
    var context = new AssessmentsDbContext(); 

    subject.Id = Guid.NewGuid().ToString();

    context.Subjects.Add(subject);
    context.SaveChanges();

    return Results.Ok(subject);
}).WithTags("Post");


app.MapDelete("api/teacher/{id}", (string id) =>
{
    var context = new AssessmentsDbContext();

    var teacher = context.Teachers.Where(g => g.Id == id).FirstOrDefault();

    if (teacher is null)
        return Results.NotFound($"Teacher with id {id} doesnt't exist.");

    context.Teachers.Remove(teacher);

    context.SaveChanges();
    return Results.Ok($"Teacher with id {id} was removed.");
}).WithTags("Delete");


app.MapDelete("api/subject/{id}", (string id) =>
{
    var context = new AssessmentsDbContext();

    var subject = context.Subjects.Where(g => g.Id == id).FirstOrDefault();

    if (subject is null)
        return Results.NotFound($"Teacher with id {id} doesnt't exist.");

    context.Subjects.Remove(subject);

    context.SaveChanges();
    return Results.Ok($"Subject with id {id} was removed.");
}).WithTags("Delete");

app.MapGet("api/teacher/", () =>
{
    var context = new AssessmentsDbContext();

    return Results.Ok(context.Teachers);
}).WithTags("Get");

app.MapGet("api/stream/", () =>
{
    var context = new AssessmentsDbContext();

    return Results.Ok(context.Streams);
}).WithTags("Get");


app.MapGet("api/subject/", () =>
{
    var context = new AssessmentsDbContext();

    return Results.Ok(context.Subjects);
}).WithTags("Get");


app.MapPost("api/stream_student/", (StreamStudentModel streamStudentModel) =>
{
    var context = new AssessmentsDbContext();

    var isStudentUnique = context.Students.Where(s => s.Username == streamStudentModel.Student.Username).FirstOrDefault();

    if (isStudentUnique != null)
    {
        var streamStudent = new StreamStudent();

        streamStudent.StudentId = isStudentUnique.Id;
        streamStudent.StreamId = streamStudentModel.StreamId;
        streamStudent.Id = Guid.NewGuid().ToString();

        context.StreamStudents.Add(streamStudent);
        context.SaveChanges();

        return Results.Ok(streamStudent);
    }

    var newStudent = streamStudentModel.Student;
    newStudent.Id = Guid.NewGuid().ToString();

    context.Students.Add(newStudent);

    var newStreamStudent = new StreamStudent();

    newStreamStudent.StudentId = newStudent.Id;
    newStreamStudent.StreamId = streamStudentModel.StreamId;
    newStreamStudent.Id = Guid.NewGuid().ToString();

    context.StreamStudents.Add(newStreamStudent);
    context.SaveChanges();

    return Results.Ok(newStreamStudent);

}).WithTags("Post");

app.MapGet("api/student_grading/{streamId}", (string streamId) =>
{
    var context = new AssessmentsDbContext();

    var foundStream = context.Streams.Where(g => g.Id == streamId).FirstOrDefault();

    if (foundStream is null)
    {
        return Results.NotFound($"Stream with id {streamId} doesnt't exist.");
    }

    var streamStudents = context.StreamStudents.Where(s=>s.StreamId == streamId).ToList();

    var student_grading = new List<Student_Grading>();

    foreach (var student in streamStudents)
    {
        var foundStudent = context.Students.Where(s => s.Id == student.StudentId).FirstOrDefault();

        var sg = new Student_Grading()
        {

            StudentId = foundStudent.Id,
            Username = foundStudent.Username,
            GradingId = context.Gradings.Where(g => g.StudentId == foundStudent.Id && g.StreamId == streamId).FirstOrDefault()?.Id,
            Grade = context.Gradings.Where(g => g.StudentId == foundStudent.Id && g.StreamId == streamId).FirstOrDefault()?.Grade,
            Comment = context.Gradings.Where(g => g.StudentId == foundStudent.Id && g.StreamId == streamId).FirstOrDefault()?.Comment,
        };

        student_grading.Add(sg);
    }
    return Results.Ok(student_grading);
}).WithTags("Get");

app.MapGet("api/teacher_gradings/{teacherId}", (string teacherId) =>
{
    var context = new AssessmentsDbContext();

    var teacherSubjects = context.Subjects.Where(s=> s.TeacherId==teacherId).ToList();

    List<string> subjectsList = teacherSubjects.Select(s => s.Id).ToList();

    var filteredGrading = context.Gradings.Where(g => subjectsList.Contains(g.SubjectId)).ToList();

    if (filteredGrading is null)
        return Results.NotFound($"Gradings with teacher id {teacherId} doesnt't exist.");


    List<Teacher_Gradings> result = new List<Teacher_Gradings>();

    foreach (var f in filteredGrading) {
        result.Add(new Teacher_Gradings
        {
            Grade = f.Grade,
            Comment = f.Comment,
            Date = f.Date,
            StudentName = context.Students.Where(s=> s.Id == f.StudentId).FirstOrDefault().Username,
            StreamName = context.Streams.Where(s => s.Id == f.StreamId).FirstOrDefault().Name,
            subjectName = context.Subjects.Where(s => s.Id == f.SubjectId).FirstOrDefault().Name
        });
    }

    return Results.Ok(result);
}).WithTags("Get");


app.Run();