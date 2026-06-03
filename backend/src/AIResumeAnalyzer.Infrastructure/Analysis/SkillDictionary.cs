namespace AIResumeAnalyzer.Infrastructure.Analysis;

internal static class SkillDictionary
{
    public static readonly IReadOnlyDictionary<string, string[]> RoleSkills =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["Software Engineer"] =
            [
                "C#", ".NET", "ASP.NET Core", "SQL", "REST API", "Git", "Unit Testing",
                "Entity Framework", "Docker", "Azure", "TypeScript", "JavaScript"
            ],
            ["Full Stack Developer"] =
            [
                "Angular", "React", "TypeScript", "JavaScript", "C#", ".NET", "Node.js",
                "HTML", "CSS", "SQL", "REST API", "Git", "Docker"
            ],
            ["Frontend Developer"] =
            [
                "Angular", "React", "Vue", "TypeScript", "JavaScript", "HTML", "CSS",
                "RxJS", "Webpack", "Git", "REST API"
            ],
            ["Backend Developer"] =
            [
                "C#", ".NET", "ASP.NET Core", "Java", "Python", "SQL", "PostgreSQL",
                "REST API", "Microservices", "Docker", "Redis", "Git"
            ],
            ["DevOps Engineer"] =
            [
                "CI/CD", "Docker", "Kubernetes", "Terraform", "Azure", "AWS",
                "Linux", "Git", "Monitoring", "Bash", "PowerShell"
            ],
            ["Data Analyst"] =
            [
                "SQL", "Excel", "Power BI", "Tableau", "Python", "Statistics",
                "Data Visualization", "ETL"
            ],
            ["Product Manager"] =
            [
                "Agile", "Scrum", "Roadmapping", "Jira", "User Research",
                "Stakeholder Management", "KPIs", "Product Strategy"
            ]
        };

    public static readonly (string Skill, string[] Keywords)[] TechnicalSkills =
    [
        ("C#", ["c#", "csharp"]),
        (".NET", [".net", "dotnet", "asp.net", "aspnet"]),
        ("ASP.NET Core", ["asp.net core", "aspnet core"]),
        ("Angular", ["angular"]),
        ("React", ["react", "react.js", "reactjs"]),
        ("Vue", ["vue", "vue.js", "vuejs"]),
        ("TypeScript", ["typescript", "ts"]),
        ("JavaScript", ["javascript", "js"]),
        ("Node.js", ["node.js", "nodejs", "node "]),
        ("Python", ["python"]),
        ("Java", ["java"]),
        ("SQL", ["sql", "t-sql", "tsql", "mysql", "postgresql", "postgres", "sql server"]),
        ("Entity Framework", ["entity framework", "ef core"]),
        ("REST API", ["rest api", "restful", "web api"]),
        ("Docker", ["docker"]),
        ("Kubernetes", ["kubernetes", "k8s"]),
        ("Azure", ["azure", "azure devops"]),
        ("AWS", ["aws", "amazon web services"]),
        ("Git", ["git", "github", "gitlab"]),
        ("CI/CD", ["ci/cd", "cicd", "continuous integration", "continuous delivery"]),
        ("Unit Testing", ["unit test", "unit testing", "xunit", "nunit", "jest"]),
        ("Power BI", ["power bi", "powerbi"]),
        ("Excel", ["excel"]),
        ("Agile", ["agile"]),
        ("Scrum", ["scrum"]),
        ("Microservices", ["microservice", "microservices"]),
        ("Redis", ["redis"]),
        ("Terraform", ["terraform"]),
        ("Linux", ["linux"]),
        ("HTML", ["html"]),
        ("CSS", ["css", "scss", "sass"]),
        ("RxJS", ["rxjs"]),
        ("Tableau", ["tableau"]),
        ("Statistics", ["statistics", "statistical"]),
        ("ETL", ["etl"]),
        ("Jira", ["jira"])
    ];

    public static string[] ResolveRoleSkills(string targetRole)
    {
        if (RoleSkills.TryGetValue(targetRole, out var exact))
            return exact;

        var normalized = targetRole.ToLowerInvariant();
        var match = RoleSkills
            .FirstOrDefault(kv =>
                normalized.Contains(kv.Key.ToLowerInvariant(), StringComparison.Ordinal) ||
                kv.Key.Contains(targetRole, StringComparison.OrdinalIgnoreCase));

        if (match.Value is { Length: > 0 })
            return match.Value;

        if (normalized.Contains("front"))
            return RoleSkills["Frontend Developer"];
        if (normalized.Contains("back") || normalized.Contains("api"))
            return RoleSkills["Backend Developer"];
        if (normalized.Contains("full") || normalized.Contains("stack"))
            return RoleSkills["Full Stack Developer"];
        if (normalized.Contains("data"))
            return RoleSkills["Data Analyst"];
        if (normalized.Contains("devops") || normalized.Contains("sre"))
            return RoleSkills["DevOps Engineer"];

        return RoleSkills["Software Engineer"];
    }
}
