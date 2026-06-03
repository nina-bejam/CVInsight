namespace AIResumeAnalyzer.Infrastructure.MockData;

public static class SampleResumes
{
    public const string SoftwareEngineer = """
        Alex Morgan
        Seattle, WA | alex.morgan@email.com | +1 555 010 2234

        SUMMARY
        Full stack software engineer with 6 years building enterprise web applications on .NET and Angular.

        EXPERIENCE
        Senior Software Engineer — Contoso Ltd (2020 – Present)
        - Led migration of monolith services to ASP.NET Core microservices, reducing deployment time by 40%
        - Built REST APIs with Entity Framework and SQL Server for 200k daily users
        - Mentored 3 junior developers on unit testing and code review practices

        Software Engineer — Fabrikam Inc (2018 – 2020)
        - Delivered Angular dashboards integrated with .NET backend services
        - Implemented CI/CD pipelines with Azure DevOps and Docker

        SKILLS
        C#, .NET, ASP.NET Core, Entity Framework, SQL, Angular, TypeScript, REST API, Git, Docker, Azure, Unit Testing

        EDUCATION
        B.S. Computer Science — University of Washington (2018)
        """;

    public const string DataAnalyst = """
        Jordan Lee
        Austin, TX | jordan.lee@email.com

        PROFILE
        Data analyst translating business questions into actionable insights.

        EXPERIENCE
        Data Analyst — Northwind Analytics (2021 – Present)
        - Built Power BI dashboards tracking revenue and churn KPIs
        - Wrote SQL queries for ETL pipelines and ad-hoc reporting
        - Partnered with product on A/B test analysis

        SKILLS
        SQL, Excel, Power BI, Python, Statistics, Data Visualization

        EDUCATION
        B.A. Economics — UT Austin (2021)
        """;
}
