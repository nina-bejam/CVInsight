/** ASP.NET Core validation error payload (400 Bad Request) */
export interface ValidationProblemResponse {
  title?: string;
  status?: number;
  detail?: string;
  errors?: Record<string, string[]>;
}
