/** Response body from POST /api/resume/analyze */
export interface ResumeAnalysisResponse {
  summary: string;
  detectedSkills: string[];
  missingSkills: string[];
  feedback: string[];
  linkedInHeadline: string;
  improvementSuggestions: string[];
}
