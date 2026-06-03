# AI Resume Analyzer

A full-stack web app for reviewing CVs against a target job role. Paste a resume, pick the role you're hiring for, and get structured feedback in a few seconds.

The analysis runs locally on your machine using a rule-based engine — skill matching, section checks, and plain-language suggestions. No API keys, no cloud AI bill, no signup.

> **Note:** This project uses a local rule-based analysis engine. It does not require OpenAI, ChatGPT, Claude, Gemini, or any paid AI service. The backend is structured so you can plug in an external provider later without rewriting the UI or controllers.

---

## Overview

Hiring teams and candidates often need quick, consistent feedback on whether a CV fits a role. This app fills that gap for demos, portfolios, and internal tooling: you send resume text and a target role to a .NET API, and an Angular dashboard displays the results.

The feedback is specific ("Your Angular experience shows up throughout the CV") rather than generic filler. That comes from keyword dictionaries, role skill maps, and template rules — not from an LLM.

---

## Features

- Paste-and-analyze workflow with form validation
- Target role comparison with detected and missing skills
- Candidate summary and LinkedIn headline suggestion
- Actionable CV feedback and improvement tips
- Loading states and user-friendly error messages (no stack traces in the UI)
- Swagger API docs in development
- CORS configured for local Angular development

---

## Tech Stack

| Layer | Technologies |
|-------|----------------|
| **Frontend** | Angular 20, Angular Material, Reactive Forms, HttpClient, Signals |
| **Backend** | .NET 8 Web API, Clean Architecture, Dependency Injection |
| **Analysis** | Rule-based engine (skill dictionaries, keyword matching, parsers) |

---

## Architecture

```
┌─────────────────────┐         POST /api/resume/analyze         ┌──────────────────────┐
│  Angular Frontend   │  ──────────────────────────────────────► │  .NET 8 Web API      │
│  (port 4200)        │         JSON request / response            │  (port 7287)         │
└─────────────────────┘                                          └──────────┬───────────┘
                                                                              │
                                                                   ┌──────────▼───────────┐
                                                                   │ ResumeAnalyzerService │
                                                                   └──────────┬───────────┘
                                                                              │
                                                                   ┌──────────▼───────────┐
                                                                   │ IResumeAnalysisEngine │
                                                                   │ (rule-based today)    │
                                                                   └──────────────────────┘
```

**Backend layers**

- `AIResumeAnalyzer.Api` — controllers, CORS, Swagger, exception handling
- `AIResumeAnalyzer.Application` — DTOs, `ResumeAnalyzerService`, interfaces
- `AIResumeAnalyzer.Domain` — domain models
- `AIResumeAnalyzer.Infrastructure` — parsers, skill dictionaries, rule-based engine

**Frontend structure**

- `core/` — models, `ResumeAnalyzerService`, error handling, environment config
- `features/resume-analyzer/` — main dashboard component
- `environments/` — API base URL per build configuration

To swap in a future AI provider, implement `IResumeAnalysisEngine` and register it in `Infrastructure/DependencyInjection.cs`. Controllers stay the same.

---

## Screenshots

_Add screenshots here after running the app locally._

| Screen | Description |
|--------|-------------|
| `docs/screenshot-form.png` | Analysis form with resume text and target role |
| `docs/screenshot-results.png` | Results panel with skills, feedback, and suggestions |

```bash
# Suggested: capture at 1440×900 after loading the sample resume
```

---

## How to run the backend

**Prerequisites:** [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) and runtime

```bash
cd backend/src/AIResumeAnalyzer.Api
dotnet run
```

| Resource | URL |
|----------|-----|
| API (HTTPS) | https://localhost:7287 |
| API (HTTP) | http://localhost:5136 |
| Swagger | https://localhost:7287/swagger |

Trust the dev certificate if the browser warns about HTTPS:

```bash
dotnet dev-certs https --trust
```

---

## How to run the frontend

**Prerequisites:** [Node.js 20+](https://nodejs.org/)

```bash
cd frontend
npm install
npm start
```

Open **http://localhost:4200**

The dev server proxies `/api` to the backend (see `frontend/proxy.conf.json`). Start the API first, then the Angular app.

**Production build**

```bash
cd frontend
npm run build
```

Output: `frontend/dist/ai-resume-analyzer-frontend`

---

## API documentation

### Analyze resume

**`POST /api/resume/analyze`**

**Request**

```json
{
  "resumeText": "Alex Morgan\nSenior Software Engineer...",
  "targetRole": "Full Stack Developer"
}
```

**Response** `200 OK`

```json
{
  "summary": "Alex Morgan looks like a Senior Software Engineer with about 6 years...",
  "detectedSkills": ["Angular", "C#", ".NET", "SQL"],
  "missingSkills": ["Node.js", "React"],
  "feedback": [
    "Your Angular experience is visible throughout the CV.",
    "I would add more information about the backend projects you worked on."
  ],
  "linkedInHeadline": "Full Stack Developer · Angular · 6+ years",
  "improvementSuggestions": [
    "If you have Node.js experience, mention it in a recent role or project..."
  ]
}
```

**Validation** `400 Bad Request` — returns ASP.NET `ValidationProblemDetails` with field errors.

**Server error** `500` — generic problem response; details are logged server-side only.

**Example (curl)**

```bash
curl -k -X POST https://localhost:7287/api/resume/analyze \
  -H "Content-Type: application/json" \
  -d "{\"resumeText\":\"Jane Doe\nSoftware Engineer with 5 years in C# and Angular...\",\"targetRole\":\"Full Stack Developer\"}"
```

---

## Project structure

```
AIResumeAnalyzer/
├── backend/
│   └── src/
│       ├── AIResumeAnalyzer.Api/
│       ├── AIResumeAnalyzer.Application/
│       ├── AIResumeAnalyzer.Domain/
│       └── AIResumeAnalyzer.Infrastructure/
├── frontend/
│   └── src/
│       ├── app/
│       │   ├── core/
│       │   │   ├── constants/user-messages.ts
│       │   │   ├── errors/
│       │   │   ├── models/
│       │   │   └── services/resume-analyzer.service.ts
│       │   └── features/resume-analyzer/
│       └── environments/
├── README.md
└── AIResumeAnalyzer.sln
```

---

## Environment configuration (frontend)

| File | Purpose |
|------|---------|
| `environment.ts` | Production defaults |
| `environment.development.ts` | Local dev (`ng serve` uses file replacement) |

Both set `apiBaseUrl: '/api'` and `resumeAnalyzePath: '/resume/analyze'`. The dev proxy forwards those calls to the .NET API.

---

## Future improvements

- [ ] PDF and DOCX upload instead of paste-only
- [ ] Save analysis history (SQLite or PostgreSQL)
- [ ] Optional `IResumeAnalysisEngine` backed by an external AI provider
- [ ] Role templates managed in admin UI
- [ ] Docker Compose for one-command local setup
- [ ] Authentication for multi-user HR teams

---

## License

MIT — suitable for portfolio and learning use.
