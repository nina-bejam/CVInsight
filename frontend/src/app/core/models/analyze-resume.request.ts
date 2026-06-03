/** Request body for POST /api/resume/analyze */
export interface AnalyzeResumeRequest {
  resumeText: string;
  targetRole: string;
}
