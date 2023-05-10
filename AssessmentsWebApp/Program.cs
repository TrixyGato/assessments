using AssessmentsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.UseStaticFiles();

//app.UseStaticFiles(new StaticFileOptions()
//{
//    FileProvider = new PhysicalFileProvider(
//                        Path.Combine(Directory.GetCurrentDirectory(), @"Images")),
//    RequestPath = new PathString("/app-images")
//});

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//           Path.Combine(builder.Environment.ContentRootPath, "MyStaticFiles")),
//    RequestPath = "/StaticFiles"
//});



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
});

app.MapGet("api/student/{id}", (string id) =>
{
    var context = new AssessmentsDbContext();

    var student = context.Students.Where(s => s.Id == id).FirstOrDefault();

    if (student is null)
        return Results.NotFound($"Student with id {id} doesnt't exist.");

    return Results.Ok(student);
});

app.MapGet("api/grading", () =>
{
    var context = new AssessmentsDbContext();
    return context.Gradings;
});

app.MapGet("api/grading/{id}", (int id) =>
{
    var context = new AssessmentsDbContext();

    var grading = context.Gradings.Where(g => g.Id == id).FirstOrDefault(); ;

    if (grading is null)
        return Results.NotFound($"Grading with id {id} doesnt't exist.");

    return Results.Ok(grading);
});

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
});


app.MapPost("api/grading/", (Grading grading) =>
{
    var context = new AssessmentsDbContext();

    context.Add(grading);
    context.SaveChanges();

    return Results.Ok(grading);
});


app.MapGet("api/student&grading", () =>
{
    var context = new AssessmentsDbContext();

    var students = context.Students.ToList();

    var student_grading = new List<Student_Grading>();

    foreach (var student in students)
    {
        var sg = new Student_Grading()
        {
            StudentId = student.Id,
            Username = student.Username,
            Grade = context.Gradings.Where(g => g.StudentId == student.Id).FirstOrDefault()?.Grade,
            Comment = context.Gradings.Where(g => g.StudentId == student.Id).FirstOrDefault()?.Comment,
        };

        student_grading.Add(sg);
    }


    return Results.Ok(student_grading);
});

app.Run();