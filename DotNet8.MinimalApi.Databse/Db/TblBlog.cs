using System;
using System.Collections.Generic;

namespace DotNet8.EmailServiceMinimalApi.Databse.Db;

public partial class TblBlog
{
    public int BlogId { get; set; }

    public string BlogTitle { get; set; } = null!;

    public string BlogAuthor { get; set; } = null!;

    public string? BlogContent { get; set; }

    public string? UserId { get; set; }
}
