using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

//app.MapGet("/", () => "Boa tarde!");
/*
app.MapGet("/user", () => new {Name = "Joilton Almeida", Age = 43});

app.MapGet("/AddHeader", (HttpResponse response) => {
    
    response.Headers.Add("Teste", "Joilton Almeida");

    return new {Name = "Joilton Almeida", Age = 43};

});
*/
//app.MapPost("/products", (ProductRequest productRequest, ApplicationDbContext context) => {
  //  
    // var category = context.Categories.Where(c => c.Id == productRequest.CategoryId).First();
//
    // var product = new Product {
//
    //     Code = productRequest.Code,
    //     Name = productRequest.Name,
    //     Description = productRequest.Description,
    //     Category = category
    // };
//
    // context.Products.Add(product);
//
    // return Results.Created($"/products/{product.Id}", product.Id);
//
//});

//Através de query param
app.MapGet("/products", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {
    return dateStart + " - " + dateEnd;
});

//Através de rota
app.MapGet("/products/{code}", ([FromRoute] string code) => {
    
    var product = ProductRepository.GetBy(code);

    if(product != null){

        return Results.Ok(product);
        
    }
    
    return Results.NotFound();

});

app.MapPut("/products", (Product product) => {
    var productSaved = ProductRepository.GetBy(product.Code);
    productSaved.Name = product.Name;

    return Results.Ok();

});

app.MapGet("/getproductbyheader", (HttpRequest request) => {
    return request.Headers["product-code"].ToString();
});

app.MapDelete("/products/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);
    ProductRepository.Remove(product);

    return Results.Ok();
});

app.MapGet("/configuration/database", (IConfiguration configuration) => {
    return Results.Ok($"{configuration["database:Connection"]}/{configuration["database:Port"]}");
});

app.Run();