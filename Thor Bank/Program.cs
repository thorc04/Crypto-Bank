using Thor_Bank.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddRazorPages().Services.AddSingleton<IHelper, Helper>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24); options.Cookie.HttpOnly = true; options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/{0}");

app.MapRazorPages();

app.Run();
